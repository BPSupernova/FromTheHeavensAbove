using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject joinPopUp;
    [SerializeField] private TextMeshProUGUI joinPopUpText;

    private bool infrontOfPartyMember;
    private GameObject joinableMember;
    private PlayerControls playerControls;

    private const string NPC_Joinable_TAG = "NPCJoinable";
    private const string JOINED_PARTY_MESSAGE = " joined the party!";
    
    private void Awake() {
        playerControls = new PlayerControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerControls.Player.Interact.performed += _ => Interact();
    }
    
    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Interact() {
        if (infrontOfPartyMember == true && joinableMember != null) {
            MemberJoined(joinableMember.GetComponent<JoinableCharacter>().memberToJoin);
            infrontOfPartyMember = false;
            joinableMember = null;
        }
    }

    private void MemberJoined(PartyMemberInfo partyMember) {
        GameObject.FindFirstObjectByType<PartyManager>().AddMemberToParty(partyMember.MemberName);
        joinableMember.GetComponent<JoinableCharacter>().CheckIfJoined();
        joinPopUp.SetActive(true);
        joinPopUpText.text = partyMember.MemberName + JOINED_PARTY_MESSAGE; 
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == NPC_Joinable_TAG) {
            infrontOfPartyMember = true;
            joinableMember = other.gameObject;
            joinableMember.GetComponent<JoinableCharacter>().ShowInteractionPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == NPC_Joinable_TAG) {
            infrontOfPartyMember = false;
            joinableMember.GetComponent<JoinableCharacter>().ShowInteractionPrompt(false);
            joinableMember = null;
        }
    }
}
