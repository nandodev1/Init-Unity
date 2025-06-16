using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // This is the correct namespace for scene management
using UnityEngine;

public class MenuManage : MonoBehaviour
{
    [SerializeField] private string nomeDoLevelJogador;
    [SerializeField] private GameObject PainelMenuInicial;
    [SerializeField] private GameObject PainelCreditos;
    public void Jogar()
    {
        // Initialize the menu or perform any setup needed
        SceneManager.LoadScene(nomeDoLevelJogador);
        PainelMenuInicial.SetActive(false);
        PainelCreditos.SetActive(false);
    }
    public void AbrirCréditos()
    {
        // Initialize the menu or perform any setup needed
        PainelMenuInicial.SetActive(false);
        PainelCreditos.SetActive(true);
    }
    public void FecharCredito()
    {
        // Initialize the menu or perform any setup needed
        PainelMenuInicial.SetActive(true);
        PainelCreditos.SetActive(false);
    }
    public void SairDoJogo()
    {
        // Initialize the menu or perform any setup needed
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
    
}
