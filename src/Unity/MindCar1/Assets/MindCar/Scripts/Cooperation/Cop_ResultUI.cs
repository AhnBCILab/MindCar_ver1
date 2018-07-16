using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cop_ResultUI : MonoBehaviour
{
    public GameObject ResultUIObject;
    public Text ResultScore1P;
    public float LimitTime = 90.0f;

    bool EndCondition = false;
    public bool CheckPause = true;

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

        var Car_1P = GameObject.Find("Car_1P").GetComponent<Cop_MoveCar1>(); // Reference the script 'Ind_MoveCar1'
        var DefaultUIObject = GameObject.Find("Camera").GetComponent<Cop_DefaultUI>();
        var SliderObject = GameObject.Find("Camera").GetComponent<Cop_Slider>();

        // End Condition (= game end trigger)
        if (DefaultUIObject.timeCount > LimitTime)
        {
            ResultUIObject.SetActive(true);
            Car_1P.CheckTheEnd = true;
            DefaultUIObject.CheckTheEnd = true;
            SliderObject.CheckTheEnd = true;

            ResultScore1P.text = DefaultUIObject.DefaultScore1P.text;
            EndCondition = true;
        }
    }


}
