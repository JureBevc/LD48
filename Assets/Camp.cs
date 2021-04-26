using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour
{
    public static Camp instance;

    public GameObject campUI;
    public List<PowerData> powers;
    public RandomPowerButton[] randomPowerButtons;

    private List<GameObject> units = new List<GameObject>();
    public Camp()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowCamp()
    {
        foreach (Unit unit in Combat.instance.GetPlayerUnits())
        {
            GameObject obj = new GameObject("Unit");
            obj.AddComponent<SpriteRenderer>().sprite = unit.GetComponentInChildren<SpriteRenderer>().sprite;
            obj.transform.position = Random.insideUnitCircle.normalized * Random.Range(2.8f, 3.2f);
            obj.transform.parent = transform;
            units.Add(obj);
        }

        foreach (RandomPowerButton r in randomPowerButtons)
        {
            r.SetInfoForPower(powers[Random.Range(0, powers.Count)]);
        }

        gameObject.SetActive(true);
        campUI.SetActive(true);
    }

    public void HideCamp()
    {
        foreach (GameObject unit in units)
        {
            Destroy(unit);
        }
        units = new List<GameObject>();
        gameObject.SetActive(false);
        campUI.SetActive(false);
    }

    public void ActivatePower(PowerData powerData)
    {
        if (powerData.oneUse)
        {
            powers.Remove(powerData);
        }
        switch (powerData.powerType)
        {
            case PowerType.ACCURACY:
                Combat.instance.accuracyStage += 1;
                break;
            case PowerType.ATTACK_SPEED:
                Combat.instance.attackSpeedStage += 1;
                break;
            case PowerType.BUY_UNIT:
                Combat.instance.CreatePlayerUnit();
                break;
            case PowerType.BUY_UNIT_DISCOUNT:
                for (int i = 0; i < 3; i++)
                {
                    Combat.instance.CreatePlayerUnit();
                }
                break;
            case PowerType.DIVINE_BLESSING:
                Combat.instance.divineBlessing = true;
                break;
            case PowerType.DIVINE_JUDGEMENT:
                Combat.instance.divineJudgement = true;
                break;
            case PowerType.EVASION:
                Combat.instance.evasionStage += 1;
                break;
            case PowerType.HOLY_INTERVENTION:
                Combat.instance.holyIntervention = true;
                break;
            case PowerType.SELL_UNITS:
                Combat.instance.KillUnits(powerData.unitCost);
                Combat.instance.collectedCoins += powerData.moneyCost;
                Combat.instance.UpdateMoneyText();
                break;
        }
    }
}
