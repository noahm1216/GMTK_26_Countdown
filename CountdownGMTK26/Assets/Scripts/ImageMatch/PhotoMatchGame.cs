using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PhotoMatchGame : MonoBehaviour
{
    private enum GamePhase
    {
        Preview,
        Rearranging,
        Results
    }

    [System.Serializable]
    private class SpawnedShape
    {
        public RectTransform rectTransform;
        public DraggableShape draggable;
        public Vector2 targetPosition;
    }

    [Header("References")]
    [SerializeField] private RectTransform shapeRegion;
    [SerializeField] private List<DraggableShape> shapePrefabs;
    
    [Header("Results View")]
    [SerializeField] private RectTransform referenceResultRegion;
    [SerializeField] private RectTransform playerResultRegion;

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text instructionText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Button submitButton;

    [Header("Level Settings")]
    [Min(1)]
    [SerializeField] private int numberOfShapes = 8;

    [Min(0.1f)]
    [SerializeField] private float previewDuration = 5f;

    [Min(0.1f)]
    [SerializeField] private float arrangementDuration = 20f;

    [Header("Random Placement")]
    [Tooltip("Prevents shapes from spawning directly against the region's edges.")]
    [SerializeField] private float edgePadding = 10f;

    [Tooltip("How far a shape should move when positions are randomized.")]
    [SerializeField] private float minimumShuffleDistance = 100f;

    [Tooltip("Attempts made to find a shuffled position far from the target.")]
    [SerializeField] private int shufflePositionAttempts = 20;

    [Header("Colors")]
    [SerializeField] private List<Color> possibleColors = new()
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        new Color(1f, 0.4f, 0.8f),
        new Color(0.5f, 0.2f, 1f),
        new Color(1f, 0.5f, 0f)
    };

    [Header("Scoring")]
    [Tooltip("A shape at least this far from its target receives 0% for that shape.")]
    [Min(1f)]
    [SerializeField] private float zeroScoreDistance = 300f;

    [Tooltip("Positions closer than this are treated as perfect.")]
    [Min(0f)]
    [SerializeField] private float perfectDistance = 10f;

    private readonly List<SpawnedShape> spawnedShapes = new();
    private readonly List<GameObject> referenceVisuals = new();

    private GamePhase currentPhase;
    private Coroutine gameRoutine;
    private float remainingTime;

    private RectTransform PlayerBoard =>
        playerResultRegion != null ? playerResultRegion : shapeRegion;

    private RectTransform ReferenceBoard =>
        referenceResultRegion != null ? referenceResultRegion : shapeRegion;

    private void Awake()
    {
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(SubmitArrangement);
            submitButton.gameObject.SetActive(false);
        }

        if (scoreText != null)
            scoreText.gameObject.SetActive(false);

        SetReferenceBoardVisible(false);
    }

    private void Start()
    {
        StartLevel();
    }
    
    public void Update()
    {
        bool reloadKeyPressed = Keyboard.current.rKey.wasPressedThisFrame;
        if (reloadKeyPressed)
        {
            SceneManager.LoadScene("ImageMatchPrototype");
        }
    }

    public void StartLevel()
    {
        if (gameRoutine != null)
            StopCoroutine(gameRoutine);

        ClearReferenceVisuals();
        ClearShapes();

        SetReferenceBoardVisible(false);

        if (playerResultRegion != null)
            playerResultRegion.gameObject.SetActive(true);

        SpawnShapes();

        gameRoutine = StartCoroutine(RunLevel());
    }

    private void SpawnShapes()
    {
        RectTransform playerBoard = PlayerBoard;

        if (playerBoard == null)
        {
            Debug.LogError("Photo Match Game: No Player Board has been assigned.");
            return;
        }

        if (shapePrefabs == null || shapePrefabs.Count == 0)
        {
            Debug.LogError("Photo Match Game: No shape prefabs have been assigned.");
            return;
        }

        for (int i = 0; i < numberOfShapes; i++)
        {
            DraggableShape prefab =
                shapePrefabs[Random.Range(0, shapePrefabs.Count)];

            DraggableShape shape =
                Instantiate(prefab, playerBoard);

            RectTransform shapeRect = shape.RectTransform;

            // Using centered anchors makes random anchored positions predictable.
            shapeRect.anchorMin = new Vector2(0.5f, 0.5f);
            shapeRect.anchorMax = new Vector2(0.5f, 0.5f);
            shapeRect.pivot = new Vector2(0.5f, 0.5f);
            shapeRect.localScale = Vector3.one;
            shapeRect.localRotation = Quaternion.identity;

            SetRandomColor(shape);

            Vector2 targetPosition = GetRandomPosition(shapeRect);
            shapeRect.anchoredPosition = targetPosition;

            shape.SetMovementRegion(playerBoard);
            shape.SetDraggable(false);
            shape.gameObject.SetActive(false);

            spawnedShapes.Add(new SpawnedShape
            {
                rectTransform = shapeRect,
                draggable = shape,
                targetPosition = targetPosition
            });
        }
    }

    private IEnumerator RunLevel()
    {
        currentPhase = GamePhase.Preview;

        if (instructionText != null)
            instructionText.text = "Memorize the reference!";

        if (scoreText != null)
            scoreText.gameObject.SetActive(false);

        if (submitButton != null)
            submitButton.gameObject.SetActive(false);

        SetAllShapesVisible(false);
        SetAllShapesDraggable(false);

        ShowReferenceBoard();

        yield return RunTimer(previewDuration);

        SetReferenceBoardVisible(false);

        ShuffleShapes();
        SetAllShapesVisible(true);

        currentPhase = GamePhase.Rearranging;

        if (instructionText != null)
            instructionText.text = "Recreate the reference on the empty board!";

        if (submitButton != null)
            submitButton.gameObject.SetActive(true);

        SetAllShapesDraggable(true);

        yield return RunTimer(arrangementDuration);

        SubmitArrangement();
    }

    private IEnumerator RunTimer(float duration)
    {
        remainingTime = duration;

        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;

            if (timerText != null)
                timerText.text = Mathf.CeilToInt(remainingTime).ToString();

            yield return null;
        }

        remainingTime = 0f;

        if (timerText != null)
            timerText.text = "0";
    }
    
    private void SetReferenceBoardVisible(bool isVisible)
    {
        if (referenceResultRegion != null)
            referenceResultRegion.gameObject.SetActive(isVisible);
    }
    
    private void SetAllShapesVisible(bool isVisible)
    {
        foreach (SpawnedShape shape in spawnedShapes)
        {
            if (shape.rectTransform != null)
                shape.rectTransform.gameObject.SetActive(isVisible);
        }
    }
    
    private void ShowReferenceBoard()
    {
        ClearReferenceVisuals();
        SetReferenceBoardVisible(true);

        foreach (SpawnedShape shape in spawnedShapes)
        {
            CreateReferenceShape(
                shape.rectTransform,
                shape.targetPosition
            );
        }
    }
    

    private void ShuffleShapes()
    {
        foreach (SpawnedShape shape in spawnedShapes)
        {
            Vector2 shuffledPosition =
                GetShuffledPosition(
                    shape.rectTransform,
                    shape.targetPosition
                );

            shape.rectTransform.anchoredPosition = shuffledPosition;
        }
    }

    private Vector2 GetShuffledPosition(
        RectTransform shape,
        Vector2 targetPosition)
    {
        Vector2 bestPosition = GetRandomPosition(shape);
        float bestDistance = Vector2.Distance(bestPosition, targetPosition);

        for (int attempt = 0; attempt < shufflePositionAttempts; attempt++)
        {
            Vector2 candidate = GetRandomPosition(shape);
            float distance = Vector2.Distance(candidate, targetPosition);

            if (distance >= minimumShuffleDistance)
                return candidate;

            if (distance > bestDistance)
            {
                bestDistance = distance;
                bestPosition = candidate;
            }
        }

        return bestPosition;
    }

    private Vector2 GetRandomPosition(RectTransform shape)
    {
        RectTransform playerBoard = PlayerBoard;

        if (playerBoard == null)
            return Vector2.zero;

        Rect regionRect = playerBoard.rect;

        float halfWidth = shape.rect.width * 0.5f;
        float halfHeight = shape.rect.height * 0.5f;

        float minX = regionRect.xMin + halfWidth + edgePadding;
        float maxX = regionRect.xMax - halfWidth - edgePadding;
        float minY = regionRect.yMin + halfHeight + edgePadding;
        float maxY = regionRect.yMax - halfHeight - edgePadding;

        // Prevent invalid ranges if a shape is larger than the region.
        if (minX > maxX)
            minX = maxX = 0f;

        if (minY > maxY)
            minY = maxY = 0f;

        return new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );
    }

    private void SetRandomColor(DraggableShape shape)
    {
        if (possibleColors == null || possibleColors.Count == 0)
            return;

        Color randomColor =
            possibleColors[Random.Range(0, possibleColors.Count)];

        shape.SetColor(randomColor);
    }

    public void SubmitArrangement()
    {
        if (currentPhase != GamePhase.Rearranging)
            return;

        currentPhase = GamePhase.Results;

        if (gameRoutine != null)
        {
            StopCoroutine(gameRoutine);
            gameRoutine = null;
        }

        SetAllShapesDraggable(false);

        if (submitButton != null)
            submitButton.gameObject.SetActive(false);

        float score = CalculateScore();

        SetReferenceBoardVisible(true);

        if (timerText != null)
            timerText.text = string.Empty;

        if (instructionText != null)
            instructionText.text = "Original vs Your Picture";

        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true);
            scoreText.text = $"Score: {score:0}%";
        }
    }

    private void ShowReferenceResult()
    {
        ClearReferenceVisuals();
        SetReferenceResultVisible(true);

        foreach (SpawnedShape shape in spawnedShapes)
        {
            CreateReferenceShape(
                shape.rectTransform,
                shape.targetPosition
            );
        }
    }

    private void CreateReferenceShape(
        RectTransform sourceShape,
        Vector2 anchoredPosition)
    {
        RectTransform referenceBoard = ReferenceBoard;

        if (sourceShape == null || referenceBoard == null)
            return;

        GameObject clone = Instantiate(sourceShape.gameObject, referenceBoard);
        referenceVisuals.Add(clone);

        clone.SetActive(true);

        RectTransform cloneRect = clone.GetComponent<RectTransform>();

        cloneRect.anchorMin = new Vector2(0.5f, 0.5f);
        cloneRect.anchorMax = new Vector2(0.5f, 0.5f);
        cloneRect.pivot = new Vector2(0.5f, 0.5f);
        cloneRect.localScale = Vector3.one;
        cloneRect.localRotation = Quaternion.identity;
        cloneRect.anchoredPosition = anchoredPosition;

        DraggableShape draggable = clone.GetComponent<DraggableShape>();

        if (draggable != null)
            draggable.SetDraggable(false);
    }

    private void SetReferenceResultVisible(bool isVisible)
    {
        if (referenceResultRegion != null)
            referenceResultRegion.gameObject.SetActive(isVisible);
    }

    private void ClearReferenceVisuals()
    {
        foreach (GameObject referenceVisual in referenceVisuals)
        {
            if (referenceVisual != null)
                Destroy(referenceVisual);
        }

        referenceVisuals.Clear();
    }

    private float CalculateScore()
    {
        if (spawnedShapes.Count == 0)
            return 0f;

        float totalScore = 0f;

        foreach (SpawnedShape shape in spawnedShapes)
        {
            float distance = Vector2.Distance(
                shape.rectTransform.anchoredPosition,
                shape.targetPosition
            );

            float shapeScore;

            if (distance <= perfectDistance)
            {
                shapeScore = 1f;
            }
            else
            {
                float scoringRange =
                    Mathf.Max(1f, zeroScoreDistance - perfectDistance);

                float adjustedDistance = distance - perfectDistance;

                shapeScore = 1f -
                    Mathf.Clamp01(adjustedDistance / scoringRange);
            }

            totalScore += shapeScore;

            Debug.Log(
                $"{shape.rectTransform.name}: " +
                $"{distance:0.0}px away, " +
                $"{shapeScore * 100f:0}% score"
            );
        }

        return totalScore / spawnedShapes.Count * 100f;
    }

    private void SetAllShapesDraggable(bool canDrag)
    {
        foreach (SpawnedShape shape in spawnedShapes)
        {
            if (shape.draggable != null)
                shape.draggable.SetDraggable(canDrag);
        }
    }

    private void ClearShapes()
    {
        foreach (SpawnedShape shape in spawnedShapes)
        {
            if (shape.rectTransform != null)
                Destroy(shape.rectTransform.gameObject);
        }

        spawnedShapes.Clear();
    }

    private void OnDestroy()
    {
        if (submitButton != null)
            submitButton.onClick.RemoveListener(SubmitArrangement);
    }
}