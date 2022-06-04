using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Houses : MonoBehaviour
{
    [SerializeField] GameObject _villiager;
    Transform _spawnPoint;
    private void Start()
    {
        _spawnPoint = this.transform.GetChild(0).transform;
    }
    private void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(SpawnVilliager());
            if (i ==4)
            {
                this.GetComponent<Houses>().enabled = false;
            }
        }        
    }
    IEnumerator SpawnVilliager()
    {
        yield return new WaitForSeconds(5f);
        Instantiate(_villiager, _spawnPoint);
    }
}
