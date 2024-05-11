using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Party Member")]
public class PartyMemberInfo : ScriptableObject
{
    public string MemberName;
    public int StartingLevel;
    public int BaseHealth;
    public int BaseEnergy;
    public int BaseStrength;
    public int BaseSmarts;
    public int BaseInitiative;

    public GameObject MemberBattleVisualPrefab; // What will be displayed in Battle
    public GameObject MemberOverworldVisualPrefab; // What will be displayed in the Overworld
}
