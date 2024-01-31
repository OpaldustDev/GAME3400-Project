using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EmptyDialogue", menuName = "ScriptableObjects/DialogueAsset", order = 2)]
public class PopupDialogueSO : ScriptableObject
{
    [TextArea]
    public string dialogue;
}
