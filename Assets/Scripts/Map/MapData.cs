using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "MenuData", menuName = "ScriptableObjects/MenuData")]
public class MapData : ScriptableObject
{
    [SerializeField] public LevelData[] levels;
}
