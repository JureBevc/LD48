using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerData", menuName = "ScriptableObjects/PowerData")]
public class PowerData : ScriptableObject
{
    public PowerType powerType;
    public string powerTitle;
    [TextArea] public string powerDescription;
    public int moneyCost;
    public int unitCost;
    public bool oneUse;
}
