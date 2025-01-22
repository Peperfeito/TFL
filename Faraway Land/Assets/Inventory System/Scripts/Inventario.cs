using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public static Inventario Singleton;
    List<Item> itens = new List<Item>();

    public void AddItem(Item item)
    {
        if(itens.Contains(item)) return;

        itens.Add(item);


        
    }

    
    

    


    public bool RemoveItem(Item item)
    {
        if(!itens.Contains(item)) return false;

        itens.Remove(item);
        return true;
    }

    



    
    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(this.gameObject);
        
    }
}
