using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pista : MonoBehaviour
{
    public Transform player;
    public float minScaleX = 0.5f;
    public float maxScaleX = 2.0f;
    public float maxZ = 50f;
    public float scrollSpeed = 5f;
    private Vector2 offset;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Mathf.Clamp((transform.position.z - player.position.z) / maxZ, 0f, 1f);

        float scaleX = Mathf.Lerp(minScaleX, maxScaleX, distance);
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        offset = new Vector2(0, Time.time * scrollSpeed);
        GetComponent<Renderer>().material.mainTextureOffset = offset;


    }
}
