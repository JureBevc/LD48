﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginButton : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject moneyDisplay;
    public void StartGame()
    {
        AudioPlayer.instance.PlayClick();
        mainMenu.SetActive(false);
        Map.instance.gameObject.SetActive(true);
        moneyDisplay.SetActive(true);
    }
}