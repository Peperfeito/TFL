using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuckingItemPropBaby : MonoBehaviour, Interactable
{
    [SerializeField] private FuckingItemDataBaby _itemData;
    public FuckingItemDataBaby ItemData { get { return this._itemData; } }

    private Animator _animator;

    private void Start()
    {
        this._animator = this.GetComponentInChildren<Animator>();
        this._animator.runtimeAnimatorController = this._animator.runtimeAnimatorController;

        FarueiUtils.AlignWithGrid(this.transform, AlignMode.Center);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.RegisterInteractable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.UnregisterInteractable(this);
        }
    }

    public void ActivateInteraction()
    {
        GameManager.Instance.AddToInventory(this.ItemData);
        Destroy(this.gameObject);
    }
}
