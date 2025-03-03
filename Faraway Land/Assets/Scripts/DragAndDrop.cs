using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public Color objectColor;
    Vector3 mousePositionOffset;

    private Vector3 GetMouseWordPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseEnter()
    {
        
        GetComponent<SpriteRenderer>().color = new Color(objectColor.r, objectColor.g, objectColor.b, 0.5f);
    }

    private void OnMouseExit()
    {
        
        GetComponent<SpriteRenderer>().color = objectColor;
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWordPosition() + mousePositionOffset;
    }

    private void OnMouseDown()
    {
        mousePositionOffset = gameObject.transform.position - GetMouseWordPosition();
    }

    private void Start()
    {
        GetComponent<SpriteRenderer>().color = objectColor;
    }
}
