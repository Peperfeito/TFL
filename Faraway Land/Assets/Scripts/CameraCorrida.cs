using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCorrida : MonoBehaviour
{
    



    public Transform player;      
    public float bottomOffset = 4f; 
    public float smoothSpeed = 5f; 

    void LateUpdate()
    {
        if (player == null) return;

        
        Vector3 targetPosition = new Vector3(
            player.position.x,
            player.position.y + bottomOffset,
            transform.position.z
        );

        
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }


}
