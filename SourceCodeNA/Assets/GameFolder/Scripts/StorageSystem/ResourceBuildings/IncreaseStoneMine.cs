using UnityEngine;

public class IncreaseStoneMine : MonoBehaviour
{
    int _increaseAmount = 5;

    float _time;

    private void Update()
    {
        if (_time < 5f)
        {
            _time += Time.deltaTime;
        }
        
        if (_time > 4.5f)
        {
            IncreaseResource();
        }
    }

    void IncreaseResource()
    {
        Storage._stone += _increaseAmount;
        _time = 0f;
    }
}
