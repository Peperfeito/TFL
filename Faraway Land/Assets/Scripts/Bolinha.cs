using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bolinha : MonoBehaviour
{

    [SerializeField] Image ForceBar;
    [SerializeField] float Force;
    [SerializeField] float MaxForce = 100;
    private bool press;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Force > MaxForce)
        {
            Force = 0f;
        }

        if(press)
        {
            Force += Time.deltaTime * 10f;

            ForceBar.fillAmount = Force / MaxForce;
        }

       
    }

    private void OnMouseDown()
    {
        press = true;
    }

    private void OnMouseUp()
    {
        press = false;
    }

    

}
