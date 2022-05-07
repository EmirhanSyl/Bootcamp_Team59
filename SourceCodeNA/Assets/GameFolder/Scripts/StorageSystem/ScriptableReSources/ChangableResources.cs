using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangableResources : MonoBehaviour
{
    [SerializeField] ScriptableResources _resourceType;

    public int _thisResource;
    public string _tag;
    void Start()
    {
        _thisResource = _resourceType._resource;
        this.gameObject.tag = _tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Villiager"))
        {
            other.gameObject.tag = _tag;
            _thisResource -= 3;
        }
    }

    private void Update()
    {
        if(_thisResource <= 0)
        {
            Destroy(this);
        }
    }
}
