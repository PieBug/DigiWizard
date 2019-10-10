using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private static Stack<Tutorial> tutorialQueue;
    private static Tutorial currentTutorial;
    public static void Push(Tutorial tutorial)
    {
        currentTutorial.Pause();
        tutorialQueue.Push(tutorial);
        currentTutorial = tutorial;
    }

    public static void Pop()
    {
        tutorialQueue.Pop();
        currentTutorial = tutorialQueue.Peek();
        currentTutorial.Resume();
    }
}
