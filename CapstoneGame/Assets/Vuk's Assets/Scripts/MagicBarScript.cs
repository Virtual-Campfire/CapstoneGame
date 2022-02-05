using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicBarScript : MonoBehaviour
{

    public Slider slider;
    
    public void SetMaxMagic(int magic)
    {
        slider.maxValue = magic;
        slider.value = magic;
    }

    public void SetMagic(int magic)
    {
        slider.value = magic;
    }
}
