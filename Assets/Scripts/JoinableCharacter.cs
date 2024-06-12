using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinableCharacter : MonoBehaviour
{
    public PartyMemberInfo memberToJoin;
    [SerializeField] private GameObject interactionPrompt;

    public void ShowInteractionPrompt(bool showPrompt) {
        if (showPrompt) {
            interactionPrompt.SetActive(true);
        } else {
            interactionPrompt.SetActive(false);
        }
    }
}
