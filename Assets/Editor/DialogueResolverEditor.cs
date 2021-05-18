using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Dialogue Resolver Editor Class
/// 
/// This class is a Custom Editor for the Dialogue Resolver class.
/// It provides the game designer with a way to test the full functionallity of the dialogues from within the Unity's UI with no need to create the in-game UI. 
/// </summary>
[CustomEditor(typeof(DialogueResolver))]
[CanEditMultipleObjects]
public class DialogueResolverEditor : Editor
{
    /// <summary>
    /// The Dialogue Resolver that this editor will handle.
    /// </summary>
    DialogueResolver dialogueResolver;

    /// <summary>
    /// The character name that the user will provide to start his Dialogue.
    /// </summary>
    string characterName = "";

    /// <summary>
    /// This method adds aditional functionallity to the default editor.
    /// Extra components:
    ///     A text area for a character name that will start speaking.
    ///     A button to start the character's Dialogue
    ///     A button to end the currently active Dialogue.
    ///     A button for every answer of the dialogue for the user to select.
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        characterName = EditorGUILayout.TextField("Enter Character to speak first", characterName);
        dialogueResolver = (DialogueResolver)target;

        if (GUILayout.Button("Start Dialogue"))
        {
            DialogueNode dialogueNode = Array.Find(dialogueResolver.dialogueNodes.ToArray(), dNode => dNode.CharacterName == characterName);

            if (dialogueNode != null)
            {
                dialogueResolver.onDialogueStart?.Invoke(dialogueNode);
            }
            else
            {
                Debug.Log("The character name provided cannot be found...");
            }
        }

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if (dialogueResolver.currentDialogue.IsActive)
        {

            for (int i = 0; i < dialogueResolver.currentDialogue.DalogueAnswers.Length; i++)
            {
                DrawUserOption(i);
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (GUILayout.Button("End Dialogue"))
        {
            if (dialogueResolver.currentDialogue.IsActive)
            {
                dialogueResolver.onDialogueEnd?.Invoke();
            }
            else
            {
                Debug.Log("There are no Active Dialogues...");
            }
        }

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Serialize Current Dialogue"))
        {
            dialogueResolver.SeriallizeDialogue();
        }

        if (GUILayout.Button("Deserialize to Current Dialogue"))
        {
            dialogueResolver.DeserializeDialogue();
        }

        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// This method takes an integer and draws a new button in the edtor with this integer as a label.
    /// Then, it invokes the onUserOption delegate of the current dialogue giving the sane integer as index.
    /// </summary>
    /// <param name="index"> The label / Answer index</param>
    public void DrawUserOption(int index)
    {
        if (GUILayout.Button(new GUIContent(index.ToString(), "Press to Select"), GUILayout.Width(40), GUILayout.Height(40)))
        {
            dialogueResolver.currentDialogue.onUserOption?.Invoke(index);
        }
    }
}
