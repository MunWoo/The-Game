using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public Slider slider;
    public GameObject playerGameObject;

    public void SetMaxExperience(int experience)
    {
        slider.maxValue = experience;
    }

    public void UpdateExperience(int experience)
    {
        slider.value = experience;
    }

}
