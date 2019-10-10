using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDialouge", menuName = "Dialouge")]
public class TutorialContent : ScriptableObject
{
    public enum Speaker
    {
        headWizard,
    }

    public Speaker speaker;

    [TextArea(5, 10)]
    public string textContent;
    
}
