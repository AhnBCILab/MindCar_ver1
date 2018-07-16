using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Com_Slider : MonoBehaviour
{

    public bool CheckPause = true;
    public bool CheckTheEnd = false;

    public int MaxValue = 100;
    public Slider Slider_1P;  //reference for slider_1P
    public Slider Slider_2P;  //reference for slider_2P

    public Image Fill_1P;  // assign in the editor the "Fill_1P"
    public Image Fill_2P;  // assign in the editor the "Fill_2P"       
    
    //----------------------------------
    int Index_1P = 0;
    int Index_2P = 0;
    int BeginningCommmand = 1;  // 1
    int FinalCommmand = 121;    // 121
    int SDResolution = 11;      // It means 10.
    //----------------------------------

    double ReceivedCommand;
    public float Speed_1P;          // To send the command which means what is the car's speed.
    public float Speed_2P;

    private void ChangeSlider(float target_1P, float target_2P, Color color_1P, Color color_2P)
    {
        Slider_1P.value = Mathf.MoveTowards(Slider_1P.value, target_1P, 0.01f); // from current to target.
        Fill_1P.color = Color.Lerp(Fill_1P.color, color_1P, 0.1f);

        Slider_2P.value = Mathf.MoveTowards(Slider_2P.value, target_2P, 0.01f);
        Fill_2P.color = Color.Lerp(Fill_2P.color, color_2P, 0.1f);
    }

    void Update()
    {
        if (CheckPause)
            return;
        if (CheckTheEnd)  // If the checking result from ResultUI is the end, it will not do anything.
            return;

        var epoc = GameObject.Find("Camera").GetComponent<SystemControl>();
        ReceivedCommand = (epoc.command);

        // Checking protocol(1 ~ 121). 
        // It determines the car speed and slider of each players. 
        //--------------------------------------------------------------------------    
        if (ReceivedCommand < BeginningCommmand || ReceivedCommand > FinalCommmand)
        {  // if exception, than default case. And checking the default command(protocol == 0).
            Speed_1P = SupportSlider.CarSpeed[0];
            Speed_2P = SupportSlider.CarSpeed[0];
            ChangeSlider(0f, 0f, SupportSlider.SliderColor[0], SupportSlider.SliderColor[0]);
        }
        else
        {
            for (int i = BeginningCommmand; i <= FinalCommmand; i++)
            {
                // i == FinalCommmand 인 경우 고려.
                
                if (ReceivedCommand >= i && ReceivedCommand < (i + 1))  // If it was 1, then (1 <= x < 2).
                {        
                    Speed_1P = SupportSlider.CarSpeed[Index_1P];
                    Speed_2P = SupportSlider.CarSpeed[Index_2P];
                    ChangeSlider(SupportSlider.SliderTarget[Index_1P], SupportSlider.SliderTarget[Index_2P], SupportSlider.SliderColor[Index_1P], SupportSlider.SliderColor[Index_2P]);
                }
                Index_2P++;
                if (Index_2P == SDResolution)
                {
                    Index_1P++;
                    Index_2P = 0;
                }
            }
            Index_1P = 0;
            Index_2P = 0;
        }
        //--------------------------------------------------------------------------          
    }


    void OnGUI()
    {
        //GUI.Label(new Rect(50, 40, 3000, 3000), " Received value : " + ReceivedCommand.ToString("f5"));
    }
}
