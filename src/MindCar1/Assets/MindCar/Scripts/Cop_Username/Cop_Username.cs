﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Threading.Tasks;


public class Cop_Username : MonoBehaviour 
{
    public string Username_1P;
    public string Username_2P;
    public string Username;
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
        Username = Username_1P + " & " + Username_2P;

        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        User user = new User();
        string json = JsonUtility.ToJson(user);

        reference.Child("Cooperation").Child(Username).SetRawJsonValueAsync(json); 
    }


    private void CheckUsernameInDB()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("Cooperation") // Individual
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
                        if (ChildSnapshot.Key == Username) // Compare DB with the user's name
                        {   // If that user's name is existing,                   
                            // Handle the error...
                            Debug.Log("Failed : There is a same name.");
                            ErrorMessage.SetActive(true);
                            IsExisting = true;
                            break;
                        }
                    }

                    if (!IsExisting)
                    {   // If that user's name is not existing,
                        // then just use it.
                        //Debug.Log("Success Writing.");

                        // Write in DB.
                        WriteDB();

                        // Store user's name in static class 'SharingData'
                        SharingData.User.Add(new Username { name = Username });

                        // Go to the Individual game.
                        LoadOnIndividual();
                    }
                }
            });
    }

    public void LoadOnCheckDB()
    {
        CheckUsernameInDB();
    }

    private void LoadOnIndividual()
    {
        SceneManager.LoadScene("Cooperation");
    }

    public void Back()
    {
        SceneManager.LoadScene("PlayList");
    }

}


