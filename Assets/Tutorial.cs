using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public TutorialContent tutorialContent;
    public UnityEvent onComplete;

    private bool triggered = false;
    private bool notPaused = true;
    
    public virtual void TriggerTutorial()
    {
        if (!triggered)
        {
            TutorialManager.Push(this);
            triggered = true;
        }
        else
        {
            return;
        }
    }

    public virtual void Pause()
    {
        notPaused = false;
    }

    public virtual void Resume()
    {
        notPaused = false;
    }
}
