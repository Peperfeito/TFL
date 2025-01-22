using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private List<Interactable> objetos = new List<Interactable>();
    [SerializeField] protected GameObject Sidescroll;
    [SerializeField] protected GameObject Grid;
    [SerializeField] protected Transform Saida;

    

    protected virtual void InputHandler()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
           if(objetos.Count > 0 )
           {
            Interacting();
           }
        }
        
    }

    protected virtual void OnTriggerEnter2DReaction(Collider2D collision)
    {
        Debug.Log("entrou");
        Interactable objetoA = collision.gameObject.GetComponent<Interactable>();

        if(objetoA != null)
        {
            objetos.Add(objetoA);

        }

        Debug.Log("entrou mais");

    }

    protected virtual void OnTriggerExit2DReaction(Collider2D collision)
    {
        Debug.Log("saiu");
        Interactable objetoA = collision.gameObject.GetComponent<Interactable>();

        if(objetoA != null && objetos.Contains(objetoA))
        {
            objetos.Remove(objetoA);

        }
        
        Debug.Log("saiu mais");
    }

    private void Interacting()
    {
        
        int indicAtual = 0;
        for(int i = 1; i < objetos.Count; i++)
        {
            float distAtual = (objetos[i].transform.position - this.transform.position).magnitude;
            float distAnter = (objetos[i - 1].transform.position - this.transform.position).magnitude;
            if(distAtual < distAnter)
            {
                indicAtual = i;

            }


        }

        objetos[indicAtual].AtivarInteracao();
    }

    
}
