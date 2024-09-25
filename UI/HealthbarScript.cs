using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScript : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider.maxValue = GameManager.instance._playerHealth.MaxHealth;
        slider.value = GameManager.instance._playerHealth.Health;
    }

    private void Update()
    {
        slider.value = GameManager.instance._playerHealth.Health;
    }
}
