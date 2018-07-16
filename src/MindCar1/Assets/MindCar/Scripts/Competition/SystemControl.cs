using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SystemControl : MonoBehaviour {

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
        theClient = new UIVA_Client(ipUIVAServer);
        timeCount = 5.4f;
    }
    //-------------------------------------------------------------------

	void Update () {

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
                var Car_1P = GameObject.Find("Car_1P").GetComponent<MoveCar1>(); // Reference the script 'MoveCar1'
                var Car_2P = GameObject.Find("Car_2P").GetComponent<MoveCar2>();
                var DefaultUIObject = GameObject.Find("Camera").GetComponent<DefaultUI>();
                var ResultUIObject = GameObject.Find("Camera").GetComponent<ResultUI>();
                var SliderObject = GameObject.Find("Camera").GetComponent<Com_Slider>();
                Car_1P.CheckPause = false;
                Car_2P.CheckPause = false;
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
