using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour {

    public void LoadSceneGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void Salir()
    {
        Application.Quit();
    }
}
