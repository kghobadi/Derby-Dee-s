using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylindroner : MonoBehaviour
{
    //physics refs
    Rigidbody carBody;
    CapsuleCollider carCollider;

    //velocity vars
    public float forwardVelocity, turningVelocity;
    public float velocityMax, angularVelocityMax;
    Vector3 torque;

    //input holders
    public float horizontalInput, forwardInput;

    //audio
    public AudioSource[] engineSources;
    public int currentSource = 0;
    public AudioClip[] drivingSounds, reverseSounds;
    public float soundTimerTotal, soundTimer;

    //animation
    Animator cylinimator;

    //for bounding
    public float xMin, xMax, zMin, zMax;

    void Start()
    {
        carBody = GetComponent<Rigidbody>();
        carCollider = GetComponent<CapsuleCollider>();

        cylinimator = GetComponent<Animator>();
    }
    
    void Update()
    {
        //take input
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        PlayerBounding();
    }

    void FixedUpdate()
    {
        //for checking angle
        Vector3 forward = transform.forward;
        float angleInDegrees = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
        Debug.Log(angleInDegrees);

        //user is trying to turn
        if(horizontalInput != 0)
        {
            //if player is turning, apply torque
            if (carBody.angularVelocity.magnitude < angularVelocityMax)
            {
                torque = transform.up * horizontalInput * turningVelocity;

                carBody.AddTorque(torque);
            }
            else
            {
                carBody.angularVelocity = Vector3.MoveTowards(carBody.angularVelocity, Vector3.zero, 1 * Time.deltaTime);

            }
        }
        //reset angular vel
        else
        {
            carBody.angularVelocity = Vector3.MoveTowards(carBody.angularVelocity, Vector3.zero, 3 * Time.deltaTime);
        }
       

       
        //user is trying to move
        if(forwardInput != 0)
        {
            //apply forward or backward force depending on input
            if (carBody.velocity.magnitude < velocityMax)
            {
                if (torque.magnitude > 1)
                {
                    carBody.AddRelativeForce(0, 0, forwardInput * forwardVelocity * torque.magnitude);
                }
                else
                {
                    carBody.AddRelativeForce(0, 0, forwardInput * forwardVelocity);
                }
            }
            //reset vel
            else
            {
                carBody.velocity = Vector3.MoveTowards(carBody.velocity, Vector3.zero, 5 * Time.deltaTime);
            }

         
        }
        //reset vel
        else
        {
            carBody.velocity = Vector3.MoveTowards(carBody.velocity, Vector3.zero, 3 * Time.deltaTime);
        }

        //moving forward
        if (carBody.velocity.z > 0)
        {
            SetAnimation("forward");

            soundTimer -= Time.deltaTime;
            if (soundTimer < 0)
            {
                PlayDrivingSound(true);
            }
        }
        //moving backward
        else if (carBody.velocity.z < 0)
        {
            SetAnimation("backward");

            soundTimer -= Time.deltaTime;
            if (soundTimer < 0)
            {
                PlayDrivingSound(false);
            }
        }

        //check if we moving to play sound
        if (carBody.velocity.magnitude == 0)
        {
            StopAudio();
            SetAnimation("idle");
        }
        else
        {
            //moving
        }
    }

    //turns off all anims and sets true animState
    void SetAnimation(string animState)
    {
        cylinimator.SetBool("idle", false);
        cylinimator.SetBool("forward", false);
        cylinimator.SetBool("backward", false);

        cylinimator.SetBool(animState, true);
    }

    void PlayDrivingSound(bool forwardOrBackward)
    {
        //increment audio source counter
        if (currentSource < engineSources.Length - 1)
        {
            currentSource++;
        }
        else
        {
            currentSource = 0;
        }
        //play sound w random pitch
        float randomPitch = Random.Range(0.8f, 1.2f);
        engineSources[currentSource].pitch = randomPitch;

        //play forward or backward sound
        if (forwardOrBackward)
        {
            engineSources[currentSource].PlayOneShot(drivingSounds[0]);
        }
        else
        {
            engineSources[currentSource].PlayOneShot(reverseSounds[0]);
        }
        

        //reset timer
        soundTimer = soundTimerTotal;
    }

    void StopAudio()
    {
        for(int i = 0; i < engineSources.Length; i++)
        {
            engineSources[i].Stop();
        }
    }

    void PlayerBounding()    {
        //above xmax
        if (transform.position.x > xMax)        {            transform.position = new Vector3(xMin + 15, transform.position.y, transform.position.z);        }
        //below xmin
        if (transform.position.x < xMin)        {            transform.position = new Vector3(xMax - 15, transform.position.y, transform.position.z);        }
        //above zmax
        if (transform.position.z > zMax)        {            transform.position = new Vector3(transform.position.x, transform.position.y, zMin + 15);        }
        //below zmin    
        if (transform.position.z < zMin)        {            transform.position = new Vector3(transform.position.x, transform.position.y, zMax - 15);        }    }
}
