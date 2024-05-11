using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create New Enemy")]
public class EnemyInfo : ScriptableObject
{
    public string EnemyName;
    public int BaseHealth;
    public int BaseEnergy;
    public int BaseStrength;
    public int BaseSmarts;
    public int BaseInitiative;

    public GameObject EnemyVisualPrefab; // Used in Battle Scene    
}
