using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;




public class Menu : MonoBehaviour
{

    public void LoadOn()
    {
        SceneManager.LoadScene("PlayList");
    }

    public void Quit()
    {
        Application.Quit();
    }

}


 
