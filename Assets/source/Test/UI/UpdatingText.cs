using System;

using UnityEngine;
using UnityEngine.UI;

public class UpdatingText : MonoBehaviour
{
    public Slider slider;

    void Awake()
    {
        if ( slider != null )
        {
            slider.onValueChanged.AddListener( UpdateSliderText );
        }
    }

    public void UpdateSliderText(float value)
    {
        GetComponent<Text>().text = value.ToString();
    }
}