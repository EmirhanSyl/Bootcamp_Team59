using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI.MonsterBehavior;

public class Storage : MonoBehaviour
{
    public static int _food = 100;
    public static int _stone = 100;
    public static int _wood = 100;
    public static int _soul = 100;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Villiager"))
        {
            return;
        }        
        else if (other.gameObject.CompareTag("Wood"))
        {
            WoodGatherer((int)other.gameObject.GetComponent<EnemyBehaviours>().carryingCapacity);
            other.gameObject.tag = "Villiager";
        }        
        else if (other.gameObject.CompareTag("Food"))
        {
            FoodGatherer((int)other.gameObject.GetComponent<EnemyBehaviours>().carryingCapacity);
            other.gameObject.tag = "Villiager";
        }        
        else if (other.gameObject.CompareTag("Stone"))
        {
            StoneGatherer((int)other.gameObject.GetComponent<EnemyBehaviours>().carryingCapacity);
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