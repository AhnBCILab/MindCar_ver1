﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ind_Replay : MonoBehaviour
{
    public GameObject InformationObject;
    UIVA_Client theClient;
    int buttonIndexNum = 3;   // End condition is 3.

    void Start()
    {
        // Stimulate when the game is over. 
        var Client = GameObject.Find("Camera").GetComponent<Ind_SystemControl>();
        theClient = Client.theClient;
        Debug.Log("Individual is finished!");
        theClient.Press(buttonIndexNum);
    }

    void OnGUI()
    {
        GUIStyle customButton = new GUIStyle("button");
        customButton.fontSize = 50;

        if (GUI.Button(new Rect(665, 300, 200, 80), "Info.", customButton))
        {
            InformationObject.SetActive(true);
        }

        if (GUI.Button(new Rect(965, 300, 200, 80), "Replay", customButton))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
