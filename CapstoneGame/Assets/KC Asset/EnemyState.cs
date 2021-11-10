using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public bool ReturnFromChase = false;

    private GameObject LocationSave;

    public GameObject PositionHolder;

    public bool IsPatrol;

    [Header("Basic")]
    public int HP;

    DamageKnockback health;


    private void Awake()
    {

        LocationSave = GameObject.FindWithTag("LocationSave");

        health = GetComponent<DamageKnockback>();

    }

    // Start is called before the first frame update
    void Start()
    {
        PositionHolder = Instantiate(LocationSave, this.transform.position, Quaternion.identity);
        PositionHolder.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y);


    }

    // Update is called once per frame
    void Update()
    {
        // Temporary solution, so that nothing that relying on HP breaks
        HP = (int)health.currentHealth;
    }
}
