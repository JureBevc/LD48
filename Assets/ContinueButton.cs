using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{

    public void ExitCamp()
    {
        AudioPlayer.instance.PlayClick();
        System.Action campAction = new System.Action(() =>
        {
            Camp.instance.HideCamp();
            Map.instance.gameObject.SetActive(true);
        });
        SceneTransition.instance.StartTransition(campAction);

    }
}
