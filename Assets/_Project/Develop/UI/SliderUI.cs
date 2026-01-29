using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderUI : MonoBehaviour 
{
    [SerializeField] private Slider _slider;
    
    public void SetValue(float value) => _slider.value = value;
    public void RotateTo(Vector3 direction) =>  transform.rotation = Quaternion.LookRotation(direction);
}
