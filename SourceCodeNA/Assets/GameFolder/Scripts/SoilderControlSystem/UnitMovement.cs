using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    Camera _cam;
    NavMeshAgent _myAgent;
    public LayerMask _ground;
    public static List<NavMeshAgent> _meshAgents = new List<NavMeshAgent>();

    //buray? ?ald?m - Kafkass

    void Start()
    {
        _cam = Camera.main;
        _myAgent = GetComponent<NavMeshAgent>();
        _meshAgents.Add(_myAgent);
    }

    void Update()
    {
        GoToTheDestination();
    }

    void GoToTheDestination()
    {
        if (_meshAgents.Contains(_myAgent))
        {
            //absolutely nothing   
        }
        else
        {
            _meshAgents.Add(_myAgent);
        }
        if (Input.GetMouseButtonDown(1) && _meshAgents.Contains(_myAgent))
        {
            if (_meshAgents.IndexOf(_myAgent) == 0)
            {
                RaycastHit hit;
                Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _ground))
                {
                    _myAgent.SetDestination(hit.point);
                }

                float angle = 60; // angular step
                int countOnCircle = (int)(360 / angle); // max number in one round
                int count = _meshAgents.Count; // number of agents
                float step = 1; // circle number
                int i = 1; // agent serial number
                float randomizeAngle = Random.Range(0, angle);
                while (count > 1)
                {
                    var vec = Vector3.forward;
                    vec = Quaternion.Euler(0, angle * (countOnCircle - 1) + randomizeAngle, 0) * vec;
                    _meshAgents[i].SetDestination(_myAgent.destination + vec * (_myAgent.radius + _meshAgents[i].radius + 0.5f) * step);
                    countOnCircle--;
                    count--;
                    i++;
                    if (countOnCircle == 0)
                    {
                        if (step != 3 && step != 4 && step < 6 || step == 10) { angle /= 2f; }

                        countOnCircle = (int)(360 / angle);
                        step++;
                        randomizeAngle = Random.Range(0, angle);
                    }
                }

            }
        }
    }

    public void GoToThePlayer(Vector3 player)
    {
        if (_meshAgents.Contains(_myAgent))
        {
            //absolutely nothing   
        }
        else
        {
            _meshAgents.Add(_myAgent);
        }
        if (_meshAgents.Contains(_myAgent)) //&& k?sm?n? sildim-unutma
        {
            if (_meshAgents.IndexOf(_myAgent) == 0)
            {

                _myAgent.SetDestination(player);


                float angle = 60; // angular step
                int countOnCircle = (int)(360 / angle); // max number in one round
                int count = _meshAgents.Count; // number of agents
                float step = 1; // circle number
                int i = 1; // agent serial number
                float randomizeAngle = Random.Range(0, angle);
                while (count > 1)
                {
                    var vec = Vector3.forward;
                    vec = Quaternion.Euler(0, angle * (countOnCircle - 1) + randomizeAngle, 0) * vec;
                    _meshAgents[i].SetDestination(_myAgent.destination + vec * (_myAgent.radius + _meshAgents[i].radius + 0.5f) * step);
                    countOnCircle--;
                    count--;
                    i++;
                    if (countOnCircle == 0)
                    {
                        if (step != 3 && step != 4 && step < 6 || step == 10) { angle /= 2f; }

                        countOnCircle = (int)(360 / angle);
                        step++;
                        randomizeAngle = Random.Range(0, angle);
                    }
                }

            }
        }
    }

    void OnDisable()
    {
        _meshAgents.Clear();
    }
}