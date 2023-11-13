using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkInteract : Interactable
{

    NPCCharacter npcCharacter;
    NPCDefinition npcDefinition;

    private void Awake()
    {
        npcCharacter = GetComponent<NPCCharacter>();
        npcDefinition = npcCharacter.character;
    }

    public override void Interact(Character character)
    {
        DialogueContainer dialogueContainer = npcDefinition.generalDialogues[UnityEngine.Random.Range(0, npcDefinition.generalDialogues.Count)];
        npcCharacter.IncreaseRelationship(10);

        GameManager.instance.dialogueSystem.Initialize(dialogueContainer);
    }
}
