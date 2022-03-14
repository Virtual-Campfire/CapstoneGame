using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public int numOfHearths;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private void Update()
    {
        if (health > numOfHearths)
        {
            health = numOfHearths;
        }
        
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearths)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }

            // Returns heart size to default (used when changing which heart is pulsing)
            hearts[i].transform.localScale = Vector3.one;
        }

        // While on low health
        if (health <= Mathf.Round(numOfHearths * 0.3f))
        {
            // Catch edge cases
            if (health > 0)
            {
                // Pulse the first full heart in the health UI
                hearts[health - 1].transform.localScale = Vector3.one * (1 + Mathf.Sin(Time.time * 3) * 0.25f);
            }
        }
    }
}
