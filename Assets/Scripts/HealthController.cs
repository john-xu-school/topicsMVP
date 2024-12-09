using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public int health;
    public LevelHandler lh;
    public int maxHealth = 50;
    public Slider healthBarUI;
    // Start is called before the first frame update
    void Start()
    {
        healthBarUI.maxValue = health;
        healthBarUI.value = health;
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            endGame();
        }
    }

    public void endGame()
    {
        lh.deathUI();
    }

    public void decHealth()
    {
        health -= 2;
        healthBarUI.value -= 2;
    }

    public void incHealth()
    {
        if (health + 20 < health)
        {
            health += 20;
            healthBarUI.value += 20;
        }
        else
        {
            health = maxHealth;
            healthBarUI.value = maxHealth;
        }
        
    }
}
