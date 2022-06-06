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
        if (Input.GetKeyDown(KeyCode.H)) // m tu�una bas�nca
        {
            if (this.transform.GetChild(2).gameObject.activeSelf == false) // e�er panel kapal�ysa
            {
                this.transform.GetChild(2).gameObject.SetActive(true); // paneli a�
            }
            else
            {
                this.transform.GetChild(2).gameObject.SetActive(false); // paneli kapat
            }
        }
    }
}
