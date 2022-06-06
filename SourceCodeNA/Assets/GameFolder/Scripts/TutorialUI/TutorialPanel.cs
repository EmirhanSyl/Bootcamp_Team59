using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    private void Update()
    {
        OpenThePanel();
    }

    void OpenThePanel()
    {
        if (Input.GetKeyDown(KeyCode.H)) // m tuþuna basýnca
        {
            if (this.transform.GetChild(2).gameObject.activeSelf == false) // eðer panel kapalýysa
            {
                this.transform.GetChild(2).gameObject.SetActive(true); // paneli aç
            }
            else
            {
                this.transform.GetChild(2).gameObject.SetActive(false); // paneli kapat
            }
        }
    }
}
