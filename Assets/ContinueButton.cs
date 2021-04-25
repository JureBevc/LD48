using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{

    public void ExitCamp()
    {
        AudioPlayer.instance.PlayClick();
        Camp.instance.HideCamp();
        Map.instance.gameObject.SetActive(true);
    }
}
