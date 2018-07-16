using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Cop_SystemControl : MonoBehaviour
{

    public GameObject StartTimerUIObject;
    public Text Timer;

    bool IsBeginnig = true;
    float timeCount;


    // For communication with UIVA_Client ----------------------------
    public UIVA_Client theClient;
    string ipUIVAServer = "127.0.0.1";


    public double command; /*
    *  Using this variable, we can communicate like this (Epoc->OpenViBE->UIVA->Unity3D).
    *  If you want to use this variable in another scripts,
    *         var var_name = GameObject.Find("Camera").GetComponent<SystemControl>();
              ReceiveCommand = var_name.command;
    *  use this in local scope.
    */

    public double epoc_num_channel;
    public DateTime analogTS;
    public string buttons;


    void Start()
    {
        timeCount = 5.4f;  // from 5.4 sec to 0 sec
        theClient = new UIVA_Client(ipUIVAServer);
    }
    //-------------------------------------------------------------------



    void Update()
    {

        // For the initial 5 seconds.
        if (IsBeginnig)
        {
            if (timeCount > 0.1f)
            {
                timeCount -= Time.deltaTime;
                Timer.text = timeCount.ToString("f0");
                return;
            }
            else
            {
                StartTimerUIObject.SetActive(false);
                IsBeginnig = false;

                // System pause unlock.
                var Car_1P = GameObject.Find("Car_1P").GetComponent<Cop_MoveCar1>(); // Reference the script 'Ind_MoveCar1'
                var DefaultUIObject = GameObject.Find("Camera").GetComponent<Cop_DefaultUI>();
                var ResultUIObject = GameObject.Find("Camera").GetComponent<Cop_ResultUI>();
                var SliderObject = GameObject.Find("Camera").GetComponent<Cop_Slider>();
                Car_1P.CheckPause = false;
                DefaultUIObject.CheckPause = false;
                ResultUIObject.CheckPause = false;
                SliderObject.CheckPause = false;
            }
        }


        // For getting the data of epoc 
        theClient.GetEpocData(out command, out epoc_num_channel, out analogTS, out buttons);  // buttons에 버튼 a, z 그리고 버튼타임스탬프까지 하나의 string으로 다 들어온다.

        Getting();
    }

    void Initialize()
    {
        Debug.Log("Input.GetButtonDown : Jump ");
        theClient.Press(0);
        Debug.Log("Input.GetButtonUp : Jump ");
        theClient.Release(0);
    }

    void Getting()
    {
        // For getting the data of keyboard
        if (Input.GetButtonDown("Jump")) // pressed
        {
            Debug.Log("Input.GetButtonDown : Jump ");
            theClient.Press(0);
        }
        if (Input.GetButtonUp("Jump")) // released
        {
            Debug.Log("Input.GetButtonUp : Jump ");
            theClient.Release(0);
        }
    }
}
