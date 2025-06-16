using UnityEngine;
using TMPro; // Add this for TextMeshPro support

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject PainelHistory;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject Enemys;
    [SerializeField] private TextMeshProUGUI textoCountInimigos;
    private int quantidadeInimigos = 0;

    void Start()
    {
        if (textoCountInimigos == null)
        {
            Debug.LogError("TextMeshProUGUI não está atribuído no Inspector!");
            return;
        }
        
        Time.timeScale = 0f;
        AtualizarQuantidadeInimigos();
    }

    private void Update()
    {
        // Atualiza a contagem a cada frame
        AtualizarQuantidadeInimigos();

        // Verifica se a tecla ESC foi pressionada
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            PauseMenu.SetActive(true);
            Time.timeScale = 0f; // Pausa o tempo do jogo
        }
    }

    private void AtualizarQuantidadeInimigos()
    {
        if (Enemys != null && textoCountInimigos != null)
        {
            quantidadeInimigos = Enemys.transform.childCount;
            textoCountInimigos.text = $" {quantidadeInimigos}";
            //Debug.Log($"Quantidade de inimigos atualizada: {quantidadeInimigos}");
        }
        else
        {
           // Debug.LogWarning("Enemys ou textoCountInimigos não está atribuído!");
        }
    }

    public void FecharHistpry()
    {
        // Initialize the menu or perform any setup needed
        PainelHistory.SetActive(false);
        Time.timeScale = 1f;// Inicia o jogo.
    }

    private void SairDoJogo()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        Debug.Log("Saindo do jogo...");
        
    }
}
