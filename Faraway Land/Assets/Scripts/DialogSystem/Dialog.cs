using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "DialogSys/Dialog")]
public class Dialog : ScriptableObject
{
    public DialogCharacter[] characters;
    public DialogInfo[] dialogChain;
    [Header("Diary Log Entry")]
    [TextArea] public string logEntry;
    public Sprite logDrawing;
}
