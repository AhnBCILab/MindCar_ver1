using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCar2 : MonoBehaviour
{

    public GameObject Car;

    float CarSpeed = 0.0f;

    public bool CheckPause = true;
    public bool CheckTheEnd = false;



    void Update()
    {
        if (CheckPause)
            return;
        if (CheckTheEnd)  // If the checking result from ResultUI is the end, it will not do anything.
            return;

        var epoc = GameObject.Find("Camera").GetComponent<Com_Slider>();
        CarSpeed = (epoc.Speed_2P);


        // To control the car's speed
        //-------------------------------------------------------------------------- 
        Car.transform.Translate(Vector3.forward * CarSpeed * Time.deltaTime);
        //--------------------------------------------------------------------------

    }


}