using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    //Camera _cam;
    GameObject _playerCam;

    private void Awake()
    {
        //_cam = Camera.main;
        _playerCam = GameObject.FindGameObjectWithTag("PlayerCam");
    }

    private void Update()
    {
        //transform.eulerAngles = _cam.transform.eulerAngles;
        transform.eulerAngles = _playerCam.transform.eulerAngles;
    }
}
