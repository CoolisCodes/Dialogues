using System;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    [SerializeField]
    private string characterName;

    public string CharacterName
    {
        get { return characterName; }
        set { characterName = value; }
    }

    [SerializeField]
    private string nextCharacterName;

    public string NextCharacterName
    {
        get { return characterName; }
        set { characterName = value; }
    }

    [SerializeField]
    [TextArea]
    private string[] dialogueScentences;

    public string[] DalogueScentences
    {
        get { return dialogueScentences; }
        set { dialogueScentences = value; }
    }

    public Action onContinue;

    private int currentScententceIndex = 0;

    private bool isActive = false;

    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    //private bool 

    private DialogueResolver dialogueResolver;

    public void StartDialogue(DialogueResolver dialogueResolver)
    {
        this.dialogueResolver = dialogueResolver;
        isActive = true;
        Debug.Log($"Hi im, {characterName}!");
    }

    public void IterateScentence()
    {
        if (currentScententceIndex >= dialogueScentences.Length)
        {
            dialogueResolver.onDialogueEnd?.Invoke();

            return;
        }

        Debug.Log($"{characterName}: {dialogueScentences[currentScententceIndex]}");

        currentScententceIndex++;
    }

    public void EndDialogue()
    {
        currentScententceIndex = 0;
        isActive = false;

        dialogueResolver.currentDialogue = null;

        GoToNextCharacter();
    }

    public void GoToNextCharacter()
    {
        DialogueNode nextDialogue = Array.Find(dialogueResolver.dialogueNodes.ToArray(), dialogueNode => dialogueNode.CharacterName == nextCharacterName);

        if (nextDialogue == null) return;

        dialogueResolver.onDialogueStart?.Invoke(nextDialogue);
    }
}
