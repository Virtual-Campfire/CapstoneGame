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
    public Sprite empytHeart;

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
                hearts[i].sprite = empytHeart;
            }

            if (i < numOfHearths)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
