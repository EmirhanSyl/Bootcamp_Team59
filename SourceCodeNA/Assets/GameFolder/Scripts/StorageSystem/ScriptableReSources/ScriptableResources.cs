using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource Type", menuName = "TypeOfTheResource")]
public class ScriptableResources : ScriptableObject
{
    public string _name = "TypeOfTheResource";
    public int _resource = 100;
}
