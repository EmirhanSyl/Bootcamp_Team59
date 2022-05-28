using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceInformations : MonoBehaviour
{
    public int enemyCountOnResource;
    public bool isExploiting;
    public LayerMask whichRegionOnResource;

    public void SetInformations(LayerMask regionLayer, int enemyCount)
    {
        isExploiting = true;
        whichRegionOnResource = regionLayer;
        enemyCountOnResource = enemyCount;
    }
    public void ClearInformations()
    {
        isExploiting = false;
        whichRegionOnResource = 0;
        enemyCountOnResource = 0;
    }
}
