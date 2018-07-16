using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WholeCamControl : MonoBehaviour {

    public GameObject WholeCam;

    float Ivalue = 5f;  // The initialized position value 'z' of car
    int changeValue = 500;  // The standard value For the next whole camera 
    int record_1P;
    int record_2P;

	void Update () {
        var Car_1P = GameObject.Find("Car_1P").GetComponent<MoveCar1>(); // Reference the script 'MoveCar1'
        var Car_2P = GameObject.Find("Car_2P").GetComponent<MoveCar2>();
        record_1P = (int)(Car_1P.Car.transform.position.z - Ivalue); // Since the initialized position value 'z' of car is Ivalue. ex> 5
        record_2P = (int)(Car_2P.Car.transform.position.z - Ivalue);

        if ((record_1P > changeValue) || (record_2P > changeValue))
        {
            changeValue += 500;
            WholeCam.transform.Translate(Vector3.up * 500);
        }
	}

}
