using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _stoneText;


    public static int _food;
    public static int _stone;
    public static int _wood;
    public static int _soul;    

    private void Update()
    {
        _stoneText.text = _stone.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Villiager"))
        {
            return;
        }        
        else if (other.gameObject.CompareTag("Wood"))
        {
            WoodGatherer(3);
            other.gameObject.tag = "Villiager";
        }        
        else if (other.gameObject.CompareTag("Food"))
        {
            FoodGatherer(3);
            other.gameObject.tag = "Villiager";
        }        
        else if (other.gameObject.CompareTag("Stone"))
        {
            StoneGatherer(3);
            other.gameObject.tag = "Villiager";
        }
        else if (other.gameObject.CompareTag("Soul"))
        {
            SoulGatherer(1);
            other.gameObject.tag = "Villiager";
        }
    }

    void FoodGatherer(int food)
    {
        _food += food;
    }

    void StoneGatherer(int stone)
    {
        _stone += stone;
    }

    void WoodGatherer(int wood)
    {
        _wood += wood;
    }

    void SoulGatherer(int soul)
    {
        _soul += soul;
    }
}
