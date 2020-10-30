using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateController : MonoBehaviour
{
    private static int currentFrame = 0;
    public UnityEvent m_NewFrame = new UnityEvent();
    public static GameStateController _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        if (m_NewFrame == null)
            m_NewFrame = new UnityEvent();
    }

    public void NewFrame(int newframe)
    {
        currentFrame = newframe;
        m_NewFrame.Invoke();
    }

    public static int GetCurrentFrame()
    {
        return currentFrame;
    }

    public void testFunction()
    {
        int value = GetCurrentFrame();
        print("Frame is now: " + value);
    }
}
