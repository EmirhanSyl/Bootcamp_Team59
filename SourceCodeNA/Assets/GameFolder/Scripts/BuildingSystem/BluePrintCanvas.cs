using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BluePrintCanvas : MonoBehaviour
{
    [SerializeField]GameObject[] _build;

    [SerializeField] TextMeshProUGUI priceText;

    int _foodCost;
    int _woodCost;
    int _stoneCost;

    //ne kadar bina yerlestirilecegini ayarlayacagiz
    int _isCenterPlaced;
    int _isHousesPlaced;
    int _isBarrackPlaced;

    void LimitterForBuildButtons()
    {
        if (_isCenterPlaced != 0)
        {
            this.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
        }

        if (_isHousesPlaced > 5)
        {
            this.transform.GetChild(1).gameObject.SetActive(false);
        }

        if (_isBarrackPlaced != 0)
        {
            this.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    public void InstantiateTheColonyCenter()
    {
        _foodCost = 0;
        _woodCost = 0;
        _stoneCost = 0;

        if (Storage._food >= _foodCost && Storage._wood >= _woodCost && Storage._stone >= _stoneCost)
        {                        
            Storage._food -= _foodCost;
            Storage._wood -= _woodCost;
            Storage._stone -= _stoneCost;
            Instantiate(_build[0]);
            _isCenterPlaced++;
        }
        LimitterForBuildButtons();
    }
    public void InstantiateHouse()
    {
        _foodCost = 0;
        _woodCost = 0;
        _stoneCost = 0;

        if (Storage._food >= _foodCost && Storage._wood >= _woodCost && Storage._stone >= _stoneCost)
        {
            Storage._food -= _foodCost;
            Storage._wood -= _woodCost;
            Storage._stone -= _stoneCost;
            Instantiate(_build[1]);
            _isHousesPlaced++;
        }
        LimitterForBuildButtons();
    }

    public void InstantiateBarrack()
    {
        _foodCost = 0;
        _woodCost = 0;
        _stoneCost = 0;

        if (Storage._food >= _foodCost && Storage._wood >= _woodCost && Storage._stone >= _stoneCost)
        {
            Storage._food -= _foodCost;
            Storage._wood -= _woodCost;
            Storage._stone -= _stoneCost;
            Instantiate(_build[2]);
            _isBarrackPlaced++;
        }
        LimitterForBuildButtons();
    }

    public void InstantiateMagician()
    {
        _foodCost = 0;
        _woodCost = 0;
        _stoneCost = 0;

        if (Storage._food >= _foodCost && Storage._wood >= _woodCost && Storage._stone >= _stoneCost)
        {
            Storage._food -= _foodCost;
            Storage._wood -= _woodCost;
            Storage._stone -= _stoneCost;
            Instantiate(_build[3]);
        }
        LimitterForBuildButtons();
    }
}
