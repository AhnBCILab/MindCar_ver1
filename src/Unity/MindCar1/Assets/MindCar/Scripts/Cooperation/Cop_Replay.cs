using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cop_Replay : MonoBehaviour
{
    UIVA_Client theClient;
    int buttonIndexNum = 3;   // End condition is 3.

    void Start()
    {
        // Stimulate when the game is over. 
        var Client = GameObject.Find("Camera").GetComponent<Cop_SystemControl>();
        theClient = Client.theClient;
        Debug.Log("Cooperation is finished!");
        theClient.Press(buttonIndexNum);
    }

    void OnGUI()
    {
        GUIStyle customButton = new GUIStyle("button");
        customButton.fontSize = 50;

        if (GUI.Button(new Rect(865, 300, 200, 80), "Replay", customButton))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}

