using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayList : MonoBehaviour 
{    
    public void LoadOnIndividual()
    {
        SceneManager.LoadScene("Ind_Username"); 
    }

    public void LoadOnCompetition()
    {
        SceneManager.LoadScene("Com_Username");
    }

    public void LoadOnCooperation()
    {
        SceneManager.LoadScene("Cop_Username");
    }

    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }

}

