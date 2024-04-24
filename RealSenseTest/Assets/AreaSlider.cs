using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AreaSlider : MonoBehaviour
{
    public VariableGameZones gameZones;
    public VariableBorderSizes borderSizes;
    public TextMeshProUGUI sliderNumberField;
    public BorderAreas borderArea;
    public ValueAreas valueArea;
    private Slider slider;
    private float lastValue = 0;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SendToVariableGameZoneScript()
    {
        float newValue = GetDifference();
        sliderNumberField.text = slider.value.ToString();
        gameZones.ChangeAreaSize(valueArea, newValue);
        lastValue = slider.value;
    }

    public void SendToVariableBorderSizesScript()
    {
        float newValue = GetDifference();
        sliderNumberField.text = slider.value.ToString();
        borderSizes.ChangeAreaSize(borderArea, newValue);
        lastValue = slider.value;
    }

    public float GetDifference()
    {
        if(lastValue < slider.value)
        {
            Debug.Log("Slider moving right");
            return slider.value - lastValue;
        }
        else
        {
            Debug.Log("Slider moving left");
            return slider.value - lastValue;
        }
    }

}
