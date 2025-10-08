using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogCharacter", menuName = "DialogSys/DialogCharacter")]
public class DialogCharacter : ScriptableObject
{
    public string characterName;
    public Color frameColor;
    public Color textColor;
    public Sprite characterBaseImage;
    [Header("Expressions")]
    public Sprite characterNeutralExpression;
    public Sprite characterHappyExpression;
    public Sprite characterSadExpression;
    public Sprite characterEmbarassedExpression;
    public Sprite characterAngryExpression;

    public Sprite[] GetExpressions()
    {
        return new Sprite[]
        {
            characterNeutralExpression,
            characterHappyExpression,
            characterSadExpression,
            characterEmbarassedExpression,
            characterAngryExpression,
        };
    }
}
