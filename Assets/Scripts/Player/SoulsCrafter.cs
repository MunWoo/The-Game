using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoulsCrafter : MonoBehaviour
{
    public static SoulsCrafter instance;
    [Header("Souls")]
    public int enemySouls;
    public int Increase_FlatDamage; //In %, its the % of souls converted into FlatValue
    public float Increase_PercentDamage; //In %, its the % of souls converted into PercentValue
    public int Increase_FlatDefence; //In %, its the % of souls converted into FlatValue
    public float Increase_PercentDefence; //In %, its the % of souls converted into PercentValue
    public float Increase_AttackRate; //In %, its the % of souls converted into PercentValue

    [Header("Text boxes")]
    public TextMeshProUGUI text_Souls;
    public TextMeshProUGUI text_FlatDamage;
    public TextMeshProUGUI text_PercentDamage;
    public TextMeshProUGUI text_FlatDefence;
    public TextMeshProUGUI text_PercentDefence;
    public TextMeshProUGUI text_AttackRate;
    public TextMeshProUGUI buttontext_FlatDamage;
    public TextMeshProUGUI buttontext_PercentDamage;
    public TextMeshProUGUI buttontext_FlatDefence;
    public TextMeshProUGUI buttontext_PercentDefence;
    public TextMeshProUGUI buttontext_AttackRate;

    void Start()
    {
        instance = this;
        SetStats();
        DisplayTexts();
    }

    void SetStats()
    {
        Increase_FlatDamage = 20; //In %
        Increase_PercentDamage = 40;//In %
        Increase_FlatDefence = 30;//In %
        Increase_PercentDefence = 50;//In %
        Increase_AttackRate = 35;//In %
        enemySouls = 375;
    }

    void DisplayTexts()
    {
        //Calculate the Values to display
        float souls_flatDamage = enemySouls * (Increase_FlatDamage / 100f);
        float souls_percentDamage = enemySouls * (Increase_PercentDamage / 100f);
        float souls_flatDefence = enemySouls * (Increase_FlatDefence / 100f);
        float souls_percentDefence = enemySouls * (Increase_PercentDefence / 100f);
        float souls_attackRate = enemySouls * (Increase_AttackRate / 100f);


        //Texts to display
        buttontext_FlatDamage.text = souls_flatDamage.ToString();
        buttontext_PercentDamage.text = souls_percentDamage.ToString();
        buttontext_FlatDefence.text = souls_flatDefence.ToString();
        buttontext_PercentDefence.text = souls_percentDefence.ToString();
        buttontext_AttackRate.text = souls_attackRate.ToString();

        text_Souls.text = $"Souls: {enemySouls}";
        text_FlatDamage.text = $"Increase Flat Damage by {souls_flatDamage}, {Increase_FlatDamage}% of Souls";
        text_PercentDamage.text = $"Increase Percent Damage by {souls_percentDamage}%, {Increase_PercentDamage}% of Souls";
        text_FlatDefence.text = $"Increase Flat Defence by {souls_flatDefence}, {Increase_FlatDefence}% of Souls";
        text_PercentDefence.text = $"Increase Percent Defence by {souls_percentDefence}%, {Increase_PercentDefence}% of Souls";
        text_AttackRate.text = $"Increase Attack Rate by {souls_attackRate}%, {Increase_AttackRate}% of Souls";
    }
}
