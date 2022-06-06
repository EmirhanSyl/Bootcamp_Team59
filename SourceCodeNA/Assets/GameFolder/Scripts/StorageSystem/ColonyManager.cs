using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColonyManager : MonoBehaviour
{
    int _population;
    GameObject[] _villiagers;
    GameObject _attackerGroup;

    TextMeshProUGUI _soilderText;
    TextMeshProUGUI _villiagerText;

    private void Start()
    {
        _soilderText = GameObject.FindGameObjectWithTag("SoilderText").GetComponent<TextMeshProUGUI>();
        _villiagerText = GameObject.FindGameObjectWithTag("VilliagerText").GetComponent<TextMeshProUGUI>();
        _attackerGroup = GameObject.FindGameObjectWithTag("AttackerGroup");
        InvokeRepeating("DecraseFood", Time.deltaTime, 25.0f);
    }

    private void Update()
    {
        //bunu her bi spawn olan npcde calistiracagiz
         VilliagerPopulation();
         PopulationOfTheVilliage();
        _soilderText.text = _attackerGroup.transform.childCount.ToString();
        _villiagerText.text = _villiagers.Length.ToString();
    }

    void PopulationOfTheVilliage() //notplayerdan instancce alýp köylü sayýsýný alacaktým ama hata aldým oradaki sayýyý tekrar if'e attým -- zamaným olursa villiagerlarý daha optimize bi sekilde ayarlayacagým.
    {
        _population = _attackerGroup.transform.childCount + _villiagers.Length;
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
