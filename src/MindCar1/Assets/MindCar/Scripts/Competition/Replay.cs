using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Replay : MonoBehaviour {

    public GameObject InformationObject_1P;
    public GameObject InformationObject_2P;
    UIVA_Client theClient;
    int buttonIndexNum = 3;   // End condition is 3.

    void Start()
    {
        // Stimulate when the game is over. 
        var Client = GameObject.Find("Camera").GetComponent<SystemControl>();
        theClient = Client.theClient;
        Debug.Log("Competition is finished!");
        theClient.Press(buttonIndexNum);
    }

	void OnGUI()
    {
        GUIStyle customButton = new GUIStyle("button");
        customButton.fontSize = 50;

        if (GUI.Button(new Rect(665, 200, 200, 80), "Info.", customButton))
        {
            InformationObject_1P.SetActive(true);
            InformationObject_2P.SetActive(true);
        }

        if (GUI.Button(new Rect(1065, 200, 200, 80), "Replay", customButton))
        {
            SceneManager.LoadScene("Menu");
        }
        
    }

}

