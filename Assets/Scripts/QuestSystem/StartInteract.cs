using UnityEngine;
using Cinemachine;
using System.Collections;

public class StartInteract : MonoBehaviour
{
    public GameObject[] holoStructure;

    public GameObject cat;
    public Quest catQuest;

    [SerializeField] private CinemachineBrain cameraBrain;
    public CinemachineVirtualCamera questCamera;

    private int ClipHeightPropertyID = Shader.PropertyToID("_ClipHeight");

    private void Start()
    {
        cameraBrain = FindAnyObjectByType<CinemachineBrain>();
        if (cameraBrain != null)
        {
            cameraBrain.m_CameraActivatedEvent.AddListener(OnCameraSwitch);
        }
    }

    public void Rebuild()
    {
        questCamera.gameObject.SetActive(true);
        float duration = 9.5f;
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

        questCamera.gameObject.SetActive(false);
        targetRenderer.material.SetFloat(ClipHeightPropertyID, endHeight);
    }

    private void OnCameraSwitch(ICinemachineCamera fromCamera, ICinemachineCamera toCamera)
    {
        Debug.Log("yes");
    }
}
