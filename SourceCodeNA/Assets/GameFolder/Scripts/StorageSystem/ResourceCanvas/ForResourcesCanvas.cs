using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ForResourcesCanvas : MonoBehaviour
{
    [SerializeField] ChangableResources _resource;
    [SerializeField] GameObject _popupCanvas;
    [SerializeField] TextMeshProUGUI _popupResourcePanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _popupCanvas.SetActive(true);            
        }        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _popupResourcePanel.text = _resource._thisResource.ToString();
        }        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _popupCanvas.SetActive(false);
        }
    }
    private void OnMouseEnter()
    {
        _popupCanvas.SetActive(true);
        _popupResourcePanel.text = _resource._thisResource.ToString();
    }
    private void OnMouseExit()
    {
        _popupCanvas.SetActive(false);
    }
}
