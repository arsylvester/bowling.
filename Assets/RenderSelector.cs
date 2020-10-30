using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderSelector : MonoBehaviour
{
    [SerializeField] Texture ScoreSheet;
    [SerializeField] Texture BallCam;
    [SerializeField] Texture blackScreen;
    [SerializeField] MeshRenderer Screen;
    [SerializeField] Texture[] flickerArray;
    [SerializeField] float flickerFrequencyBase;
    [SerializeField] float betweenFlickerTime = .3f;
    [SerializeField] float randomFlickerTime = .1f;
    private float flickerFrequency;

    [SerializeField] bool callToScore;
    [SerializeField] bool callToBall;

    private void Start()
    {
        Screen.sharedMaterial.SetTexture("_CRT_Texture", ScoreSheet);
        flickerFrequency = flickerFrequencyBase;
        StartCoroutine(flickerRandom());
    }

    private void Update()
    {
        if(callToScore)
        {
            cameraSwapToScore();
            callToScore = false;
        }
        else if(callToBall)
        {
            cameraSwapToBall();
            callToBall = false;
        }
    }

    public void cameraSwap()
    {
        if(Screen.sharedMaterial.GetTexture("_CRT_Texture") == ScoreSheet)
        {
            Screen.sharedMaterial.SetTexture("_CRT_Texture", BallCam);
        }
        else
        {
            Screen.sharedMaterial.SetTexture("_CRT_Texture", ScoreSheet);
        }
    }

    public void cameraSwapToScore()
    {
        StartCoroutine(flickerBlack(ScoreSheet));
    }

    public void cameraSwapToBall()
    {
        StartCoroutine(flickerBlack(BallCam));
    }

    public IEnumerator flickerBlack(Texture textureToSwapTo)
    {
        Screen.sharedMaterial.SetTexture("_CRT_Texture", blackScreen);
        yield return new WaitForSeconds(betweenFlickerTime);
        Screen.sharedMaterial.SetTexture("_CRT_Texture", textureToSwapTo);
    }

    public IEnumerator flickerRandom()
    {
        while (true)
        {
            if (Random.value <= flickerFrequency)
            {
                Texture currentTexture = Screen.sharedMaterial.GetTexture("_CRT_Texture");
                Screen.sharedMaterial.SetTexture("_CRT_Texture", flickerArray[(int)Random.Range(0, flickerArray.Length)]);
                yield return new WaitForSeconds(randomFlickerTime);
                Screen.sharedMaterial.SetTexture("_CRT_Texture", currentTexture);
            }
            yield return new WaitForSeconds(1);
        }
    }
}
