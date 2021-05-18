using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueResolver : MonoBehaviour
{
    public DialogueNode currentDialogue = null;

    [SerializeField]
    public List<DialogueNode> dialogueNodes = new List<DialogueNode>();

    public Action<DialogueNode> onDialogueStart;
    public Action onDialogueEnd;

    [TextArea(6, 100)]
    public string serializedDialogue;

    private void OnEnable()
    {
        onDialogueStart += OnDialogueStart;
        onDialogueEnd += OnDialogueEnd;
    }

    public void OnDialogueStart(DialogueNode dialogueNode)
    {
        currentDialogue = dialogueNode;
        dialogueNode.StartDialogue(this);
    }

    public void OnDialogueEnd()
    {
        currentDialogue.EndDialogue();
    }

    public void SeriallizeDialogue()
    {
        if (currentDialogue != null)
        {
            serializedDialogue = JsonUtility.ToJson(currentDialogue, true);
        }

    }

    public void DeserializeDialogue()
    {
        DialogueNode dialogueNode = JsonUtility.FromJson<DialogueNode>(serializedDialogue);

        if (dialogueNode == null) return;

        currentDialogue.CharacterName = dialogueNode.CharacterName;
        currentDialogue.DalogueScentences = dialogueNode.DalogueScentences;
    }

    private void OnDisable()
    {
        onDialogueStart -= OnDialogueStart;
        onDialogueEnd -= OnDialogueEnd;
    }
}
