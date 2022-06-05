using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI.MonsterBehavior;

public class ForResourcesCanvas : MonoBehaviour
{
    [SerializeField] GameObject[] _villiagers;
    [SerializeField] GathererVilliager[] _gathererVilliagers;
    [SerializeField] ChangableResources _resource;
    [SerializeField] GameObject _popupCanvas;
    [SerializeField] TextMeshProUGUI _popupResourcePanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _popupCanvas.SetActive(true);            
        }        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _popupResourcePanel.text = _resource._thisResource.ToString();
        }        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _popupCanvas.SetActive(false);
        }
    }
    private void OnMouseEnter()
    {
        _popupCanvas.SetActive(true);
        _popupResourcePanel.text = _resource._thisResource.ToString();
    }
    private void OnMouseExit()
    {
        _popupCanvas.SetActive(false);
    }

    public void GoToResource()
    {
        _villiagers = GameObject.FindGameObjectsWithTag("Villiager");
        _gathererVilliagers = new GathererVilliager[_villiagers.Length];

        for (int i = 0; i < _villiagers.Length; i++)
        {
            _gathererVilliagers[i] = _villiagers[i].GetComponent<GathererVilliager>();
            _villiagers[i].GetComponent<EnemyBehaviours>().protectedResource = this.gameObject;
            //_gathererVilliagers[i].GoToTheResource();

            //_gathererVilliagers[i].GetComponent<EnemyBehaviours>().protectedResource = this.gameObject;
            Debug.Log("AA");
        }
    }
}
