using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestory : MonoBehaviour
{
    // Start is called before the first frame update

    public float timeLeft;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }
}
