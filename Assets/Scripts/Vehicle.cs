using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public VehicleType vehicleType;

    public enum VehicleType
    {
        BASIC, FAST, KATAMARI,
    }

    public void UseVehicle()
    {
        if(vehicleType == VehicleType.BASIC)
        {
            //GetComponent<Basic>().UseVehicle();
        }

        if (vehicleType == VehicleType.FAST)
        {
            //GetComponent<Fast>().UseVehicle();
        }

        if (vehicleType == VehicleType.KATAMARI)
        {
            //GetComponent<Katamari>().UseVehicle();
        }
    }
}
