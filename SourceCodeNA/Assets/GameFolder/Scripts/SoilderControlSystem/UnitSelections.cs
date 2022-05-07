using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelections : MonoBehaviour
{
    public List<GameObject> _unitList = new List<GameObject>();
    public List<GameObject> _unitSelectedList = new List<GameObject>();

    // proplarý static yapýyoruz ki unit scriptinden ulaþabilelim
    public static UnitSelections _unitSelectionsInstance;
    public static UnitSelections Instance { get { return _unitSelectionsInstance; } }

    private void Awake()
    {
        //Singelton kullanarak destroy ettirmiyoruz
        if (_unitSelectionsInstance != null && _unitSelectionsInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _unitSelectionsInstance = this;
        }
    }

    public void ClickSelect(GameObject unitToAdd)
    {
        DeselectAll();
        _unitSelectedList.Add(unitToAdd);
        unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
        unitToAdd.GetComponent<UnitMovement>().enabled = true;
    }

    public void ShiftClickSelect(GameObject unitToAdd)
    {
        if (!_unitSelectedList.Contains(unitToAdd))
        {
            _unitSelectedList.Add(unitToAdd);
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
            unitToAdd.GetComponent<UnitMovement>().enabled = true;
        }
        else
        {
            unitToAdd.GetComponent<UnitMovement>().enabled = false;
            unitToAdd.transform.GetChild(0).gameObject.SetActive(false);
            _unitSelectedList.Remove(unitToAdd);
        }
    }

    public void DragClickSelect(GameObject unitToAdd)
    {
        if (!_unitSelectedList.Contains(unitToAdd))
        {
            _unitSelectedList.Add(unitToAdd);
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
            unitToAdd.GetComponent<UnitMovement>().enabled = true;
        }
    }

    public void DeselectAll()
    {
        foreach (var Units in _unitSelectedList)
        {
            Units.GetComponent<UnitMovement>().enabled = false;
            Units.transform.GetChild(0).gameObject.SetActive(false);
        }
        _unitSelectedList.Clear();
    }

    public void deselect(GameObject unitToDeselect)
    {

    }
}
