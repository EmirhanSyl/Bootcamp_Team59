using UnityEngine;

public class UnitClick : MonoBehaviour
{
    Camera _cam;

    public GameObject _groundPositionObject;

    public LayerMask _clickable, _ground;

    private void Update()
    {
        _cam = Camera.main;        

        if (Input.GetMouseButtonDown(2))
        {
            // mouse pozisyonunu alýyoruz
            RaycastHit hit;
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _clickable))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    UnitSelections.Instance.ShiftClickSelect(hit.collider.gameObject);
                }
                else
                {
                    UnitSelections.Instance.ClickSelect(hit.collider.gameObject);
                }
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    UnitSelections.Instance.DeselectAll();
                }                
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _ground))
            {
                _groundPositionObject.transform.position = hit.point;
                _groundPositionObject.SetActive(false);
                _groundPositionObject.SetActive(true);
            }
        }
    }
}
