using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class FirstPersonController : MonoBehaviour
{
    //timers and values for speed
    public float currentSpeed, walkSpeed, sprintSpeed;
    public float scrollSpeed = 2.0f;
    float sprintTimer = 0;
    public float sprintTimerMax = 1;

    float footStepTimer = 0;
    public float footStepTimerTotal = 0.5f;

    CharacterController player;
    GroundCamera mouseLook;
    Vector3 movement;

    //for footstep sounds
    public AudioClip[] currentFootsteps/*, indoorFootsteps, gardenFootsteps, pathFootsteps*/;
    AudioSource playerAudSource;

    //for screamer
    public AudioSource[] voices;
    public int currentVoice;
    public AudioClip[] myGibberishSounds;

    public bool moving;

    Vector3 lastPosition;

    public Animator guyAnimator;

    Vector3 origScale;
    Vector3 origCamPos;

    void Start()
    {
        player = GetComponent<CharacterController>();
        playerAudSource = GetComponent<AudioSource>();
        mouseLook = GetComponentInChildren<GroundCamera>();

        origScale = transform.localScale;
        origCamPos = Camera.main.transform.localPosition;
        Cursor.visible = false;
    }

    void Update()
    {
        //when hold mouse 1, you begin to move in that direction
        if (Input.GetMouseButton(0))
        {
            moving = true;

            movement = new Vector3(0, 0, currentSpeed);
            

            SprintSpeed();
        }
        //move backwards
        else if (Input.GetMouseButton(1))
        {
            moving = true;

            movement = new Vector3(0, 0, -currentSpeed);
           
            SprintSpeed();
        }
        //WASD controls
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            moving = true;
            float moveForwardBackward = Input.GetAxis("Vertical") * currentSpeed;
            float moveLeftRight = Input.GetAxis("Horizontal") * currentSpeed;

            movement = new Vector3(moveLeftRight, 0, moveForwardBackward);

            SprintSpeed();
        }
        //when not moving
        else
        {
            moving = false;
            movement = Vector3.zero;
            currentSpeed = walkSpeed;
        }

        movement = transform.rotation * movement;
        player.Move(movement * Time.deltaTime);

        player.Move(new Vector3(0, -0.5f, 0));

        //for animator
        if (moving)
        {
            guyAnimator.SetBool("moving", true);
        }
        else
        {
            guyAnimator.SetBool("moving", false);
        }
        
        //hold escape to shrink screm and quit
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    

    
    //increases move speed while player is moving over time
    public void SprintSpeed()
    {
        footStepTimer -= Time.deltaTime;
        if (footStepTimer < 0)
        {
            PlayFootStepAudio();
            footStepTimer = footStepTimerTotal;
        }

        sprintTimer += Time.deltaTime;
        //while speed is less than sprint, autoAdd
        if (sprintTimer > sprintTimerMax && currentSpeed < sprintSpeed)
        {
            currentSpeed += Time.deltaTime;
        }
    }

    private void PlayFootStepAudio()
    {
        int n = Random.Range(1, currentFootsteps.Length);
        playerAudSource.clip = currentFootsteps[n];
        playerAudSource.PlayOneShot(playerAudSource.clip, 1f);
        // move picked sound to index 0 so it's not picked next time
        currentFootsteps[n] = currentFootsteps[0];
        currentFootsteps[0] = playerAudSource.clip;
    }
    
   

}
