using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cop_Slider : MonoBehaviour
{

    public bool CheckPause = true;
    public bool CheckTheEnd = false;

    public int MaxValue = 100;
    public Slider Slider_1P;  //reference for slider_1P
    public Slider Slider_2P;  //reference for slider_2P

    public Image Fill_1P;  // assign in the editor the "Fill_1P"
    public Image Fill_2P;  // assign in the editor the "Fill_2P"

    public float[] SliderTarget = { 0f, 0.3f, 0.7f, 1.0f };   // Slider bar : Default -> First -> Second -> Third
    public Color[] SliderColor = { Color.white, Color.yellow, Color.white /*might be orange*/, Color.red }; // The color of sliders
    public float[] CarSpeed = { 2.0f, 5.0f, 15.0f, 30.0f };   // Car's speed : Default -> First -> Second -> Third

    int Index_1P = 0;
    int Index_2P = 0;

    //----------------------------------
    int BeginningCommmand = 21;  //21
    int FinalCommmand = 36;      //36
    //----------------------------------

    double ReceivedCommand;
    public float Speed;          // To send the command which means what is the car's speed.


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

        var epoc = GameObject.Find("Camera").GetComponent<Cop_SystemControl>();
        ReceivedCommand = (epoc.command);


        // Cooperation game has the cases which are [21,36].   
        // To control the each signal of users
        // It determines the car speed and slider of each players. 
        //--------------------------------------------------------------------------    
        if (ReceivedCommand < BeginningCommmand || ReceivedCommand > FinalCommmand)
        {  // if exception, than default case
            Speed = CarSpeed[0];
            ChangeSlider(SliderTarget[0], SliderTarget[0], SliderColor[0], SliderColor[0]);
        }
        else
        {
            for (int i = BeginningCommmand; i <= FinalCommmand; i++)
            {
                if (ReceivedCommand < (i + 1) && ReceivedCommand >= i)  // If it was 4, then (4 <= x < 5).
                {                    
                    ChangeSlider(SliderTarget[Index_1P], SliderTarget[Index_2P], SliderColor[Index_1P], SliderColor[Index_2P]);

                    if ((ReceivedCommand % 5) == 0)
                    {
                        Speed = CarSpeed[Index_2P];  // Only move on this case which means each player's speed is same. 
                    }
                    else
                    {
                        Speed = CarSpeed[0];
                    }
                }
                Index_2P++;
                if (Index_2P == 4)
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
        GUI.Label(new Rect(50, 40, 3000, 3000), " Received value : " + ReceivedCommand.ToString("f5"));
    }
}
