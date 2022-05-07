using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    void Start()
    {
        UnitSelections.Instance._unitList.Add(this.gameObject);
    }

    void OnDestroy()
    {
        UnitSelections.Instance._unitList.Remove(this.gameObject);
    }
}
