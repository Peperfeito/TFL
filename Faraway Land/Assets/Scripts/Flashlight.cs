using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private GameObject flashlightLights;

    private void Update()
    {
        if (this.flashlightLights != null && Input.GetKeyDown(KeyCode.F))
        {
            this.flashlightLights.SetActive(!this.flashlightLights.activeSelf);
        }
    }
}
