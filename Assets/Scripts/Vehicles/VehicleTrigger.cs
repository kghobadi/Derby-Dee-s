using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleTrigger : MonoBehaviour
{
    //player reference
    GameObject derbyPlayer;

    //ref to collider/trigger, only uses a box collider rn
    BoxCollider myBoxTrigger;

    //ref to actual vehicle object to turn on (could be this same object or a child or something)
    public GameObject actualVehicle, scriptObject, vehicleCam;

    //lets us know whether the vehicle is being used
    public bool vehicleActive;

    //just for input overlaps on enter/exit
    float vehicleTimer, vehicleTimeTotal = 0.1f;

    //actual list of enums (appear in the drop down menu on this script)
    //Each time you make a new vehicle, add it's name here
    public enum VehicleType
    {
        BASIC, FAST, CYLINDRONER,
    }

    //grab refs on awake
    void Awake()
    {
        derbyPlayer = GameObject.FindGameObjectWithTag("Player");
        myBoxTrigger = GetComponent<BoxCollider>();

        //turn off actual vehicle to start
        vehicleCam.SetActive(false);
        DisableVehicleScript();
    }

    void Update()
    {
        if (vehicleActive)
        {
            vehicleTimer -= Time.deltaTime;

            //player presses Space while driving vehicle
            if (Input.GetKeyDown(KeyCode.Space) && vehicleTimer < 0)
            {
                ExitVehicle();
            }
        }
    }

    //called when an object is in this collider's trigger
    void OnTriggerStay(Collider other)
    {
        vehicleTimer -= Time.deltaTime;

        //is player
        if(other.gameObject.tag == "Player" && !vehicleActive)
        {
            //player presses Space
            if (Input.GetKeyDown(KeyCode.Space) && vehicleTimer < 0)
            {
                UseVehicle();
            }
        }
    }

    public void UseVehicle()
    {
        Debug.Log("use vehicle");
        //turn on vehicle object
        EnableVehicleScript();
        vehicleCam.SetActive(true);
        vehicleActive = true;

        //disable trigger
        myBoxTrigger.enabled = false;

        //child Derby to the vehicle
        derbyPlayer.transform.SetParent(actualVehicle.transform);

        //turn off derby
        derbyPlayer.SetActive(false);

        vehicleTimer = vehicleTimeTotal;
    }

    public void ExitVehicle()
    {
        Debug.Log("exit vehicle");
        //child Derby to the void (where he belongs)
        derbyPlayer.transform.SetParent(null);

        //turn derby back on 
        derbyPlayer.transform.Translate(0, 3, 0);
        derbyPlayer.SetActive(true);

        //turn off vehicle object
        DisableVehicleScript();
        vehicleCam.SetActive(false);
        vehicleActive = false;

        //reenable trigger
        myBoxTrigger.enabled = true;

        vehicleTimer = vehicleTimeTotal;
    }

    //called by UseVehicle ()
    //uses my enum tag to locate the correct script to turn on in the actualVehicle obj which is set publicly
    //each time you make a new vehicle, add an if statement to this linking you enum type to the corresponding movement script(s) you create
    void EnableVehicleScript()
    {
        scriptObject.SetActive(true);  
    }

    //called by ExitVehicle ()
    //uses my enum tag to locate the correct script to turn off in the actualVehicle obj which is set publicly
    //each time you make a new vehicle, add an if statement to this linking you enum type to the corresponding movement script(s) you create
    void DisableVehicleScript()
    {
        scriptObject.SetActive(false);
    }
}
