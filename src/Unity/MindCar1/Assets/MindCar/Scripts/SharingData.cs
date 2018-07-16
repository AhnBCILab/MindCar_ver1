using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class SharingData
{
    public static List<Username> User = new List<Username>();
}

public class SupportSlider
{
    // In this case, the slider's resolution is ten. 
    public static float[] SliderTarget = {0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f };   // Slider bar : Default -> First -> Second -> Third
    public static Color[] SliderColor =  { Color.white, Color.white, Color.white, Color.yellow, Color.yellow, Color.yellow, 
     /* For expressing the orange color */ new Color(255F, 195F, 0F, 255F), new Color(255F, 195F, 0F, 255F), new Color(255F, 195F, 0F, 255F), Color.red, Color.red }; // The color of sliders
    public static float[] CarSpeed = { 5f, 7.5f, 10.0f, 12.5f, 15.0f, 17.5f, 20.0f, 22.5f, 25.0f, 27.5f, 30.0f };   // Car's speed 
}

public class Username
{
    public string name;
    public double score;
}


public class User
{
    public string Case;
    public string name;
    public double MaxBeta;
    public double AvgBeta;
    public double Score;
    public float Time;
    public int SustainedAttentionLevel;

    public User()
    {
        this.Case = "";
        this.name = "";
        this.MaxBeta = 0;
        this.AvgBeta = 0;
        this.Score = 0;
        this.Time = 0;
        this.SustainedAttentionLevel = 0;
    }

    public User(string Case, string name, double MaxBeta, double AvgBeta, double Score, float Time, int SustainedAttentionLevel)
    {
        this.Case = Case;
        this.name = name;
        this.MaxBeta = MaxBeta;
        this.AvgBeta = AvgBeta;
        this.Score = Score;
        this.Time = Time;
        this.SustainedAttentionLevel = SustainedAttentionLevel;
    }
}


