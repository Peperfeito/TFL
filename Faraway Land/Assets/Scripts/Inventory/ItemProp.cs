using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProp : MonoBehaviour
{
    [SerializeField] Item item;

    public Item GetItem()
    {
        Debug.Assert(item != null);

        return item;
    }
}
