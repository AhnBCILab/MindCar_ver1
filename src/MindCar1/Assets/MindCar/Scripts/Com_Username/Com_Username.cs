using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Threading.Tasks;


public class Com_Username : MonoBehaviour 
{
    public string Username_1P;
    public string Username_2P;
    public GameObject ErrorMessage;
    bool IsExisting = false;

    void Start()
    {
        // Set up the editor befor calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://testdb-be2e2.firebaseio.com/");
    }


    public void ReceiveText_1P(Text field)
    {
        IsExisting = false;
        Username_1P = field.text;
        //Debug.Log(Username);
    }

    public void ReceiveText_2P(Text field)
    {
        IsExisting = false;
        Username_2P = field.text;
        //Debug.Log(Username);
    }

        
    
    private void WriteDB()
    {
        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        User user_1P = new User();
        User user_2P = new User();
        string json_1P = JsonUtility.ToJson(user_1P);
        string json_2P = JsonUtility.ToJson(user_2P);

        reference.Child("MindCar").Child(Username_1P).SetRawJsonValueAsync(json_1P); // Competition
        reference.Child("MindCar").Child(Username_2P).SetRawJsonValueAsync(json_2P); // Competition
    }


    private void CheckUsernameInDB()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("MindCar") // Competition
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {   // Handle the error...
                    Debug.Log("Failed");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    // Do something with snapshot...

                    foreach (DataSnapshot ChildSnapshot in task.Result.Children)
                    {
                        if (ChildSnapshot.Key == Username_1P || ChildSnapshot.Key == Username_2P) // Compare DB with the user's name
                        {   // If that user's name is existing,                   
                            // Handle the error...
                            Debug.Log("Failed : There is a same name.");
                            ErrorMessage.SetActive(true);
                            IsExisting = true;
                            break;
                        }                  
                    }

                    if(!IsExisting)
                    {   // If that user's name is not existing,
                        // then just use it.

                        // Write in DB.
                        WriteDB();

                        // Store user's name in static class 'SharingData'
                        SharingData.User.Add(new Username { name = Username_1P });
                        SharingData.User.Add(new Username { name = Username_2P });

                        // Go to the Individual game.
                        LoadOnCompetition(); 
                    }     
                }
            });
    }

    public void LoadOnCheckDB()
    {
        CheckUsernameInDB();
    }

    private void LoadOnCompetition()
    {
        SceneManager.LoadScene("Competition"); 
    }

    public void Back()
    {
        SceneManager.LoadScene("PlayList");
    }

}

