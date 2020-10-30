﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{
    private static int currentFrame = 0;
    private bool canReturnToMenu = false;
    private float currentVolume = 50f;
    public UnityEvent m_NewFrame = new UnityEvent();
    public static GameStateController _instance;

    [SerializeField] RawImage white;
    [SerializeField] Text bowlingText;
    [SerializeField] Text highscoreText;
    [SerializeField] AkAudioListener akListener;
    [SerializeField] float fadeSpeed = .05f;
    [SerializeField] float textFadeSpeed = .05f;
    [SerializeField] float textWaitTime = 5f;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        if (m_NewFrame == null)
            m_NewFrame = new UnityEvent();

        //EndGame();
    }

    private void Update()
    {
        if(canReturnToMenu && Input.anyKeyDown)
        {
            SceneManager.LoadScene("_MainMenu");
        }
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

    public void EndGame()
    {
        white.gameObject.SetActive(true);
        StartCoroutine(fadeInEnd());
    }

    private IEnumerator fadeInEnd()
    {
        AkSoundEngine.PostEvent("FinalSounds", gameObject);

        float alpha = white.color.a;
        while (alpha < 1)
        {
            alpha += fadeSpeed;
            white.color = new Color(1,1,1,alpha);
            currentVolume = alpha * 100;
            AkSoundEngine.SetRTPCValue("MasterVolume", currentVolume);
            yield return new WaitForFixedUpdate();
        }

        akListener.enabled = false;
        AkSoundEngine.SetRTPCValue("MasterVolume", 50);
        yield return new WaitForSeconds(textWaitTime);

        alpha = bowlingText.color.a;
        while (alpha < 1)
        {
            alpha += textFadeSpeed;
            bowlingText.color = new Color(0, 0, 0, alpha);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(textWaitTime);

        highscoreText.text = "Highscore: " + GetComponent<scoreMaster>().runningTotal;
        alpha = highscoreText.color.a;
        while (alpha < 1)
        {
            alpha += textFadeSpeed;
            highscoreText.color = new Color(0, 0, 0, alpha);
            yield return new WaitForFixedUpdate();
        }
        canReturnToMenu = true;
    }
}
