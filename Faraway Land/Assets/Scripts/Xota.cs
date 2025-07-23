using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Xota : MonoBehaviour
{
    [SerializeField] float litTorchTime = 20f;
    bool canLightTorch = false;
    bool canDarkTorch = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canLightTorch = true;
    }
    
    
    
    private void LightTorch()
    {
        if (canLightTorch && Input.GetKeyDown(KeyCode.E))
        {
            this.GetComponent<Animator>().Play("tochaoanimao");
            canDarkTorch = true;
            XotaToncroller.Instancia.TransicaoTestosterona(this);
        }

        else return;
    }

    private void DarkTorch()
    {
        this.GetComponent<Animator>().Play("desanimacao");
        XotaToncroller.Instancia.TransicaoEstrogenio(this);
        litTorchTime = 10f;
        canDarkTorch = false;
    }

    void Start()
    {
        XotaToncroller.Instancia.TransicaoEstrogenio(this);
    }

    // Update is called once per frame
    void Update()
    {
        LightTorch();
        if (canDarkTorch)
        {
            litTorchTime -= Time.deltaTime;
        }
        if (litTorchTime <= 0)
        {
            DarkTorch();
        }
    }
}
