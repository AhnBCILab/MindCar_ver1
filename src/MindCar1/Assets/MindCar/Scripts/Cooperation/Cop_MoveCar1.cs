using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop_MoveCar1 : MonoBehaviour
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

        var epoc = GameObject.Find("Camera").GetComponent<Cop_Slider>();
        CarSpeed = (epoc.Speed);


        // To control the car's speed
        //-------------------------------------------------------------------------- 
        Car.transform.Translate(Vector3.forward * CarSpeed * Time.deltaTime);
        //--------------------------------------------------------------------------
        
    }


}
