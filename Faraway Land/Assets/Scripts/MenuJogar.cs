using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuJogar : MonoBehaviour
{
    [SerializeField] private GameObject TelaCreditos;
    [SerializeField] private GameObject TelaLoad;
    [SerializeField] private GameObject TelaIncial;

    void Start()
    {
        Cursor.visible = false;
    }

    public void LoadScene(string cena)
   {
      SceneManager.LoadScene(cena);


   }

   public void LoadScreen()
   {
    TelaLoad.SetActive(true);
    TelaIncial.SetActive(false);
   }

   public void CreditScreen()
   {
    TelaCreditos.SetActive(true);
    TelaIncial.SetActive(false);
   }

   public void Back()
   {
    TelaIncial.SetActive(true);
    TelaCreditos.SetActive(false);
    TelaLoad.SetActive(false);

   }

   public void Quit()
    {
        Application.Quit();

    }
}
