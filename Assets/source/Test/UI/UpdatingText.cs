using System;

using UnityEngine;
using UnityEngine.UI;

public class UpdatingText : MonoBehaviour
{

    void Awake()
    {
        Slider slider = GetComponentInParent<Slider>();
        {
            slider.onValueChanged.AddListener( UpdateSliderText );
        }
    }

    public void UpdateSliderText(float value)
    {
        GetComponent<Text>().text = value.ToString();
    }
}