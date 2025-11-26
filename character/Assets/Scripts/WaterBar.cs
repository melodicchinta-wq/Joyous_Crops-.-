using UnityEngine;
using UnityEngine.UI;

public class WaterBar : MonoBehaviour
{
    public Slider slider;   // <--- INI WAJIB ADA

    public float maxWater = 100f;
    public float currentWater;

    void Start()
    {
        currentWater = 0f;
        slider.maxValue = maxWater;
        slider.value = 0f;
    }

    public void UseWater(float amount)
    {
        currentWater = Mathf.Clamp(currentWater - amount, 0, maxWater);
        slider.value = currentWater;
    }

    public void RefillWater(float amount)
    {
        currentWater = Mathf.Clamp(currentWater + amount, 0, maxWater);
        slider.value = currentWater;
    }
    

}
