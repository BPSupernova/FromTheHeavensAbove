using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField] private PartyMemberInfo[] allMembers;
    [SerializeField] private List<PartyMember> currentParty;
    
    [SerializeField] private PartyMemberInfo defaultPartyMember;

    private void Awake() {
        AddMemberToParty(defaultPartyMember.MemberName);
    }
    
    public void AddMemberToParty(string name) {
        for (int i = 0; i < allMembers.Length; i++) {
            if (allMembers[i].MemberName == name) {
                PartyMember newMember = new PartyMember();

                newMember.MemberName = allMembers[i].MemberName;
                newMember.Level = allMembers[i].StartingLevel;
                newMember.CurrentHealth = allMembers[i].BaseHealth;
                newMember.MaxHealth = allMembers[i].BaseHealth;
                newMember.CurrentEnergy = allMembers[i].BaseEnergy;
                newMember.MaxEnergy = allMembers[i].BaseEnergy;
                newMember.Strength = allMembers[i].BaseStrength;
                newMember.Smarts = allMembers[i].BaseSmarts;
                newMember.Initiative = allMembers[i].BaseInitiative;

                newMember.MemberBattleVisualPrefab = allMembers[i].MemberBattleVisualPrefab;
                newMember.MemberOverworldVisualPrefab = allMembers[i].MemberOverworldVisualPrefab;
            
                currentParty.Add(newMember);
            }
        }
    }

    public List<PartyMember> GetCurrentParty() {
        return currentParty;
    }
}

[System.Serializable]
public class PartyMember {
    public string MemberName;
    public int Level;
    public int CurrentHealth;
    public int MaxHealth;
    public int CurrentEnergy;
    public int MaxEnergy;
    public int Strength;
    public int Smarts;
    public int Initiative;
    public int CurrentExp;
    public int MaxExp;

    public GameObject MemberBattleVisualPrefab;
    public GameObject MemberOverworldVisualPrefab;
}