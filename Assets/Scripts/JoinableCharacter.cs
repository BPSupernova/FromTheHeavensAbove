using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinableCharacter : MonoBehaviour
{
    public PartyMemberInfo memberToJoin;
    [SerializeField] private GameObject interactionPrompt;

    void Start() {
        CheckIfJoined();
    }

    public void ShowInteractionPrompt(bool showPrompt) {
        if (showPrompt) {
            interactionPrompt.SetActive(true);
        } else {
            interactionPrompt.SetActive(false);
        }
    }

    public void CheckIfJoined() {
        List<PartyMember> currParty = GameObject.FindFirstObjectByType<PartyManager>().GetCurrentParty();

        for (int i = 0; i < currParty.Count; i++) {
            if (currParty[i].MemberName == memberToJoin.MemberName) {
                gameObject.SetActive(false);
            }
        }
    }
}
