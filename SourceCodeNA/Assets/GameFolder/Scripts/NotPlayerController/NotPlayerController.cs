using UnityEngine;

public class NotPlayerController : MonoBehaviour
{
    [SerializeField] GameObject[] _villiagers;
    [SerializeField] GathererVilliager[] _gathererVilliagers;

    private void Update()
    {
        
        //if (Input.GetKey(KeyCode.X))
        //{
        //    _villiagers = GameObject.FindGameObjectsWithTag("Villiager");
        //    _gathererVilliagers = new GathererVilliager[_villiagers.Length];

        //    for (int i = 0; i < _villiagers.Length; i++)
        //    {
        //        _gathererVilliagers[i] = _villiagers[i].GetComponent<GathererVilliager>();

        //        _gathererVilliagers[i].GoToTheResource();
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.M))
        {
            UnitSelections.Instance.SelectAll();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            for (int i = 0; i < UnitSelections.Instance._unitSelectedList.Count; i++)
            {
                UnitSelections.Instance._unitSelectedList[i].gameObject.GetComponent<UnitMovement>().GoToThePlayer(this.transform.position);

            }
        }
    }
}
