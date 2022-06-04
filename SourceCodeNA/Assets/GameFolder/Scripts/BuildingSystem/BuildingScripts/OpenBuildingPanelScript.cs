using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBuildingPanelScript : MonoBehaviour
{
    GameObject _buildingCanvas;
    GameObject _rtsCam;
    GameObject _playerCam;

    private void Awake()
    {
        _buildingCanvas = GameObject.FindGameObjectWithTag("BuildingCanvas");
        _rtsCam = GameObject.FindGameObjectWithTag("RTSCam");
        _playerCam = GameObject.FindGameObjectWithTag("PlayerCam");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _buildingCanvas.transform.GetChild(0).gameObject.SetActive(true);
            _rtsCam.transform.GetChild(0).gameObject.SetActive(true);
            _playerCam.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _buildingCanvas.transform.GetChild(0).gameObject.SetActive(false);
            _rtsCam.transform.GetChild(0).gameObject.SetActive(false);
            _playerCam.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
