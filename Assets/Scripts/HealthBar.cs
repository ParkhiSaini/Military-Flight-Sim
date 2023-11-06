using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider slider; // Reference to the UI Slider.

    // Set the maximum value of the slider to match the drone's maximum health.
    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth; // Set the initial value to maxHealth.
    }

    // Update the slider's value based on the drone's current health.
    public void SetHealth(float health)
    {
        slider.value = health;
    }
}
