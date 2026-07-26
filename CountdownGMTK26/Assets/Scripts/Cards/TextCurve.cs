using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextCurve : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve curve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.5f, 1f),
        new Keyframe(1f, 0f)
    );

    [SerializeField]
    private float curveStrength = 50f;

    private TMP_Text textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        ApplyCurve();
    }

    public void ApplyCurve()
    {
        if (textComponent == null)
            textComponent = GetComponent<TMP_Text>();

        textComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = textComponent.textInfo;

        if (textInfo.characterCount == 0)
            return;

        float minX = textComponent.bounds.min.x;
        float maxX = textComponent.bounds.max.x;
        float textWidth = Mathf.Max(maxX - minX, 0.0001f);

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo character = textInfo.characterInfo[i];

            if (!character.isVisible)
                continue;

            int vertexIndex = character.vertexIndex;
            int materialIndex = character.materialReferenceIndex;

            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

            float characterCenter =
                (character.bottomLeft.x + character.topRight.x) * 0.5f;

            float normalizedPosition =
                Mathf.InverseLerp(minX, maxX, characterCenter);

            float verticalOffset =
                curve.Evaluate(normalizedPosition) * curveStrength;

            Vector3 offset = new Vector3(0f, verticalOffset, 0f);

            vertices[vertexIndex + 0] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            TMP_MeshInfo meshInfo = textInfo.meshInfo[i];

            meshInfo.mesh.vertices = meshInfo.vertices;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}