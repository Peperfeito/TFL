using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDestroy : MonoBehaviour
{
    [SerializeField] GameObject prefabs;
    [SerializeField] GameObject eventsys;
    public void DestroyUi()
    {
        eventsys.SetActive(true);
        Destroy(prefabs);
        
    }
}
