using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrail : MonoBehaviour
{
    [SerializeField] private TrailRenderer trail;

    [Header("References")]
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightFoot;
    [SerializeField] private Transform leftFoot;

    private void Awake()
    {
        trail.emitting = false;
    }

    public void SetTrailParent(int type)
    {
        switch (type)
        {
            case 0:
                trail.transform.parent = rightHand;
                break;
            case 1:
                trail.transform.parent = leftHand;
                break;
            case 2:
                trail.transform.parent = rightFoot;
                break;
            case 3:
                trail.transform.parent = rightFoot;
                break;
            default:
                trail.transform.parent = leftFoot;
                break;
        }
        trail.emitting = true;       
    }
    public void DisableTrail()
    {
        trail.emitting = false;
    }
}
