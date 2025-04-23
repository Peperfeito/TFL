using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afk : MonoBehaviour
{
    private Camera cameraPlayer;
    [SerializeField] private GameObject fakMiniGame;
    public GameObject[] bagulhoQTemQSumir;


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
            fakMiniGame.SetActive(true);
            cameraPlayer.gameObject.SetActive(false);
           
            
        }

    }

    public void Verificar()
    {
        bool dicalma = true;

        for(int i = 0; i < bagulhoQTemQSumir.Length; i++)
        {
            if (bagulhoQTemQSumir[i].activeSelf)
            {
                dicalma = false;
            }
        }

        if(dicalma)
        {
            fakMiniGame.SetActive(false);
            cameraPlayer.gameObject.SetActive(true);
        }
    }
}
