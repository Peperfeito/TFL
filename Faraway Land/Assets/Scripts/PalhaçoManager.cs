using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalhaçoManager : MonoBehaviour
{
    private Camera cameraPlayer;
    [SerializeField] private GameObject paiacuMiniGame;
    [SerializeField] private GameObject barra;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        cameraPlayer = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {



        if (collision.CompareTag("Player"))
        {
            paiacuMiniGame.SetActive(true);
            barra.SetActive(true);
            cameraPlayer.gameObject.SetActive(false);


        }

    }
}
