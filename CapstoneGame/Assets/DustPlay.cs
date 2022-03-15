using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustPlay : MonoBehaviour
{
    public ParticleSystem Dust;
    public bool Isturning;
    bool IsPlaying;

    // Start is called before the first frame update
    void Start()
    {
        IsPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Extra check for component before accessing it to avoid null references
        if (GetComponentInParent<StatueCheckTurn>())
        {
            Isturning = GetComponentInParent<StatueCheckTurn>().IsTurning;
        }

        if (Isturning == true && IsPlaying == false)
        {
            CreateDust();
        }
        else { };

    }
    void CreateDust() {
        Dust.Play();
        IsPlaying = true;
    }

}
