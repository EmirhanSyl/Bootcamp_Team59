using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForBarrackCavnas : MonoBehaviour
{
    [SerializeField] GameObject _popupCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _popupCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _popupCanvas.SetActive(false);
        }
    }
}
