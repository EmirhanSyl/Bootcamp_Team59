using UnityEngine;
using Cinemachine;

public class RTSCameraController : MonoBehaviour
{
    [SerializeField] float _panSpeed = 2f;

    private CinemachineInputProvider _inputProvider;
    private CinemachineVirtualCamera _virtualCamera;
    private Transform _cameraTransform;

    private void Awake()
    {
        _inputProvider = GetComponent<CinemachineInputProvider>();
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        //_cameraTransform = _virtualCamera.VirtualCameraGameObject.transform;
        _cameraTransform = this.transform.parent.transform;
    }

    void Update()
    {
        float x = _inputProvider.GetAxisValue(0);
        float y = _inputProvider.GetAxisValue(1);
        float z = _inputProvider.GetAxisValue(2);

        if (x != 0 || y != 0)
        {
            PanScreen(x, y);
        }
    }

    public Vector3 PanDirection(float x, float y)
    {
        Vector3 direction = Vector3.zero;

        if (y >= Screen.height * 0.95f)
        {
            direction.z += 1;
        }
        else if (y <= Screen.height * 0.05f)
        {
            direction.z -= 1;
        }

        if (x >= Screen.width * 0.95f)
        {
            direction.x += 1;
        }
        else if (x <= Screen.width * 0.05f)
        {
            direction.x -= 1;
        }

        return direction;
    }

    public void PanScreen(float x, float y)
    {
        Vector3 direction = PanDirection(x, y);
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _cameraTransform.position + direction * _panSpeed, Time.deltaTime);
    }
}