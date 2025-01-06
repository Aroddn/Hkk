using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerSceen : MonoBehaviour
{
    public GameObject ScreenContent;
    public static MultiPlayerSceen Instance;
    void Awake()
    {
        Instance = this;
        HideScreen();
    }

    public void ShowScreen()
    {
        ScreenContent.SetActive(true);
    }

    public void HideScreen()
    {
        ScreenContent.SetActive(false);
    }
}
