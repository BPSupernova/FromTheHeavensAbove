using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private bool infrontOfPartyMember;
    private GameObject joinableMember;

    private const string NPC_Joinable_TAG = "NPCJoinable";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
