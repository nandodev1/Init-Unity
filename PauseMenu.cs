using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // This is the correct namespace for scene management
using UnityEngine;

public class PauseMenuClass : MonoBehaviour
{
    [SerializeField] private string nomeDoLevelJogador;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private string PainelMenuInicial;
    private bool isPaused = false;
	
	private int i = 0;
	
    private void Update()
    {
		//Debug.Log(i++);
    }

    public void Home()
    {
        SceneManager.LoadScene(nomeDoLevelJogador);
        Time.timeScale = 1f; // Retorna o tempo do jogo ao normal
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f; // Retorna o tempo do jogo ao normal
    }

    public void Restart()
    {
        SceneManager.LoadScene(nomeDoLevelJogador);
    }
}
