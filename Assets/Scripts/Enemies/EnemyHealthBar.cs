using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private Camera _camera;

    void Awake()
    {
        _camera = Camera.main;
    }
    public void SetHealthBar(int health, int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    public void UpdateHealthBar(int currentHealth)
    {
        slider.value = currentHealth;
    }

    void Update()
    {
        transform.rotation = _camera.transform.rotation;
        transform.position = target.position + offset;
    }
}

/*
    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private Camera _camera;

    void Awake()
    {
        _camera = Camera.main;
    }
    public void SetHealthBar(int health, int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    public void UpdateHealthBar(int currentHealth)
    {
        slider.value = currentHealth;
    }

    void Update()
    {
        transform.rotation = _camera.transform.rotation;
        transform.position = target.position + offset;
    }
}

*/

