using UnityEngine;

public class IncreaseMill : MonoBehaviour
{
    int _increaseAmount = 10;

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
        Storage._food += _increaseAmount;
        _time = 0f;
    }    
}
