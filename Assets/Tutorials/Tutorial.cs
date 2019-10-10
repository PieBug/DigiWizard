using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public abstract class Tutorial : MonoBehaviour
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

    public abstract bool IsComplete();

    private void Update()
    {
        if (!triggered || !notPaused)
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
        notPaused = true;
    }
}
