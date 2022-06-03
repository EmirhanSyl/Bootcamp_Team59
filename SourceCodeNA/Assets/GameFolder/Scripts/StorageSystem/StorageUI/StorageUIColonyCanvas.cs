using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StorageUIColonyCanvas : MonoBehaviour
{
    TextMeshProUGUI _stone;
    TextMeshProUGUI _food;
    TextMeshProUGUI _wood;
    TextMeshProUGUI _soul;

    private void Start()
    {
        TextTransforms();
    }

    private void Update()
    {
        WritingUI();
    }

    void TextTransforms()
    {
        _stone = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        _food = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        _wood = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        _soul = transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void WritingUI()
    {
        _stone.text = Storage._stone.ToString();
        _food.text = Storage._food.ToString();
        _wood.text = Storage._wood.ToString();
        _soul.text = Storage._soul.ToString();
    }
}
