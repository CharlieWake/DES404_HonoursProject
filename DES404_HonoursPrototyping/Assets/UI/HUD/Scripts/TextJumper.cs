using TMPro;
using UnityEngine;
using System.Collections;

public class LevelUpTitleAnimator : MonoBehaviour
{
    private TMP_Text textComponent;
    private float jumpHeight = 10f;
    private float delay = 0.05f;
    private float speed = 8f;
        
    [SerializeField] private float loopDelay;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        StartCoroutine(LoopAnimation());
    }

    private IEnumerator LoopAnimation()
    {
        while (true)
        {
            yield return StartCoroutine(AnimateLetters());
            yield return new WaitForSecondsRealtime(loopDelay);
        }
    }

    void OnEnable()
    {
        StartCoroutine(AnimateLetters());
    }

    IEnumerator AnimateLetters()
    {
        textComponent.ForceMeshUpdate();
        TMP_TextInfo textInfo = textComponent.textInfo;

        TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;
            Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

            // Copy original positions
            for (int j = 0; j < 4; j++)
            {
                destinationVertices[vertexIndex + j] = sourceVertices[vertexIndex + j];
            }

            // Jump up
            Vector3 offset = new Vector3(0, jumpHeight, 0);
            for (int j = 0; j < 4; j++)
            {
                destinationVertices[vertexIndex + j] += offset;
            }

            // Apply changes
            textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

            yield return new WaitForSecondsRealtime(delay);

            // Return to original
            for (int j = 0; j < 4; j++)
            {
                destinationVertices[vertexIndex + j] = sourceVertices[vertexIndex + j];
            }

            textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
        }
    }
}