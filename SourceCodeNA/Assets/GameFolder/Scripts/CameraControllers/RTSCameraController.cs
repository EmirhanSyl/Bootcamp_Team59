using UnityEngine;
using Cinemachine;

public class RTSCameraController : MonoBehaviour
{
    [SerializeField] float _panSpeed = 2f;

    //[SerializeField]
    //float _zoomSpeed = 3f;
    //float _minZoom = 40f;
    //float _maxZoom = 90f;



    private CinemachineInputProvider _inputProvider;
    private CinemachineVirtualCamera _virtualCamera;
    private Transform _cameraTransform;

    private void Awake()
    {
        _inputProvider = GetComponent<CinemachineInputProvider>();
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cameraTransform = _virtualCamera.VirtualCameraGameObject.transform;
    }

    void Start()
    {
        
    }

    void Update()
    {
        float x = _inputProvider.GetAxisValue(0);
        float y = _inputProvider.GetAxisValue(1);
        float z = _inputProvider.GetAxisValue(2);

        if (x !=0 || y != 0)
        {
            PanScreen(x, y);
        }
        //if (z != 0)
        //{
        //    ZoomScreen(z);
        //}
    }

    //public void ZoomScreen(float increment)
    //{
    //    float fov = _virtualCamera.m_Lens.FieldOfView;
    //    float targetZoom = Mathf.Clamp(fov + increment, _maxZoom, _minZoom);
    //    _virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(fov, targetZoom, _zoomSpeed * Time.deltaTime);
    //}

    public Vector2 PanDirection(float x, float y)
    {
        Vector2 direction = Vector2.zero;

        if (y >= Screen.height * 0.95f)
        {
            direction.y += 1;
        }
        else if (y <=Screen.height * 0.05f)
        {
            direction.y -= 1;
        }

        if (x >= Screen.width * 0.95f)
        {
            direction.x += 1;
        }
        else if (x<= Screen.width * 0.05f)
        {
            direction.x -= 1;
        }

        return direction;
    }

    public void PanScreen(float x, float y)
    {
        Vector2 direction = PanDirection(x, y);
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _cameraTransform.position + (Vector3)direction * _panSpeed, Time.deltaTime);
    }
}