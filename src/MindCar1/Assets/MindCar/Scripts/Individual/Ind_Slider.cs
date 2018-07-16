using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ind_Slider : MonoBehaviour
{

    public bool CheckPause = true;
    public bool CheckTheEnd = false;
    public Slider Slider_1P;  //reference for slider_1P
    public Image Fill_1P;  // assign in the editor the "Fill_1P"

    //----------------------------------
    int BeginningCommmand = 1;  //1
    int FinalCommmand = 10;     //10
    int Index = 0;    
    public float DefaultSpeed = 5.0f;
    //----------------------------------    

    double ReceiveCommand;
    public float Speed;          // To send the command which means what is the car's speed.


    private void ChangeSlider(float target_1P, Color color_1P)
    {
        Slider_1P.value = Mathf.MoveTowards(Slider_1P.value, target_1P, 0.01f); // from the current one to target.
        Fill_1P.color = Color.Lerp(Fill_1P.color, color_1P, 0.1f);
    }


    void Update()
    {
        if (CheckPause)
            return;
        if (CheckTheEnd)  // If the checking result from ResultUI is the end, it will not do anything.
            return;

        var epoc = GameObject.Find("Camera").GetComponent<Ind_SystemControl>();
        ReceiveCommand = (epoc.command);
        
        // Checking protocol(0 ~ 10). 0 = default speed        
        //--------------------------------------------------------------------------    
        if (ReceiveCommand < BeginningCommmand || ReceiveCommand > FinalCommmand)
        {  // If exception, than default case. And checking the default command(protocol == 0).
            Speed = DefaultSpeed;
            ChangeSlider(0f, SupportSlider.SliderColor[0]);
        }
        else
        {
            for (int i = BeginningCommmand; i <= FinalCommmand; i++)
            {
                if (ReceiveCommand >= i && ReceiveCommand < (i + 1))  // If it was 1, then (1 <= x < 2).
                {
                    Speed = SupportSlider.CarSpeed[Index];
                    ChangeSlider(SupportSlider.SliderTarget[Index], SupportSlider.SliderColor[Index]);
                    break;
                }
                Index++;
            }

            Index = 0;
        }
        //-------------------------------------------------------------------------- 
    }

    void OnGUI()
    {
       // GUI.Label(new Rect(50, 40, 3000, 3000), " Received Command : " + ReceiveCommand.ToString("f5"));
    }
}