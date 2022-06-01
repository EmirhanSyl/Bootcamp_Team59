using UnityEngine;

public class IncreaseLumber : MonoBehaviour
{
    int _increaseAmount = 5;

    private void Update()
    {
        InvokeRepeating("IncreaseResource", Time.deltaTime, 5f);
    }

    void IncreaseResource()
    {
        Storage._wood += _increaseAmount;
    }
}
