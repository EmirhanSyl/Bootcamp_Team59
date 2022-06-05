using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonyManager : MonoBehaviour
{
    int _population;
    GameObject[] _villiagers;


    private void Start()
    {
        InvokeRepeating("DecraseFood", Time.deltaTime, 5.0f);
    }

    private void Update()
    {
        //bunu her bi spawn olan npcde calistiracagiz
         VilliagerPopulation();
         PopulationOfTheVilliage();         
    }

    void PopulationOfTheVilliage() //notplayerdan instancce al�p k�yl� say�s�n� alacakt�m ama hata ald�m oradaki say�y� tekrar if'e att�m -- zaman�m olursa villiagerlar� daha optimize bi sekilde ayarlayacag�m.
    {
        _population = UnitSelections.Instance._unitList.Count + _villiagers.Length;
    }

    void DecraseFood()
    {
        for (int i = 0; i < _population; i++)
        {
            Storage._food -= 1;
            Storage._wood -= 1;
        }
    }

    void VilliagerPopulation()
    {
        _villiagers = GameObject.FindGameObjectsWithTag("Villiager");
    }
}
