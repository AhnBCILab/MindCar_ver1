using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cop_DefaultUI : MonoBehaviour
{
    public bool CheckPause = true;
    public bool CheckTheEnd = false;
    public Text DefaultScore1P;
    public Text DefaultTimer;
    public Text username;
    public float timeCount = 0f;
    float record_1P;
    float Ivalue = 5f;  // The initialized position value 'z' of car

    // For stimulation with UIVA_Client ----------------------------
    UIVA_Client theClient;
    bool stimForInit = true;
    bool stimForTrain = true;
    double trainningTime = 5.0;  // 30.0
    int buttonIndexNum = 2;   // Cooperation's button number is 2.

    void Start()
    {
        username.text = SharingData.User[0].name + " : ";
    }

    void Update()
    {
        if (CheckPause)
            return;
        if (CheckTheEnd)  // If the checking result from ResultUI is the end, it will not do anything.
            return;


        // Record
        var Car_1P = GameObject.Find("Car_1P").GetComponent<Cop_MoveCar1>(); // Reference the script 'Ind_MoveCar1'
        record_1P = Car_1P.Car.transform.position.z - Ivalue; // Since the initialized position value 'z' of car is Ivalue. ex> 5

        DefaultScore1P.text = record_1P.ToString("f2") + "m";

        // Timer
        timeCount += Time.deltaTime;
        DefaultTimer.text = timeCount.ToString("f2");

        // Stimulate for starting check and trainning time check 
        var Client = GameObject.Find("Camera").GetComponent<Cop_SystemControl>();
        theClient = Client.theClient;

        if (stimForInit)
        {
            Debug.Log("Cooperation, Initialize!");
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