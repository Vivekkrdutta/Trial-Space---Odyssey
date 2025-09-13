using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] bool hasHealthBar = false;
    [Tooltip("Enter the image of the slider that will show the remaining health of the individual.")]
    Image HealthSlider;
    [SerializeField] public int MaxHealth = 10;
    [HideInInspector]
    public int health;
    [Tooltip("The damage it will yield to others.")]
    [SerializeField] private int damage = 10;
    [SerializeField] float barMoveSpeed = 3f;

    bool sliderIsPresent = false;
    void Start()
    {
        health = MaxHealth;
        if (hasHealthBar)
        {
            HealthSlider = FindObjectOfType<PauseMenu>().PlayerHealthimage;
            HealthSlider.fillAmount = 1;
            sliderIsPresent = true;
            currentHealth = (float)health / MaxHealth;
            tarGetHealth = (float)health / MaxHealth;
        }
    }
    public float currentHealth;
    float tarGetHealth;
    public int GetHealth(int val)
    {
        health += val;
        if (health < 0)
            health = 0;
        else if (health > MaxHealth)
            health = MaxHealth;
        return health;
    }
    public int GetHealth()
    {
        return health;
    }
    public int GetDamage()
    {
        return damage;
    }
    private void Update()
    {
        if (sliderIsPresent)
        {
            currentHealth = HealthSlider.fillAmount;
            tarGetHealth = (float)health / MaxHealth;
            HealthSlider.fillAmount = Mathf.Lerp(currentHealth, tarGetHealth, barMoveSpeed * Time.deltaTime);
            HealthSlider.color = Color.Lerp(Color.red, Color.green, (float)health / MaxHealth);
        }
    }
}
