using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreMaster : MonoBehaviour {

    public pinMasterScript pin_script;
    public int[, ] score = new int[11, 3]; //scores for each frame including bonus rolls
    public int[] total = new int[11]; //total score for each frame. Used for dispay on HUD
    public string[, ] displayScore = new string[11, 2]; //string representations of roll scores. Used for dispay on HUD
    //public Queue<int[]> bonus = new Queue<int[]>(); //Used to calculate bonus points from strikes or spares
    public ArrayList bonus = new ArrayList (); //i'm going to try using an arraylist I guess
    public TextMesh[] rollText;
    public TextMesh[] totalText;
    public LaneHitbox laneEnd, pinZone;
    public int frame, roll;
    public bool exampleMethodCall;
    public bool inSetup;

    void Start () {

        for (int x = 0; x < 11; x++) { //initialize score array to all zeros
            total[x] = -1;
            for (int y = 0; y < 3; y++) {
                score[x, y] = 0;
            }
        }

        bonus.Capacity = 2;
        inSetup = false;
        frame = 0;
        roll = 0;
    }

    void Update () {
        //ROLL END TRIGGERS//
        if (exampleMethodCall) {
            rollEnd ();
            //printScore();
            exampleMethodCall = false;
        }

        if (pinZone.isTouched) {
            print ("pinZone touched.");
            pinZone.isTouched = false;
            StartCoroutine (timedRollEnd (3f));
        }

        // if(laneEnd.isTouched){
        //     print("laneEnd touched.");
        //     laneEnd.isTouched = false;
        //     StartCoroutine(timedRollEnd(1f));
        // }
    }

    public IEnumerator timedWait (float S) {
        yield return new WaitForSecondsRealtime (S);
    }

    IEnumerator timedRollEnd (float S) {
        yield return new WaitForSecondsRealtime (S);
        if (!inSetup) {
            rollEnd ();
        }
    }

    void rollEnd () {
        inSetup = true;

        //move object to obscure pins from view

        GameObject[] k = pin_script.getKnocked ();
        updateScore (pin_script.getKnocked ()); // this is pointless. move the code from updateScore() to here

        //display score
        printScore ();

        //despawn ball
        GameObject bowler = pinZone.touchedBy;
        bowler.SetActive (false);

        //despawn knocked pins
        foreach (GameObject p in k) {
            if (p != null) {
                p.SetActive (false);
            }
        }

        //reset remaining pins

        //remove pin obscuring object

        //respawn ball in ball return

        StartCoroutine (timedWait (0.6f));
        inSetup = false;
    }

    void printScore () {
        //ROLLS//
        for (int j = 0; j < 10; j++) {
            string t = "";
            for (int k = 0; k < 2; k++) {
                t += displayScore[j, k] + " ";
            }
            rollText[j].text = t;
        }

        //TOTALS//
        for (int j = 0; j < 10; j++) {
            if (total[j] != -1) {
                totalText[j].text = "" + total[j];
            } else {
                totalText[j].text = "";
            }
        }
    }

    void updateScore (GameObject[] knocked) {
        int pinFall = knocked.Length;

        pinFall = pin_script.getKnockedInt (); //TODO: replace this with sothing more streamlined

        print ("Pinfall: " + pinFall);

        if (roll == 0) {
            total[frame] = 0; //set total from -1 to 0
            score[frame, roll] = pinFall;
            total[frame] += pinFall;

            if (bonus.Count != 0) { //if there's an active bonus
                for (int y = 0; y < bonus.Count; y++) { //I am in pain
                    int[] o = (int[]) bonus[y];
                    if (o[1] == 0) {
                        bonus.Remove (o);
                        continue;
                    } else {
                        score[o[0], 3 - o[1]] = pinFall;
                        total[o[0]] += pinFall;

                        bonus.Remove (o);
                        o[1] -= 1;
                        bonus.Insert (y, o);
                    }
                }
            }

            if (pinFall == 10) { //strike
                //adjust displayScore
                displayScore[frame, roll] = "X";
                displayScore[frame, roll + 1] = " ";

                //enqueue 2 bonus rolls
                int[] b = new int[2];
                b[0] = frame;
                b[1] = 2; //amount of bonus rolls
                bonus.Add (b);

                //skip to next frame
                advanceFrame ();
            } else {
                //adjust displayScore
                displayScore[frame, roll] = "" + pinFall;
                roll++;
            }
        } else if (roll == 1) {
            score[frame, roll] = pinFall;
            total[frame] += pinFall;

            if (bonus.Count != 0) { //if there's an active bonus
                for (int y = 0; y < bonus.Count; y++) { //I am in pain
                    int[] o = (int[]) bonus[y];
                    if (o[1] == 0) {
                        bonus.Remove (o);
                        continue;
                    } else {
                        score[o[0], 3 - o[1]] = pinFall;
                        total[o[0]] += pinFall;

                        bonus.Remove (o);
                        o[1] -= 1;
                        bonus.Insert (y, o);
                    }
                }
            }

            if (pinFall == pin_script.pins.Length) { //spare
                displayScore[frame, roll] = "/";

                //enqueue 1 bonus roll
                int[] b = new int[2];
                b[0] = frame;
                b[1] = 1;
                bonus.Add (b);
            } else {
                displayScore[frame, roll] = "" + pinFall;
            }

            //continue to next frame
            advanceFrame ();
        } else {
            print ("HOW?!");
        }
    }

    void advanceFrame () {
        frame++;
        roll = 0;

        foreach (GameObject pin in pin_script.pins) {
            pin.SetActive (true);
        }
    }
}