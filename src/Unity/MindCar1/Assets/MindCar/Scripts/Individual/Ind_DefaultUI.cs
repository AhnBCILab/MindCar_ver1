using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ind_DefaultUI : MonoBehaviour
{
    public bool CheckPause = true;
    public bool CheckTheEnd = false;
    public float timeCount = 0f;
    public Text DefaultScore1P;
    public Text DefaultTimer;
    public Text username;
    public float record_1P;
    float Ivalue = 5f;  // The initialized position value 'z' of car

    // For stimulation with UIVA_Client ----------------------------
    UIVA_Client theClient;  
    bool stimForInit = true;  
    bool stimForTrain = true;
    public double trainningTime = 30.0;  // 30.0
    int buttonIndexNum = 0;   // Individual's button number is 0.

    void Start()
    {
        try
        {
            username.text = SharingData.User[0].name + " : ";
        }catch(System.ArgumentOutOfRangeException)
        {
            username.text = " ";
        }
    }

    void Update()
    {
        if (CheckPause)
            return;
        if (CheckTheEnd)  // If the checking result from ResultUI is the end, it will not do anything.
            return;


        // Record
        var Car_1P = GameObject.Find("Car_1P").GetComponent<Ind_MoveCar1>(); // Reference the script 'Ind_MoveCar1'
        record_1P = Car_1P.Car.transform.position.z - Ivalue; // Since the initialized position value 'z' of car is Ivalue. ex> 5

        DefaultScore1P.text = record_1P.ToString("f2") + "m";

        // Timer
        timeCount += Time.deltaTime;
        DefaultTimer.text = timeCount.ToString("f2");


        // Stimulate for starting check and trainning time check 
        var Client = GameObject.Find("Camera").GetComponent<Ind_SystemControl>();
        theClient = Client.theClient;

        if (stimForInit)
        {
            Debug.Log("Individual, Initialize!");
            theClient.Press(buttonIndexNum);
            stimForInit = false;
        }
        if (timeCount > trainningTime && stimForTrain)  
        {
            Debug.Log("Time is up, Trainning is finished!");
            theClient.Press(buttonIndexNum);
            stimForTrain = false;
        }
    }
}
