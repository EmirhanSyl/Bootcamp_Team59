using UnityEngine;

public class SpawnSoilders : MonoBehaviour
{
    [SerializeField] GameObject[] _soilders;
    GameObject _npcGroupManager;

    public float _cooldownTimer;
    

    int _foodCost;
    int _woodCost;
    int _stoneCost;
    int _soulCost;

    private void Start()
    {
        _npcGroupManager = GameObject.FindGameObjectWithTag("AttackerGroup");
    }

    private void Update()
    {        
        if (_cooldownTimer < 0.9f)
        {
            _cooldownTimer += Time.deltaTime;
        }
    }

    public void SpawnSoilder()
    {
        if (_cooldownTimer < 0.9f)
        {
            return;
        }
        _foodCost = 15;
        _woodCost = 15;
        _stoneCost = 15;
        _soulCost = 1;

        if (Storage._food >= _foodCost && Storage._wood >= _woodCost && Storage._stone >= _stoneCost && Storage._soul >= _soulCost)
        {
            Storage._food -= _foodCost;
            Storage._wood -= _woodCost;
            Storage._stone -= _stoneCost;
            Storage._soul -= _soulCost;
            GameObject soilder = Instantiate(_soilders[0], transform.position, transform.rotation);
            soilder.transform.parent = _npcGroupManager.transform;
            _cooldownTimer = 0f;            
        }
        
    }

    public void SpawnHugeKnight()
    {
        if (_cooldownTimer < 0.9f)
        {
            return;
        }
        _foodCost = 30;
        _woodCost = 30;
        _stoneCost = 30;
        _soulCost = 3;

        if (Storage._food >= _foodCost && Storage._wood >= _woodCost && Storage._stone >= _stoneCost && Storage._soul >= _soulCost)
        {
            Storage._food -= _foodCost;
            Storage._wood -= _woodCost;
            Storage._stone -= _stoneCost;
            Storage._soul -= _soulCost;
            GameObject soilder = Instantiate(_soilders[1], transform.position, transform.rotation);
            soilder.transform.parent = _npcGroupManager.transform;
            _cooldownTimer = 0f;
        }
    }

    public void SpawnRam()
    {
        if (_cooldownTimer < 0.9f)
        {
            return;
        }
        _foodCost = 45;
        _woodCost = 45;
        _stoneCost = 45;
        _soulCost = 6;

        if (Storage._food >= _foodCost && Storage._wood >= _woodCost && Storage._stone >= _stoneCost && Storage._soul >= _soulCost)
        {
            Storage._food -= _foodCost;
            Storage._wood -= _woodCost;
            Storage._stone -= _stoneCost;
            Storage._soul -= _soulCost;
            GameObject soilder = Instantiate(_soilders[2], transform.position, transform.rotation);
            soilder.GetComponent<SiegeMachines>()._calledByPlayer = true;
            _cooldownTimer = 0f;
        }
    }
}
