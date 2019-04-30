using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    
    //barrier physics
    Rigidbody obstacleBody;
    BoxCollider colliderBarrier;

    //for forces
    public float repulsionForce;
    
    public AudioClip[] bumpSounds;

    float frameColCounter;
    public float origPitch;

    void Start()
    {

        //physics refs
        obstacleBody = GetComponent<Rigidbody>();
        colliderBarrier = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Vehicle")
        {
            Rigidbody vehicleRB = collision.gameObject.GetComponent<Rigidbody>();
            vehicleRB.AddForce(-vehicleRB.velocity * repulsionForce);
            //bump sound
            //int randomBump = Random.Range(0, bumpSounds.Length);
            //pp.PlaySound(pp.obstacleSource, bumpSounds[randomBump]);
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Vehicle" )
        {
            Rigidbody vehicleRB = other.gameObject.GetComponent<Rigidbody>();
            vehicleRB.AddForce(-vehicleRB.velocity * repulsionForce);

            frameColCounter++;

            if(frameColCounter > 5)
            {
                other.transform.position += new Vector3(0, 1, 0);

                //i am left, push player to right
                if(transform.position.x < other.transform.position.x)
                {
                    other.transform.position += new Vector3(1, 0, 0);
                }
                //i am right, push player to left
                else
                {
                    other.transform.position += new Vector3(-1, 0, 0);
                }
            }
        }
    }
}
