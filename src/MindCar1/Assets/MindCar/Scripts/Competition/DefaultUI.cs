using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultUI : MonoBehaviour {
    public bool CheckPause = true;
    public bool CheckTheEnd = false;
    public Text DefaultScore1P;
    public Text DefaultScore2P;
    public Text DefaultTimer;
    public Text username_1P;
    public Text username_2P;
    public float timeCount = 0f;
    public float record_1P;
    public float record_2P;
    float Ivalue = 5f;  // The initialized position value 'z' of car

    // For stimulation with UIVA_Client ----------------------------
    UIVA_Client theClient;
    bool stimForInit = true;
    bool stimForTrain = true;
    public double trainningTime = 5.0;  // 30.0
    int buttonIndexNum = 1;   // Competition's button number is 1.

    void Start()
    {
        username_1P.text = SharingData.User[0].name + " : ";
        username_2P.text = SharingData.User[1].name + " : ";
    }

	void Update () {
        if (CheckPause)
            return;
        if (CheckTheEnd)  // If the checking result from ResultUI is the end, it will not do anything.
            return;


        // Record
        var Car_1P = GameObject.Find("Car_1P").GetComponent<MoveCar1>(); // Reference the script 'MoveCar1'
        var Car_2P = GameObject.Find("Car_2P").GetComponent<MoveCar2>();
        record_1P = Car_1P.Car.transform.position.z - Ivalue; // Since the initialized position value 'z' of car is Ivalue. ex> 5
        record_2P = Car_2P.Car.transform.position.z - Ivalue;

        DefaultScore1P.text = record_1P.ToString("f2") + "m";
        DefaultScore2P.text = record_2P.ToString("f2") + "m";

        // Timer
        timeCount += Time.deltaTime;
        DefaultTimer.text = timeCount.ToString("f2");

        // Stimulate for starting check and trainning time check 
        var Client = GameObject.Find("Camera").GetComponent<SystemControl>();
        theClient = Client.theClient;

        if (stimForInit)
        {
            Debug.Log("Competition, Initialize!");
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
