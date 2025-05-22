using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private Transform alvo; // O player para seguir
    [SerializeField] private float suavidade = 0.125f; // Quanto menor, mais suave
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // Distância da câmera ao player

    private Vector3 velocidade = Vector3.zero;
 
    private void Start()
    {
        // Se o alvo não foi definido, tenta encontrar o player
        if (alvo == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                alvo = player.transform;
        }
    }

    private void LateUpdate()
    {
        if (alvo == null)
            return;

        // Calcula a posição desejada da câmera
        Vector3 posicaoDesejada = alvo.position + offset;

        // Move a câmera suavemente para a posição desejada
        transform.position = Vector3.SmoothDamp(
            transform.position,
            posicaoDesejada,
            ref velocidade,
            suavidade
        );
    }
}