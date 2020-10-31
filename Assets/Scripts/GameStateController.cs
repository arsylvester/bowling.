using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameStateController : MonoBehaviour
{
    private static int currentFrame = 0;
    private bool canReturnToMenu = false;
    private float currentVolume = 50f;
    private float currentVoicePitch = 100;
    public UnityEvent m_NewFrame = new UnityEvent();
    public static GameStateController _instance;

    
    [SerializeField] LightsController lightControl;
    [SerializeField] RawImage white;
    [SerializeField] Text bowlingText;
    [SerializeField] Text highscoreText;
    [SerializeField] AkAudioListener akListener;
    [SerializeField] float fadeSpeed = .05f;
    [SerializeField] float textFadeSpeed = .05f;
    [SerializeField] float textWaitTime = 5f;
    [SerializeField] GameObject postProcessingMain, postProcessingFR;
    [SerializeField] float pitchChange = 5f;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        if (m_NewFrame == null)
            m_NewFrame = new UnityEvent();

        AkSoundEngine.SetRTPCValue("VoicePitch", currentVoicePitch);
        m_NewFrame.AddListener(changeVoicePitch);
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

    private void changeVoicePitch()
    {
        currentVoicePitch -= pitchChange;
        AkSoundEngine.SetRTPCValue("VoicePitch", currentVoicePitch);
    }

    public void EndGame()
    {
        white.gameObject.SetActive(true);
        StartCoroutine(fadeInEnd());
    }

    public IEnumerator finalRoll(){
        //flicker lights
        yield return StartCoroutine(lightControl.FlashAllLights(3f));

        //yield return new WaitForSecondsRealtime(6f);
        
        //turn off lights
        foreach (Lights l in lightControl.lights)
        {
            l.SetLight(false);
        }
        StartCoroutine(lightControl.FlickerMonitor(0f, 0, 0f, false));

        yield return new WaitForSecondsRealtime(2f);

        //make everything red
        postProcessingFR.SetActive(true);
        postProcessingMain.SetActive(false);
        
        yield return new WaitForSecondsRealtime(4f);

        //PLAY DO NOT

        //turn on lights
        StartCoroutine(lightControl.FlickerMonitor(0f, 0, 0f, true));
        foreach (Lights l in lightControl.lights)
        {
            l.SetLight(true);
        }
        
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
        AkSoundEngine.PostEvent("FinalSoundsStop", gameObject);

        bowlingText.color = new Color(0, 0, 0, 1);

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
