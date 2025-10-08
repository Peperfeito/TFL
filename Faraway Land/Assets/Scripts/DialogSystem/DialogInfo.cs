using UnityEngine;

[System.Serializable]
public struct DialogInfo
{
    public int characterIndex;
    public CharacterExpression characterExpression;
    public DialogSide dialogSide;
    [TextArea] public string text;
    
    public bool promptOptions;
    public AnswerOptions[] answerOptions;
}