using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuckingItemPropBaby : MonoBehaviour
{
    [SerializeField] private FuckingItemDataBaby _itemData;
    public FuckingItemDataBaby ItemData { get { return this._itemData; } }

    [SerializeField] private Animator _animator;

    private void Start()
    {
        this._animator.runtimeAnimatorController = this._animator.runtimeAnimatorController;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.RegisterItemInRange(this);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.UnregisterItemInRange(this);
        }
    }
}
