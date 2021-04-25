using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomPowerButton : MonoBehaviour
{

    public TextMeshProUGUI title, description, cost;
    public Image costImage;
    public Sprite unitCostSprite;
    private PowerData powerData;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetInfoForPower(PowerData powerData)
    {
        GetComponent<Button>().interactable = true;
        this.powerData = powerData;
        title.text = powerData.powerTitle;
        description.text = powerData.powerDescription;
        cost.text = "" + (powerData.unitCost > 0 ? powerData.unitCost : powerData.moneyCost);
        costImage.GetComponent<Animator>().enabled = true;
        if (powerData.unitCost > 0)
        {
            costImage.GetComponent<Animator>().enabled = false;
            costImage.sprite = unitCostSprite;
        }
    }

    public void Click()
    {
        if (powerData.unitCost > 0)
        {
            if (powerData.unitCost >= Combat.instance.GetPlayerUnits().Count)
            {
                return;
            }
            else
            {
                Combat.instance.KillUnits(powerData.unitCost);
            }
        }
        else
        {
            if (powerData.moneyCost >= Combat.instance.collectedCoins)
            {
                return;
            }
            else
            {
                Combat.instance.collectedCoins -= powerData.moneyCost;
                Combat.instance.UpdateMoneyText();
            }
        }
        AudioPlayer.instance.PlayClick();
        Camp.instance.ActivatePower(powerData);

        GetComponent<Button>().interactable = false;
    }
}
