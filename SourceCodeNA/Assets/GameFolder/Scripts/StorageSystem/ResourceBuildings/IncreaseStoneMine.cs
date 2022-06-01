using UnityEngine;

public class IncreaseStoneMine : MonoBehaviour
{
    int _increaseAmount = 5;

    private void Update()
    {
        InvokeRepeating("IncreaseResource", Time.deltaTime, 5f);
    }

    void IncreaseResource()
    {
        Storage._stone += _increaseAmount;
    }
}
