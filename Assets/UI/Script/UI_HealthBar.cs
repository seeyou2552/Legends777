using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    public void SetHealth(int current, int max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = current;
        healthText.text = $"{current}/{max}";
    }
}
