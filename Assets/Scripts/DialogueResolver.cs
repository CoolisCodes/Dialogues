using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dialogue Resolver Class
/// 
/// This class is responsible for holding all the available dialogues and to handle them.
/// This class can also Serialize the cerrent dialogue and Deserialize back to it (it can also serialize and deserialize any dialogue but it is done this way for the demo purposes).
/// </summary>
public class DialogueResolver : MonoBehaviour
{
    /// <summary>
    /// The current dialogue that is handled.
    /// </summary>
    public DialogueNode currentDialogue = null;

    /// <summary>
    /// All the available dialogues.
    /// </summary>
    [SerializeField]
    public List<DialogueNode> dialogueNodes = new List<DialogueNode>();

    /// <summary>
    /// Delegate method that will execute each time a dialogue starts.
    /// </summary>
    public Action<DialogueNode> onDialogueStart;

    /// <summary>
    /// Delegate method that will execute each time a dialogue ends.
    /// </summary>
    public Action onDialogueEnd;

    /// <summary>
    /// The serialized form of a dialogue.
    /// </summary>
    [TextArea(6, 100)]
    public string serializedDialogue;

    /// <summary>
    /// Assigning Dialogue start and End delegates to the acoording methods.
    /// </summary>
    private void OnEnable()
    {
        onDialogueStart += OnDialogueStart;
        onDialogueEnd += OnDialogueEnd;
    }

    /// <summary>
    /// This method takes a Dialogue Node and starts it.
    /// </summary>
    /// <param name="dialogueNode"> The dialogue that will start</param>
    public void OnDialogueStart(DialogueNode dialogueNode)
    {
        currentDialogue = dialogueNode;
        dialogueNode.StartDialogue(this);
    }

    /// <summary>
    /// This methods ends the current Dialogue.
    /// </summary>
    public void OnDialogueEnd()
    {
        currentDialogue.EndDialogue();
    }

    /// <summary>
    /// This methods serializes the current Dialogue to JSON
    /// </summary>
    public void SeriallizeDialogue()
    {
        if (currentDialogue != null)
        {
            serializedDialogue = JsonUtility.ToJson(currentDialogue, true);
        }
    }

    /// <summary>
    /// This method takes the a serialized Dialogue and deserialized it to the current Dialogue.
    /// </summary>
    public void DeserializeDialogue()
    {
        DialogueNode dialogueNode = JsonUtility.FromJson<DialogueNode>(serializedDialogue);

        if (dialogueNode == null) return;

        currentDialogue.CharacterName = dialogueNode.CharacterName;
        currentDialogue.DalogueAnswers = dialogueNode.DalogueAnswers;
    }

    /// <summary>
    /// Deassignig all delegates from their methods to prevent memory leaking.
    /// </summary>
    private void OnDisable()
    {
        onDialogueStart -= OnDialogueStart;
        onDialogueEnd -= OnDialogueEnd;
    }
}
