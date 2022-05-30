using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBuildingPanelScript : MonoBehaviour
{
    GameObject _buildingCanvas;

    private void Awake()
    {
        _buildingCanvas = GameObject.FindGameObjectWithTag("BuildingCanvas");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _buildingCanvas.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _buildingCanvas.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
