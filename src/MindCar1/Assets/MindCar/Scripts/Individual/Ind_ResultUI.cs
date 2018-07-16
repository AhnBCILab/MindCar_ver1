using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Ind_ResultUI : MonoBehaviour
{
    public GameObject ResultUIObject;
    public Text ResultScore1P;    
    public float LimitTime = 90.0f;
    public float Time;

    bool EndCondition = false;
    public bool CheckPause = true;

    void Start()
    {
        // Set up the editor befor calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://testdb-be2e2.firebaseio.com/");
    }

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

        var Car_1P = GameObject.Find("Car_1P").GetComponent<Ind_MoveCar1>(); // Reference the script 'Ind_MoveCar1'
        var DefaultUIObject = GameObject.Find("Camera").GetComponent<Ind_DefaultUI>();
        var SliderObject = GameObject.Find("Camera").GetComponent<Ind_Slider>();

        Time = DefaultUIObject.timeCount;
        // End Condition (= game end trigger)
        if (Time > LimitTime)
        {
            ResultUIObject.SetActive(true);
            Car_1P.CheckTheEnd = true;
            DefaultUIObject.CheckTheEnd = true;
            SliderObject.CheckTheEnd = true;

            ResultScore1P.text = DefaultUIObject.DefaultScore1P.text;
            EndCondition = true;

            try
            {
                SharingData.User[0].score = DefaultUIObject.record_1P;
            }catch (System.ArgumentOutOfRangeException e) 
            {
                Debug.Log(e);
            }

            UpdateDB();
        }

    }


    private void UpdateDB()
    {
        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        string username;
        double score;
        try
        {
            username = SharingData.User[0].name;
            score = SharingData.User[0].score;
        }
        catch (System.ArgumentOutOfRangeException)
        {
            username = " ";
            score = 0;
        }
        
        User user = new User("Ind", username, 12, 5, score, Time, 3);
        
        string json = JsonUtility.ToJson(user);

        reference.Child("MindCar").Child(username).SetRawJsonValueAsync(json);  // Individual
    }

    void OnDisable()
    {
        try
        {
            SharingData.User.RemoveAt(0);
        }
        catch (System.ArgumentOutOfRangeException) { }
    }    
}
