using UnityEngine;

public class BluePrint : MonoBehaviour
{
    RaycastHit _hit;

    Camera _cam;

    [SerializeField] GameObject _realBuilding;
    [SerializeField] LayerMask _layerMask;


    private void Start()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out _hit, float.MaxValue, _layerMask))
        {
            transform.position = _hit.point;
        }

        if (Input.GetMouseButton(0))
        {
            Instantiate(_realBuilding, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
