using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public bool rdyShoot;

    public GameObject projectile;
    public GameObject shootPoint;
    public float Velocity;
    // Start is called before the first frame update
    void Start()
    {
        rdyShoot = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Shoot() {
        if (rdyShoot)
        {
            GameObject pro = Instantiate(projectile, shootPoint.transform.position, transform.rotation);
            pro.GetComponent<Rigidbody>().AddRelativeForce(new Vector3( 0,0, Velocity));
        }




    }

}
