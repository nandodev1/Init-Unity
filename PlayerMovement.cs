using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float velocidade = 3.5f;
    [SerializeField] private float multiplicadorCorrida = 1.4f;

    [Header("Configurações de Vida")]
    [SerializeField] private float vidaMaxima = 30f;
    [SerializeField] private float vidaAtual;
    [SerializeField] private int vidas = 6;
    [SerializeField] private float danoColisao = 1f; // Aumentado para 5 pontos de dano por segundo

    [Header("Configurações de Respawn")]
    [SerializeField] private float tempoRespawn = 3f;
    [SerializeField] private Vector3 posicaoRespawn = Vector3.zero;

    [Header("Configurações de Area de Respawn")]
    [SerializeField] private float limiteXMinimo = -10f;
    [SerializeField] private float limiteXMaximo = 10f;
    [SerializeField] private float limiteYMinimo = -10f;
    [SerializeField] private float limiteYMaximo = 10f;

    [Header("Configurações de Corrida")]
    [SerializeField] private float duracaoCorrida = 5f;
    [SerializeField] private float cooldownCorrida = 15f;
    private bool corridaDisponivel = true;
    private bool corridaAtiva = false;
    private float tempoRestanteCorrida = 0f;
    private float tempoRestanteCooldown = 0f;

    private Vector2 direcaoMovimento;
    private Animator animator;
    private bool estaCorrendo;
    private bool estaRecebendoDano = false;
    private bool estaMorto = false;

    //refere-se à arma atualmente equipada do player
    [SerializeField] private Transform Weapon;
    [Header("Emogis da Corrida")]
    [SerializeField] private Image statusImage;
    [SerializeField] private Sprite[] statusSprites;

    private void Start()
    { 
        animator = GetComponent<Animator>();
        vidaAtual = vidaMaxima;
        posicaoRespawn = transform.position; // Salva posição inicial como ponto de respawn
        //Debug.Log($"Vida inicial: {vidaAtual}/{vidaMaxima}");
    }

    private void Update()
    {
        // Se estiver morto, não processa inputs
        if (estaMorto) return;

        // Atualiza timers da corrida
        AtualizarTimerCorrida();

        // Verifica dano independente do movimento
        if (estaRecebendoDano)
        {
            ReceberDano(danoColisao);
        }

        ObterInputMovimento();
        AtualizarAnimator();
        Mover();
    }

    private void AtualizarTimerCorrida()
    {
        if (corridaAtiva)
        {
            tempoRestanteCorrida -= Time.deltaTime;
            if (tempoRestanteCorrida <= 0)
            {
                EncerrarCorrida();
            }
        }
        else if (!corridaDisponivel)
        {
            tempoRestanteCooldown -= Time.deltaTime;
            if (tempoRestanteCooldown <= 0)
            {
                corridaDisponivel = true;
                statusImage.sprite = statusSprites[0]; // Reseta o sprite de status
                Debug.Log("Corrida está disponível novamente!");
            }
        }
    }

    private void IniciarCorrida()
    {
        corridaAtiva = true;
        corridaDisponivel = false;
        tempoRestanteCorrida = duracaoCorrida;
        statusImage.sprite = statusSprites[2];
        Debug.Log("Corrida ativada!");
    }

    private void EncerrarCorrida()
    {
        corridaAtiva = false;
        tempoRestanteCooldown = cooldownCorrida;
        statusImage.sprite = statusSprites[1];
        Debug.Log($"Corrida encerrada! Disponível novamente em {cooldownCorrida} segundos.");
    }

    private void ObterInputMovimento()
    {
        // Verifica se está correndo
        bool teclaKPressionada = Input.GetKey(KeyCode.K);
        
        if (teclaKPressionada && corridaDisponivel && !corridaAtiva)
        {
            IniciarCorrida();
        }

        estaCorrendo = (corridaAtiva && teclaKPressionada) || 
                      Input.GetKey(KeyCode.LeftShift) || 
                      Input.GetButton("Fire2");

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        direcaoMovimento = new Vector2(inputX, inputY).normalized;
    }

    private void AtualizarAnimator()
    {
        // Reseta todos os parâmetros
        animator.SetBool("Direita", false);
        animator.SetBool("Esquerda", false);
        animator.SetBool("Cima", false);
        animator.SetBool("Baixo", false);

        // Define estado de movimento e corrida
        animator.SetBool("Movendo", direcaoMovimento.magnitude > 0.1f);
        animator.SetBool("Correndo", estaCorrendo && direcaoMovimento.magnitude > 0.1f);

        // Define a direção baseada no input
        if (Mathf.Abs(direcaoMovimento.x) > 0.1f)
        {
            if (direcaoMovimento.x > 0)
                animator.SetBool("Direita", true);
            else
                animator.SetBool("Esquerda", true);
        }
        
        if (Mathf.Abs(direcaoMovimento.y) > 0.1f)
        {
            if (direcaoMovimento.y > 0)
                animator.SetBool("Cima", true);
            else
                animator.SetBool("Baixo", true);
        }
    }

    private void Mover()
    {
        float velocidadeAtual = velocidade * (estaCorrendo ? multiplicadorCorrida : 1f);
        Vector2 novaPosicao = (Vector2)transform.position + direcaoMovimento * velocidadeAtual * Time.deltaTime;
        transform.position = novaPosicao;
    }

    public void ReceberDano(float dano)
    {
        vidaAtual = Mathf.Max(0, vidaAtual - dano);
       //Debug .Log($"Vida atual: {vidaAtual}/{vidaMaxima} ({GetPorcentagemVida() * 100:F1}%)");
        
        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    public void RecuperarVida(float cura)
    {
        vidaAtual = Mathf.Min(vidaMaxima, vidaAtual + cura);
        //Debug.Log($"Vida atual: {vidaAtual}/{vidaMaxima} ({GetPorcentagemVida() * 100:F1}%)");
    }

    private void Morrer()
    {
        estaMorto = true;
        
        // Muda de SetBool para SetTrigger
        animator?.SetTrigger("Morto");
        
        // Zera o movimento
        direcaoMovimento = Vector2.zero;
        estaRecebendoDano = false;
        
        // Atualiza animator apenas para estados de movimento
        animator?.SetBool("Movendo", false);
        animator?.SetBool("Correndo", false);
        animator?.SetBool("Idle", false);
        
        //Debug.Log("Player morreu! Respawnando em 3 segundos...");
        
        //Verifica se o jogador já perdeu todas as vidas
        if (--vidas <= 0)
        {
            //Debug.Log("O jogador não pode mais respawnar, pois perdeu todas as vidas.");
            SceneManager.LoadScene("GameOver"); // Substitua pelo nome da sua cena de Game Over
            return;
        }
        // Inicia a contagem para respawn
        StartCoroutine(RespawnarAposDelay());
    }

    private IEnumerator RespawnarAposDelay()
    {
        yield return new WaitForSeconds(tempoRespawn);
        Respawnar();
    }

    private void Respawnar()
    {
        // Gera posição aleatória dentro dos limites
        float randomX = Random.Range(limiteXMinimo, limiteXMaximo);
        float randomY = Random.Range(limiteYMinimo, limiteYMaximo);
        Vector3 posicaoAleatoria = new Vector3(randomX, randomY, 0);

        // Reseta estados
        estaMorto = false;
        vidaAtual = vidaMaxima;
        transform.position = posicaoAleatoria;
        
        // Atualiza animator
        animator?.ResetTrigger("Morto"); // Reset do trigger de morte
        animator?.SetTrigger("Respawn");
        animator?.SetBool("Idle", true);
        
        //Debug.Log($"Player respawnou em ({randomX:F1}, {randomY:F1})! Vida resetada: {vidaAtual}/{vidaMaxima}");

        StartCoroutine(ResetarTriggers());
    }

    private IEnumerator ResetarTriggers()
    {
        yield return new WaitForEndOfFrame();
        animator?.ResetTrigger("Respawn");
        animator?.ResetTrigger("Morto");
    }

    private IEnumerator DesativarRespawn()
    {
        yield return new WaitForEndOfFrame();
        animator?.SetBool("Respawn", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.transform.parent?.CompareTag("Enemy") == true)
        {
            estaRecebendoDano = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.transform.parent?.CompareTag("Enemy") == true)
        {
            estaRecebendoDano = false;
        }
    }

    // Getters para vida
    public float GetVidaAtual() => vidaAtual;
    public int GetVidas() => vidas;
    public float GetVidaMaxima() => vidaMaxima;
    public float GetPorcentagemVida() => vidaAtual / vidaMaxima;
}