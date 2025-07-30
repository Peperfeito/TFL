using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XotaToncroller : MonoBehaviour
{
    private List<Xota> xotas;
    public static XotaToncroller Instancia;
    bool isJuicyCoochie = false;
    [SerializeField] GameObject gay;

    public void TransicaoEstrogenio(Xota xana)
    {
        xotas.Add(xana);
        isJuicyCoochie = true;
    }

    public void TransicaoTestosterona(Xota xereca)
    {
        xotas.Remove(xereca);
    }



    private void Awake()
    {
        Instancia = this;
        xotas = new List<Xota>();
    }

       
    void Update()
    {
        if (!isJuicyCoochie) return;
        if (xotas.Count == 0)
        {
            gay.SetActive(false);
        }
    }
}
