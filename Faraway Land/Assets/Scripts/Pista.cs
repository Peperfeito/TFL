using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pista : MonoBehaviour
{
    public Transform player;
    private Vector3 initialScale;
    private float initialDistance;

    public float scaleMultiplier = 2f;
    public float minScale = 0.05f; 
    public float maxScale = 5f;

    private Pista nextRoadPiece;

    private void Start()
    {
        initialScale = transform.localScale;
        initialDistance = Vector3.Distance(player.position, transform.position);

        if (transform.parent != null)
        {
            int index = transform.GetSiblingIndex();
            if (index < transform.parent.childCount - 1)
            {
                nextRoadPiece = transform.parent.GetChild(index + 1).GetComponent<Pista>();
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        float currentDistance = Vector3.Distance(player.position, transform.position);
        float scaleFactor = (initialDistance / currentDistance) * scaleMultiplier;
        scaleFactor = Mathf.Clamp(scaleFactor, minScale, maxScale);
        transform.localScale = initialScale * scaleFactor;

        if (nextRoadPiece != null)
        {
            float newYPosition = transform.position.y - (transform.localScale.y * 10); 
            nextRoadPiece.transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
        }
    }
}
