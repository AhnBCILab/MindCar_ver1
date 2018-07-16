using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class ResultUI : MonoBehaviour 
{
    public GameObject ResultUIObject;
    public GameObject Text_1P_Win;
    public GameObject Text_2P_Win;
    public Text ResultScore1P;
    public Text ResultScore2P;
    public float LimitTime = 90.0f;

    bool EndCondition = false;
    public bool CheckPause = true;
    public float Time;

    public void LoadOn()
    {
        SceneManager.LoadScene("Menu");
    }
    
    void Update()
    {
        if (CheckPause)
            return;
        if (EndCondition)  // If the end condition is ture, it will not do anything.
            return;
        
        var Car_1P = GameObject.Find("Car_1P").GetComponent<MoveCar1>(); // Reference the script 'MoveCar1'
        var Car_2P = GameObject.Find("Car_2P").GetComponent<MoveCar2>();
        var DefaultUIObject = GameObject.Find("Camera").GetComponent<DefaultUI>();
        var SliderObject = GameObject.Find("Camera").GetComponent<Com_Slider>();

        Time = DefaultUIObject.timeCount;
        // End Condition (= game end trigger)
        if (Time > LimitTime)
        {
            ResultUIObject.SetActive(true);
            Car_1P.CheckTheEnd = true;
            Car_2P.CheckTheEnd = true;
            DefaultUIObject.CheckTheEnd = true;
            SliderObject.CheckTheEnd = true;

            
            ResultScore1P.text = DefaultUIObject.DefaultScore1P.text;
            ResultScore2P.text = DefaultUIObject.DefaultScore2P.text;
            EndCondition = true;

            // winner
            if (DefaultUIObject.record_1P > DefaultUIObject.record_2P)
            {
                //1P win
                Text_1P_Win.SetActive(true);
            }
            else
            {
                //2P win
                Text_2P_Win.SetActive(true);
            }
            SharingData.User[0].score = DefaultUIObject.record_1P;
            SharingData.User[1].score = DefaultUIObject.record_2P;

            UpdateDB();
        }

    }


    private void UpdateDB()
    {
        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        string username_1P = SharingData.User[0].name;
        string username_2P = SharingData.User[1].name;

        User user_1P = new User("Com", username_1P, 6, 6, SharingData.User[0].score, Time, 6);
        User user_2P = new User("Com", username_2P, 6, 6, SharingData.User[1].score, Time, 6);

        string json_1P = JsonUtility.ToJson(user_1P);
        string json_2P = JsonUtility.ToJson(user_2P);

        reference.Child("MindCar").Child(username_1P).SetRawJsonValueAsync(json_1P); //Competition
        reference.Child("MindCar").Child(username_2P).SetRawJsonValueAsync(json_2P); //Competition

    }

    void OnDisable()
    {
        SharingData.User.RemoveAt(0);
        SharingData.User.RemoveAt(0);
    }



}
