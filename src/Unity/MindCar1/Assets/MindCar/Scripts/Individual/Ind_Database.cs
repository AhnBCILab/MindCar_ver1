using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Ind_Database : MonoBehaviour {

    public GameObject BackgroundObject;
    public Text InformationText;
    string username;  // Target's name
    double userscore; // Target's score
    static User[] user;  // This class array has the getting data from firebase DB.
    int TargetIndex;
    long UserCount;

    void Start()
    {
        // Set up the editor befor calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://testdb-be2e2.firebaseio.com/");  
        //

        BackgroundObject.SetActive(false);
        GetTargetValue();
        GetDB();
    }

    private void GetTargetValue()
    {
        username = SharingData.User[0].name;
        userscore = SharingData.User[0].score;
    }

    private void GetDB()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("MindCar") //Individual
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    // Handle the error...
                    Debug.Log("Failed");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    // Do something with snapshot...

                    int count = 0;
                    UserCount = task.Result.ChildrenCount;
                    user = new User[UserCount];

                    // Store user's name & score in the array of the class 'User'  
                    foreach (DataSnapshot ChildSnapshot in task.Result.Children)
                    {
                        string json = ChildSnapshot.GetRawJsonValue();
                        user[count] = JsonUtility.FromJson<User>(json);      
                        //Debug.Log(user[count].name + " / " + user[count].AvgBeta + " / " + user[count].MaxBeta + " / " + user[count].Score + " / " + user[count].SustainedAttentionLevel);
                        count++;
                    }

                    SortScoreDB();
                }
            });

    }

    private void SortScoreDB()
    {    // Sort the whole data in order by score.
        User temp = new User();
        
        for (int i = 0; i < UserCount - 1; i++)
        {
            for (int j = 0; j < UserCount - i - 1; j++)
            {
                if (user[j].Score < user[j + 1].Score)
                {
                    temp = user[j];
                    user[j] = user[j + 1];
                    user[j + 1] = temp;
                }
            }
        }

        //Debug.Log(UserCount);
        //for (int count = 0; count < UserCount; count++)
        //{
        //    Debug.Log(user[count].name + " / " + user[count].AvgBeta + " / " + user[count].MaxBeta + " / " + user[count].Score + " / " + user[count].SustainedAttentionLevel);
        //}
        FindRank();
    }    

    private void FindRank()
    {
        
        for (int i = 0; i < UserCount; i++)
        {
            if(user[i].name == username)
            {
                TargetIndex = i;
                break;
            }
        }


        InformationPrint();
    }

    private void InformationPrint()
    {
        InformationText.text = "    -The Ranking-    " + "\n" +
                               "1. " + user[0].Score.ToString("f2") + "m" + " / " + user[0].name + " (" + user[0].Case + ")" + "\n" +
                               "2. " + user[1].Score.ToString("f2") + "m" + " / " + user[1].name + " (" + user[1].Case + ")" + "\n" +
                               "3. " + user[2].Score.ToString("f2") + "m" + " / " + user[2].name + " (" + user[2].Case + ")" + "\n" +
                               "\n" + "\n" +
                               "Name : " + username + " (" + user[TargetIndex].Case + ")" + "\n" +
                               "Score : " + userscore.ToString("f2") + "m" + "\n" +
                               "Time : " + user[TargetIndex].Time.ToString("f1") + "sec" + "\n" +
                               "Rank : " + (TargetIndex + 1).ToString() + "\n" +
                               "You are in top " + Top() +"% " + "\n" +
                               "AvgBeta : " + user[TargetIndex].AvgBeta + "\n" +
                               "MaxBeta : " + user[TargetIndex].MaxBeta + "\n" +
                               "SAL : " + user[TargetIndex].SustainedAttentionLevel + "\n" +
                               "(Sustained Attention Level)" + "\n";
    }

    string Top()
    {   
        return (((TargetIndex + 1) / (double)UserCount) * 100).ToString("f1");
    }
}


