using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Houses : MonoBehaviour
{
    [SerializeField] GameObject _villiager;
    Transform _spawnPoint;

    float _time;
    int _limitter;
    private void Start()
    {
        _spawnPoint = this.transform.GetChild(0).transform;
    }
    private void Update()
    {        
        if (_time < 5f)
        {
            _time += Time.deltaTime;
        }
        //her 4.5-5 saniye arasýnda calisiyor
        if (_time > 4.5f)
        {
            SpawnVilliager();
            _limitter++;
        }
        //5 kere calisip scripti disable ediyor
        if (_limitter == 5)
        {
            this.GetComponent<Houses>().enabled = false;
        }
    }

    void SpawnVilliager()
    {
        Instantiate(_villiager, _spawnPoint);
        _time = 0;
    }
}
