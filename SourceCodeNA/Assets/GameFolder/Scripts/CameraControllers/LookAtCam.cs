using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    //Camera _cam;
    GameObject _playerCam;
    Canvas _canvas;

    private void Awake()
    {
        //_cam = Camera.main;
        _playerCam = GameObject.FindGameObjectWithTag("PlayerCam");
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        _canvas.worldCamera = _playerCam.transform.GetChild(0).GetComponent<Camera>();
    }

    private void Update()
    {
        //transform.eulerAngles = _cam.transform.eulerAngles;
        transform.eulerAngles = _playerCam.transform.eulerAngles;
    }
}
