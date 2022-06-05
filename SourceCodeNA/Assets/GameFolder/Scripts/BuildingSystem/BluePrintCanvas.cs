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
    int _isTombPlaced;
    int _isMillPlaced;
    int _isMinePlaced;
    int _isLumberPlaced;
    int _isStoragePlaced;

    void LimitterForBuildButtons()
    {
        if (_isCenterPlaced != 0)
        {
            this.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
        }

        if (_isHousesPlaced > 4)
        {
            this.transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(false);
        }

        if (_isBarrackPlaced != 0)
        {
            this.transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(false);
        }

        if (_isTombPlaced != 0)
        {
            this.transform.GetChild(4).transform.GetChild(2).gameObject.SetActive(false);
        }

        if (_isMillPlaced != 0)
        {
            this.transform.GetChild(5).transform.GetChild(2).gameObject.SetActive(false);
        }

        if (_isMinePlaced != 0)
        {
            this.transform.GetChild(6).transform.GetChild(2).gameObject.SetActive(false);
        }

        if (_isLumberPlaced != 0)
        {
            this.transform.GetChild(7).transform.GetChild(2).gameObject.SetActive(false);
        }

        if (_isStoragePlaced != 0)
        {
            this.transform.GetChild(8).transform.GetChild(2).gameObject.SetActive(false);
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

    public void InstantiateTomb()
    {
        _foodCost = 0;
        _woodCost = 0;
        _stoneCost = 0;

        if (Storage._food >= _foodCost && Storage._wood >= _woodCost && Storage._stone >= _stoneCost)
        {
            Storage._food -= _foodCost;
            Storage._wood -= _woodCost;
            Storage._stone -= _stoneCost;
            Instantiate(_build[4]);
            _isTombPlaced++;
        }
        LimitterForBuildButtons();
    }

    public void InstantiateMill()
    {
        _foodCost = 0;
        _woodCost = 0;
        _stoneCost = 0;

        if (Storage._food >= _foodCost && Storage._wood >= _woodCost && Storage._stone >= _stoneCost)
        {
            Storage._food -= _foodCost;
            Storage._wood -= _woodCost;
            Storage._stone -= _stoneCost;
            Instantiate(_build[5]);
            _isMillPlaced++;
        }
        LimitterForBuildButtons();
    }

    public void InstantiateStoneMine()
    {
        _foodCost = 0;
        _woodCost = 0;
        _stoneCost = 0;

        if (Storage._food >= _foodCost && Storage._wood >= _woodCost && Storage._stone >= _stoneCost)
        {
            Storage._food -= _foodCost;
            Storage._wood -= _woodCost;
            Storage._stone -= _stoneCost;
            Instantiate(_build[6]);
            _isMinePlaced++;
        }
        LimitterForBuildButtons();
    }

    public void InstantiateLumber()
    {
        _foodCost = 0;
        _woodCost = 0;
        _stoneCost = 0;

        if (Storage._food >= _foodCost && Storage._wood >= _woodCost && Storage._stone >= _stoneCost)
        {
            Storage._food -= _foodCost;
            Storage._wood -= _woodCost;
            Storage._stone -= _stoneCost;
            Instantiate(_build[7]);
            _isLumberPlaced++;
        }
        LimitterForBuildButtons();
    }

    public void InstantiateStorage()
    {
        _foodCost = 0;
        _woodCost = 0;
        _stoneCost = 0;

        if (Storage._food >= _foodCost && Storage._wood >= _woodCost && Storage._stone >= _stoneCost)
        {
            Storage._food -= _foodCost;
            Storage._wood -= _woodCost;
            Storage._stone -= _stoneCost;
            Instantiate(_build[8]);
            _isStoragePlaced++;
        }
        LimitterForBuildButtons();
    }
}
