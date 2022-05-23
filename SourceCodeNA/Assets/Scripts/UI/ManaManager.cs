using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaManager : MonoBehaviour
{
    [SerializeField] private Gradient gradient;

    [SerializeField] private Image filledManaIcon;
    [SerializeField] private Image filledCircle;

    [SerializeField] private float maxManaAmount = 200f;


    public static float manaAmount;
    public static bool readyToMagic;

    private float currentManaAmont;
    private float manaFilledRate;

    void Start()
    {
        manaAmount = 1;
        currentManaAmont = manaAmount;
    }

    
    void Update()
    {
        if (manaAmount >= maxManaAmount)
        {
            manaAmount = maxManaAmount;
            readyToMagic = true;
        }
        if (currentManaAmont != manaAmount)
        {
            DOVirtual.Float(currentManaAmont, manaAmount, 1f, (value) => currentManaAmont = value);
        }
        manaFilledRate = currentManaAmont / maxManaAmount;

        filledManaIcon.fillAmount = manaFilledRate;
        filledCircle.fillAmount = manaFilledRate;
        filledCircle.color = gradient.Evaluate(manaFilledRate);
    }
}
