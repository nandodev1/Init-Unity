using UnityEngine;
using System.Collections;

public class EnemyBear : MonoBehaviour
{
    [Header("Configurações de Detecção")]
    [SerializeField] private float raioDeteccao = 5f;
    [SerializeField] private float velocidadePerseguicao = 3f;
    [SerializeField] private float distanciaMinima = 0.5f;

    [Header("Configurações de Ataque")]
    [SerializeField] private float tempoParada = 1f; // Tempo em segundos para parar

    [Header("Referências")]
    [SerializeField] private Transform player;

    private Animator animator;
    private bool perseguindo;
    private Vector2 direcao;
    private bool estaAtacando;
    private bool estaParando = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (player == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Se estiver atacando, não se move
        if (estaAtacando)
        {
            direcao = Vector2.zero;
            return;
        }

        // Calcula a distância até o player
        float distancia = Vector2.Distance(transform.position, player.position);
        perseguindo = distancia <= raioDeteccao;

        if (perseguindo)
        {
            direcao = (player.position - transform.position).normalized;

            // Só move se não estiver muito perto
            if (distancia > distanciaMinima)
            {
                // Move em direção ao player
                transform.position += (Vector3)direcao * velocidadePerseguicao * Time.deltaTime;
            }

            // Atualiza o animator
            AtualizarAnimacao();
        }
        else
        {
            // Para de mover quando não está perseguindo
            direcao = Vector2.zero;
            animator?.SetBool("Movendo", false);
            animator?.SetBool("Idle", true);
        }
    }

    private void AtualizarAnimacao()
    {
        if (animator == null) return;

        animator.SetBool("Direita", false);
        animator.SetBool("Esquerda", false);
        animator.SetBool("Cima", false);
        animator.SetBool("Baixo", false);

        // Define estado de movimento e idle
        bool estaMovendo = direcao.magnitude > 0.1f;
        animator.SetBool("Movendo", estaMovendo);
        animator.SetBool("Idle", !estaMovendo);

        // Define direção baseada no movimento
        if (Mathf.Abs(direcao.x) > Mathf.Abs(direcao.y))
        {
            if (direcao.x > 0)
                animator.SetBool("Direita", true);
            else
                animator.SetBool("Esquerda", true);
        }
        else
        {
            if (direcao.y > 0)
                animator.SetBool("Cima", true);
            else
                animator.SetBool("Baixo", true);
        }
    }

    private void OnDrawGizmos()
    {
        if (GetComponent<Collider2D>() != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(GetComponent<Collider2D>().bounds.center, GetComponent<Collider2D>().bounds.size);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Urso está colidindo com o jogador!");
            direcao = Vector2.zero;
            estaAtacando = true;
            animator?.SetBool("Atack", true);
            animator?.SetBool("Movendo", false);
            animator?.SetBool("Idle", false);
        }
        else if (collision.gameObject.CompareTag("Traps"))
        {
            Debug.Log("Urso caiu em uma armadilha!");
            Destroy(gameObject); // Remove o urso do jogo
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Garante que o urso continue parado enquanto estiver atacando
            direcao = Vector2.zero;
            estaAtacando = true;
            animator?.SetBool("Atack", true);
            animator?.SetBool("Movendo", false);
            animator?.SetBool("Idle", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            estaAtacando = false;
            animator?.SetBool("Atack", false);
            animator?.SetBool("Idle", true);
        }
    }
}