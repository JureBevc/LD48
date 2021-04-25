using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyUnitButton : MonoBehaviour
{
    public TextMeshProUGUI title, description, cost;
    public PowerData powerData;

    void Start()
    {
        title.text = powerData.powerTitle;
        description.text = powerData.powerDescription;
        cost.text = "" + (powerData.unitCost > 0 ? powerData.unitCost : powerData.moneyCost);
    }
    public void ButonClick()
    {
        AudioPlayer.instance.PlayClick();
        Combat.instance.collectedCoins -= powerData.moneyCost;
        Combat.instance.UpdateMoneyText();
        Camp.instance.ActivatePower(powerData);
    }
}
