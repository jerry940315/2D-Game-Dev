using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public static int currentHealth;
    public static int maxHealth;

    private Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }
}
