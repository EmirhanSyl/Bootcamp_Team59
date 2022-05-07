using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePrintCanvas : MonoBehaviour
{
    [SerializeField]GameObject[] _build;

    public void InstantiateTheBuild0()
    {
        Instantiate(_build[0]);
    }
    public void InstantiateTheBuild1()
    {
        Instantiate(_build[1]);
    }
    
}
