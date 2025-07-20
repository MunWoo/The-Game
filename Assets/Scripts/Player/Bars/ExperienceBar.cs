using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI barText;
    PlayerStats player;
    void Awake()
    {
        barText = GetComponentInChildren<TextMeshProUGUI>();
    }
    void Start()
    {
        player = PlayerStats.instance;
        barText.text = $"{player.currentExperience}/{player.maxExperience}";
    }
    public void SetMaxExperience(float experience)
    {
        slider.maxValue = experience;
    }

    public void UpdateExperience(float experience)
    {
        slider.value = experience;
        barText.text = player.currentExperience + "/" + player.maxExperience;
    }

}
