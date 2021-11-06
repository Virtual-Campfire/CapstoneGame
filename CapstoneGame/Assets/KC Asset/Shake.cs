using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float power;
    public float duration;
    public Transform holder;
    public float slowDown;
    public bool shaking = false;

    Vector3 startPosition;
    float initialDuration;

    private void Start()
    {
        startPosition = holder.localPosition;
        initialDuration = duration;
    }


    private void Update()
    {
        if (shaking) {
            OnHit();
        }
    }

    public void OnHit() {
        if (duration > 0) {
            holder.localPosition = startPosition+Random.insideUnitSphere*power;
        duration -= Time.deltaTime * slowDown;
        }
        else {
            duration = initialDuration;
            holder.localPosition = startPosition;
            shaking = false;     
        }
    
    }


}
