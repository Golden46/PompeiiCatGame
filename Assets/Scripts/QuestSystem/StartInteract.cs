using UnityEngine;
using System.Collections;

public class StartInteract : MonoBehaviour
{
    public GameObject[] holoStructure;

    public GameObject cat;
    public Quest catQuest;

    private int ClipHeightPropertyID = Shader.PropertyToID("_ClipHeight");

    public void Rebuild()
    {
        float duration = 6f;
        float startHeight = 0f;
        float endHeight = 2.5f;
        foreach (GameObject structure in holoStructure)
        {
            Renderer targetRenderer = structure.GetComponent<Renderer>();
            StartCoroutine(VerticalRestoration(targetRenderer, duration, startHeight, endHeight));
        }
    }

    private IEnumerator VerticalRestoration(Renderer targetRenderer, float duration, float startHeight, float endHeight)
    {
        targetRenderer.material = new Material(targetRenderer.material);

        float elapsedTime = 0f;

        targetRenderer.material.SetFloat(ClipHeightPropertyID, startHeight);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float currentHeight = Mathf.Lerp(startHeight, endHeight, elapsedTime / duration);

            targetRenderer.material.SetFloat(ClipHeightPropertyID, currentHeight);
            yield return null;
        }

        targetRenderer.material.SetFloat(ClipHeightPropertyID, endHeight);
    }
}
