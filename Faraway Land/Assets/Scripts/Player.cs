using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private List<Interactable> objetos = new List<Interactable>();
    [SerializeField] protected GameObject Sidescroll;
    [SerializeField] protected GameObject Grid;
    [SerializeField] protected Transform Saida;
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject buttonsPegar;
    private Interactable interactable;
    private Inventario inventarioAcess;

    protected ItemProp itemProp;
    protected bool playerPodeSeMover = true;

    

    public virtual void InputHandler()
    {
        
        if (Input.GetKeyDown(KeyCode.Q) && !inventarioAcess.dialogoNaTela.activeSelf)
        {

            inventarioAcess.VisualizarInventario();
        }

       

        if (Input.GetKeyDown(KeyCode.E) && !inventarioAcess.inventario.activeSelf)
        {
            

            if(itemProp != null) 
            {




                interactable.eventSys.GetComponent<EventSystem>().SetSelectedGameObject(buttonsPegar);
                buttons.SetActive(true);
                inventarioAcess.UpdateItemBuff(itemProp);

                itemProp = null;
                
            
            }
            else if(objetos.Count > 0 )
            {
                Interacting();
            }

            
        }

        playerPodeSeMover = !inventarioAcess.dialogoNaTela.activeSelf && !inventarioAcess.inventario.activeSelf;

    }

    private void Awake()
    {
        inventarioAcess = GameObject.Find("Inventario").GetComponent<Inventario>();
    }




    protected virtual void OnTriggerEnter2DReaction(Collider2D collision)
    {

        if (collision.CompareTag("Item"))
        {
            itemProp = collision.gameObject.GetComponent<ItemProp>();


            return;
        }
       
        Interactable objetoA = collision.gameObject.GetComponent<Interactable>();

        if(objetoA != null)
        {
            objetos.Add(objetoA);

        }

        

    }

    protected virtual void OnTriggerExit2DReaction(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            itemProp = null;


            return;
        }

        Interactable objetoA = collision.gameObject.GetComponent<Interactable>();

        if(objetoA != null && objetos.Contains(objetoA))
        {
            objetos.Remove(objetoA);

        }
        
        
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
