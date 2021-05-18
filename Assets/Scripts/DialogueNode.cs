using System;
using UnityEngine;

/// <summary>
/// Dialogue Node Class
/// 
/// This class represents a dialogue object that has the character that speaks and some answers based on the input of the user.
/// A dialogue can start at any given point and Once is started (IsActive), it can be serialized to plain text and deserialized back to a Dialogue Node object.
/// The dialogue can have as many answeres needed to fit the situation and can jump automatically to the next dialogue -IF- there is one provoded.
/// </summary>
[Serializable]
public class DialogueNode
{
    /// <summary>
    /// The name of the character.
    /// </summary>
    [SerializeField]
    private string characterName;

    public string CharacterName
    {
        get { return characterName; }
        set { characterName = value; }
    }

    /// <summary>
    /// The name of the next character after the dialogue ends.
    /// </summary>
    [SerializeField]
    private string nextCharacterName;

    public string NextCharacterName
    {
        get { return characterName; }
        set { characterName = value; }
    }

    /// <summary>
    /// An array of all the answers of the dialogue.
    /// </summary>
    [SerializeField]
    [TextArea]
    private string[] dialogueAnswers;

    public string[] DalogueAnswers
    {
        get { return dialogueAnswers; }
        set { dialogueAnswers = value; }
    }

    /// <summary>
    /// Delegate method that will execute every time the user selects an option (to get an answer).
    /// </summary>
    public Action<int> onUserOption;

    /// <summary>
    /// The current status of the dialogue.
    /// </summary>
    private bool isActive = false;

    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    /// <summary>
    /// The Dialogue Resolver that handles the dialogue.
    /// </summary>
    private DialogueResolver dialogueResolver;

    /// <summary>
    /// This method takes a Dialogue Resolver and starts the dialogue by activating it and assigning the OnUserOption method to onUserOption delegate.
    /// Then, it proceeds to greet the user.
    /// </summary>
    /// <param name="dialogueResolver"> The Dialogue Resolver that will handle the dialogue.</param>
    public void StartDialogue(DialogueResolver dialogueResolver)
    {
        this.dialogueResolver = dialogueResolver;
        isActive = true;

        onUserOption += OnUserOption;

        Debug.Log($"Hi im {characterName}, how can I help you?");
    }

    /// <summary>
    /// This method takes the index of an Answer and prints the Answer to the user.
    /// </summary>
    /// <param name="index"> The index of the Anser that will be printed.</param>
    public void OnUserOption(int index)
    {
        Debug.Log($"{characterName}: {dialogueAnswers[index]}");
    }

    /// <summary>
    /// This method ednds this dialogue by deactivating it and deassigns the OnUserOption method from it's delegate to prevent memory leaking.
    /// Then, the current dialoge of the Dialogue resolver gets null and the next Dialogue starts.
    /// </summary>
    public void EndDialogue()
    {
        isActive = false;

        onUserOption -= OnUserOption;

        dialogueResolver.currentDialogue = null;

        GoToNextCharacter();
    }

    /// <summary>
    /// This method starts the next Dialogue if it exists.
    /// </summary>
    public void GoToNextCharacter()
    {
        DialogueNode nextDialogue = Array.Find(dialogueResolver.dialogueNodes.ToArray(), dialogueNode => dialogueNode.CharacterName == nextCharacterName);

        if (nextDialogue == null) return;

        dialogueResolver.onDialogueStart?.Invoke(nextDialogue);
    }
}