﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("play");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("shoo shoo");
    }
}