using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Botoes : MonoBehaviour
{

    [SerializeField] private GameObject DescricaoCilindro;
    [SerializeField] private GameObject Cilindro;

    public PlayerMove playermove;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BotacaoSim()
    {
        Destroy(DescricaoCilindro);
        Destroy(Cilindro);
        playermove.moveSpeed = 5f;
        playermove.jumpForce = 10f;

    }

    public void BotacaoNao()
    {
        DescricaoCilindro.SetActive(false);
        playermove.moveSpeed = 5f;
        playermove.jumpForce = 10f;
        

    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
