using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    //public PlayerMovement playerHealth;
    //public GameObject player;
    // Start is called before the first frame update
    void Start()
    {

        healthBar = GetComponent<Slider>();
        healthBar.maxValue = 100f;
        healthBar.value = 100f;

    }


    public void SetHealth(float hp)
    {
        healthBar.value = hp;
    }
    public void ChangeHealth(float hp)
    {
        healthBar.value += hp;
    }
    public float GetValue()
    {
        return healthBar.value;
    }
}
