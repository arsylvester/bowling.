using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderSelector : MonoBehaviour
{
    [SerializeField] Texture ScoreSheet;
    [SerializeField] Texture BallCam;
    [SerializeField] MeshRenderer Screen;

    [SerializeField] bool callToScore;
    [SerializeField] bool callToBall;

    private void Start()
    {
        Screen.sharedMaterial.SetTexture("_CRT_Texture", ScoreSheet);
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
        Screen.sharedMaterial.SetTexture("_CRT_Texture", ScoreSheet);
    }

    public void cameraSwapToBall()
    {
        Screen.sharedMaterial.SetTexture("_CRT_Texture", BallCam);
    }
}
