using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteOutlineGenerator : MonoBehaviour
{
    [Header("Outline")]
    public Color outlineColor = Color.black;
    public float outlineThickness = 0.05f;
    [Range(4, 64)]
    public int samples = 16;

    [Header("Material")]
    public Material outlineMaterial;

    [Header("Generation")]
    public bool regenerate;

    private readonly List<GameObject> outlineObjects = new();

    private void OnEnable()
    {
        GenerateOutline();
    }

    private void OnValidate()
    {
        if (regenerate)
        {
            regenerate = false;
            GenerateOutline();
        }
    }

    public void GenerateOutline()
    {
        ClearOutline();

        SpriteRenderer original = GetComponent<SpriteRenderer>();

        for (int i = 0; i < samples; i++)
        {
            float angle = Mathf.PI * 2f * i / samples;

            Vector3 offset = new Vector3(
                Mathf.Cos(angle),
                Mathf.Sin(angle),
                0
            ) * outlineThickness;

            GameObject copy = new GameObject($"Outline {i}");
            copy.transform.SetParent(transform, false);
            copy.transform.localPosition = offset;
            copy.transform.localRotation = Quaternion.identity;
            copy.transform.localScale = Vector3.one;

            SpriteRenderer sr = copy.AddComponent<SpriteRenderer>();

            sr.sprite = original.sprite;
            sr.color = outlineColor;
            sr.sortingLayerID = original.sortingLayerID;
            sr.sortingOrder = original.sortingOrder - 1;
            sr.flipX = original.flipX;
            sr.flipY = original.flipY;
            sr.drawMode = original.drawMode;
            sr.size = original.size;

            if (outlineMaterial != null)
                sr.material = outlineMaterial;

            outlineObjects.Add(copy);
        }
    }

    public void ClearOutline()
    {
        foreach (GameObject obj in outlineObjects)
        {
            if (obj == null)
                continue;

#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(obj);
            else
#endif
                Destroy(obj);
        }

        outlineObjects.Clear();
    }

    private void OnDisable()
    {
        ClearOutline();
    }
}