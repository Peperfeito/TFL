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
    //[SerializeField] GameObject buttons;
    //[SerializeField] GameObject buttonsPegar;

    protected ItemProp itemProp;
    protected bool playerPodeSeMover = true;

    public virtual void InputHandler()
    {
        //if (Input.GetKeyDown(KeyCode.Q) && !GameManager.Instance.Inventory.DialogoNaTela())
        //{
        //    GameManager.Instance.Inventory.VisualizarInventario();
        //}

        if (Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.TryItemInteraction();
        }
    }
}
