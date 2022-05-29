using UnityEngine;

public class SpawnSoilders : MonoBehaviour
{
    [SerializeField] GameObject[] _soilders;

    public float _cooldownTimer;
    

    int _foodCost;
    int _woodCost;
    int _stoneCost;
    int _soulCost;

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
        _foodCost = 0;
        _woodCost = 0;
        _stoneCost = 0;
        _soulCost = 0;

        if (Storage._food >= _foodCost && Storage._wood >= _woodCost && Storage._stone >= _stoneCost && Storage._soul >= _soulCost)
        {
            Storage._food -= _foodCost;
            Storage._wood -= _woodCost;
            Storage._stone -= _stoneCost;
            Storage._soul -= _soulCost;
            Instantiate(_soilders[0], transform.position, transform.rotation);
            _cooldownTimer = 0f;
            Debug.Log("aa");
        }        
    }

    public void SpawnHead()
    {
        if (_cooldownTimer < 0.9f)
        {
            return;
        }
        _foodCost = 0;
        _woodCost = 0;
        _stoneCost = 0;
        _soulCost = 0;

        if (Storage._food >= _foodCost && Storage._wood >= _woodCost && Storage._stone >= _stoneCost && Storage._soul >= _soulCost)
        {
            Storage._food -= _foodCost;
            Storage._wood -= _woodCost;
            Storage._stone -= _stoneCost;
            Storage._soul -= _soulCost;
            Debug.Log("bb");
            _cooldownTimer = 0f;
            Instantiate(_soilders[1]);
        }
    }

    public void SpawnBalista()
    {
        if (_cooldownTimer < 0.9f)
        {
            return;
        }
        _foodCost = 0;
        _woodCost = 0;
        _stoneCost = 0;
        _soulCost = 0;

        if (Storage._food >= _foodCost && Storage._wood >= _woodCost && Storage._stone >= _stoneCost && Storage._soul >= _soulCost)
        {
            Storage._food -= _foodCost;
            Storage._wood -= _woodCost;
            Storage._stone -= _stoneCost;
            Storage._soul -= _soulCost;
            Debug.Log("cc");
            _cooldownTimer = 0f;
            Instantiate(_soilders[2]);
        }
    }
}
