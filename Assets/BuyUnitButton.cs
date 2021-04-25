using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyUnitButton : MonoBehaviour
{
    public void ButonClick(){
        AudioPlayer.instance.PlayClick();
    }
}
