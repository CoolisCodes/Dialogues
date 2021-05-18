using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueResolver))]
[CanEditMultipleObjects]
public class DialogueResolverEditor : Editor
{
    SerializedProperty resolver;
    string characterName = "";

    void OnEnable()
    {
        resolver = serializedObject.FindProperty("DialogueResolver");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        characterName = EditorGUILayout.TextField("Enter Character to speak first", characterName);
        DialogueResolver dialogueResolver = (DialogueResolver)target;

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

        if (GUILayout.Button("Continue"))
        {
            if (dialogueResolver.currentDialogue.IsActive)
            {
                dialogueResolver.currentDialogue.IterateScentence();
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
}
