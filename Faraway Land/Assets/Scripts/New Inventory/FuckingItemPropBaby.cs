using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuckingItemPropBaby : MonoBehaviour
{
    [SerializeField] private FuckingItemDataBaby _itemData;
    public FuckingItemDataBaby ItemData { get { return this._itemData; } }

    private Animator _animator;

    private void Start()
    {
        this._animator = this.GetComponentInChildren<Animator>();
        this._animator.runtimeAnimatorController = this._animator.runtimeAnimatorController;

        FarueiUtils.AlignWithGrid(this.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.RegisterItemInRange(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.UnregisterItemInRange(this);
        }
    }
}
