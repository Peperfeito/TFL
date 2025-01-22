using UnityEngine;
using UnityEngine.EventSystems;

public class DisableMouseInput : MonoBehaviour
{
    private GameObject lastSelected;

    void Update()
    {
        // Salva o último GameObject selecionado pelo teclado.
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
        }

        // Bloqueia cliques do mouse e restaura o último selecionado.
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            // Restaura o foco para o último botão selecionado.
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }
    }
}