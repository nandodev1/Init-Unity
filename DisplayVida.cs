using UnityEngine;
using TMPro;

public class DisplayVida : MonoBehaviour
{
    [SerializeField] private TextMeshPro textoVida;
    [SerializeField] private Vector3 deslocamento = new Vector3(0, 0.5f, 0);
    [SerializeField] private int orderInLayer = 100; // Garante que ficará na frente
    
    private PlayerMovement player;
    private Transform alvoSeguir;

    private void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
        alvoSeguir = transform.parent;
        
        if (textoVida == null)
        {
            textoVida = GetComponent<TextMeshPro>();
        }

        // Configura o sorting order do texto
        if (textoVida != null)
        {
            textoVida.sortingOrder = orderInLayer;
        }
    }

    private void LateUpdate()
    {
        if (player != null && textoVida != null)
        {
            // Atualiza o texto da vida
            textoVida.text = $"{player.GetVidaAtual():F0}/{player.GetVidaMaxima():F0}";
            
            // Posiciona acima da cabeça do jogador
            transform.position = alvoSeguir.position + deslocamento;
        }
    }
}