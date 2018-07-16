using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;


public class Com_Database : MonoBehaviour
{
    public GameObject BackgroundObject;
    public Text InformationText;
    string username;  // Target's name
    double userscore; // Target's score

    public int PlayerNum; // ***It determines a player whether the player is 1P(=0) or 2P(=1).

    static User[] user;  // This class array has the getting data of palyer 1 from firebase DB.

    int TargetIndex;
    long UserCount;

    void Start()
    {
        // Set up the editor befor calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://testdb-be2e2.firebaseio.com/"); 

        if(BackgroundObject != null)
        {
            BackgroundObject.SetActive(false);
        }
        GetTargetValue();
        GetDB();
    }

    private void GetTargetValue()
    {
        username = SharingData.User[PlayerNum].name;
        userscore = SharingData.User[PlayerNum].score;
    }

    private void GetDB()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("MindCar") //Competition
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
        
        FindRank();
    }

    private void FindRank()
    {

        for (int i = 0; i < UserCount; i++)
        {
            if (user[i].name == username)
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
                               "You are in top " + Top() + "% " + "\n" +
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

