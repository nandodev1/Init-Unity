using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private float fadeDuration = 5f;
    private float currentTime = 0f;
    private CanvasRenderer[] renderers;

    private void Start()
    {
        if (gameOverCanvas == null)
        {
            gameOverCanvas = GetComponent<Canvas>();
        }

        // Pega todos os CanvasRenderers filhos
        renderers = gameOverCanvas.GetComponentsInChildren<CanvasRenderer>();
        
        // Inicia com alpha 0
        SetCanvasAlpha(0f);
    }

    private void Update()
    {
        if (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = currentTime / fadeDuration;
            SetCanvasAlpha(alpha);
        }
    }

    private void SetCanvasAlpha(float alpha)
    {
        // Aplica o alpha em todos os elementos do canvas
        foreach (var renderer in renderers)
        {
            renderer.SetAlpha(alpha);
        }
    }
}
