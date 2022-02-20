using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrunkPiggyText : MonoBehaviour
{
    
    public GameObject TargetPiggy;
    public GameObject text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetPiggy == null) {
          
            text.SetActive(true);


        }
    }

    

}
