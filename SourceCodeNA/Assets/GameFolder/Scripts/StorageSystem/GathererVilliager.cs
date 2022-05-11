using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GathererVilliager : MonoBehaviour
{

    string _tag;
    NavMeshAgent _navMeshAgent;
    GameObject _storage;
    GameObject _player;

    public static GathererVilliager _gathererInstance;
    public static GathererVilliager Instance { get { return _gathererInstance; } }

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _storage = GameObject.FindGameObjectWithTag("Storage");
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        _tag = this.gameObject.tag;
        GoToTheStorage();
    }
    void GoToTheStorage()
    {
        if (_tag != "Villiager") //tag villiager de?ilse bi kaynak ta??yodur, depoya gitmelidir
        {
            _navMeshAgent.SetDestination(_storage.transform.position);
        }
    }

    public void GoToTheResource()
    {
        _navMeshAgent.SetDestination(_player.transform.position);
    }
}
