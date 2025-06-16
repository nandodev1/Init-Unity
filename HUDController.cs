using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{

    [SerializeField] private Image vidaBar;
    [SerializeField] private Sprite[] bars;
    [SerializeField] private PlayerMovement player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        vidaBar.sprite = bars[player.GetVidas()];

    }
}
