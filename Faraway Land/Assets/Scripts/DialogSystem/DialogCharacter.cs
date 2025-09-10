using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogCharacter", menuName = "DialogSys/DialogCharacter")]
public class DialogCharacter : ScriptableObject
{
    public string characterName;
    public Color frameColor;
    public Color textColor;
    public Sprite characterPic;
}
