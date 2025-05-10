using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour
{
    [SerializeField] Dialogos dialogo;

    public void AtivarInteracao()
    {
        GameManager.Instance.Inventory.UpdateDescriUI(true, dialogo.neutra);
    }
}
