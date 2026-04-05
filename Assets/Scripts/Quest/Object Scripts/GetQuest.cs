using UnityEngine;
using Cinemachine;
using System.Collections;

public class GetQuest : MonoBehaviour
{
    public GameObject[] holoStructure;

    public GameObject cat;
    public Quest catQuest;

    public CinemachineVirtualCamera questCamera;

    private readonly int _clipHeightPropertyID = Shader.PropertyToID("_ClipHeight");

    public void Rebuild()
    {
        questCamera.gameObject.SetActive(true);
        float duration = 9.5f; // Original 9.5f
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

        targetRenderer.material.SetFloat(_clipHeightPropertyID, startHeight);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float currentHeight = Mathf.Lerp(startHeight, endHeight, elapsedTime / duration);

            targetRenderer.material.SetFloat(_clipHeightPropertyID, currentHeight);
            yield return null;
        }

        questCamera.gameObject.SetActive(false);
        targetRenderer.material.SetFloat(_clipHeightPropertyID, endHeight);
    }
}