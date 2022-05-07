using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePrint : MonoBehaviour
{
    RaycastHit _hit;
    [SerializeField] GameObject _realBuilding;
    [SerializeField] LayerMask _layerMask;
    
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
