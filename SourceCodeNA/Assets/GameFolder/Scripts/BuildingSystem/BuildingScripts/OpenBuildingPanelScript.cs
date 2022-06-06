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
            if (CharacterMovement.Skeleton)
            {
                Storage._soul -= 5;
                other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                other.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                PlayerHealth.health = 300;
                other.gameObject.GetComponent<CharacterMovement>().animator = other.gameObject.GetComponent<Animator>();
                other.gameObject.GetComponent<CharacterMovement>().movementSpeed = 5f;
                other.gameObject.GetComponent<CharacterMovement>().sprintSpeed = 8f;
                other.gameObject.GetComponent<PlayerHealth>()._bool = false;
                CharacterMovement.Skeleton = false;
            }
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
