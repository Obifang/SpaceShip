using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrusterUI : MonoBehaviour
{
    private float[] _positiveAxisValues;
    private float[] _negativeAxisValues;

    public Slider RightSlider;
    public Slider LeftSlider;
    public Slider UpSlider;
    public Slider DownSlider;
    public Slider ForwardSlider;
    public Slider BackwardSlider;

    private bool _valuesUpdated = false;
    // Start is called before the first frame update
    void Start()
    {
        _positiveAxisValues = new float[] {0,0,0};
        _negativeAxisValues = new float[] {0,0,0};
    }

    // Update is called once per frame
    void Update()
    {
        if (!_valuesUpdated) {
            return;
        }

        UpdateSliders();
    }

    public void UpdateSliders()
    {
        if (_positiveAxisValues.Length == 0) {
            return ;
        }
        RightSlider.value = _positiveAxisValues[0];
        LeftSlider.value = -_negativeAxisValues[0];
        UpSlider.value = _positiveAxisValues[1];
        DownSlider.value = -_negativeAxisValues[1];
        ForwardSlider.value = _positiveAxisValues[2];
        BackwardSlider.value = -_negativeAxisValues[2];
    }

    public void UpdateUIValues(Vector3 axisOfValues)
    {
        var x = axisOfValues.x;
        var y = axisOfValues.y;
        var z = axisOfValues.z;
        var axisValues = new float[] { x, y, z };
        Debug.Log(axisValues[0]);

        if (_positiveAxisValues == axisValues) {
            _valuesUpdated = false;
            return;
        }

        _valuesUpdated = true;

        for (int i = 0; i < axisValues.Length;i++) {
            if (axisValues[i] > 0) {
                _positiveAxisValues[i] = axisValues[i];
                _negativeAxisValues[i] = 0;
            } else {
                _positiveAxisValues[i] = 0;
                _negativeAxisValues[i] = axisValues[i];
            }
        }
    }
}
