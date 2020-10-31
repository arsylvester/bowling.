using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreMaster : MonoBehaviour {

    public pinMasterScript pin_script;
    [SerializeField] RenderSelector screen;
    [SerializeField] GameObject[] knocked_pins;
    [SerializeField] int[, ] score = new int[11, 3]; //scores for each frame including bonus rolls
    [SerializeField] int[] total = new int[11]; //total score for each frame. Used for dispay on HUD
    [SerializeField] string[, ] displayScore = new string[11, 2]; //string representations of roll scores. Used for dispay on CRT
    [SerializeField] ArrayList bonus = new ArrayList (); //Used to calculate bonus points from strikes or spares
    public TextMesh[] rollText;
    public TextMesh[] totalText;
    public LaneHitbox laneEnd, pinZone;
    public BallDispenser dispenser;
    public int frame, roll, runningTotal;
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
        runningTotal = 0;

        printScore();
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
            StartCoroutine (timedRollEnd (4f));
            //pinZone.touchedBy = null;
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
            pinZone.touchedBy = null;
        }
    }

    void rollEnd () {
        inSetup = true;

        //reset crt screen to display score
        screen.cameraSwapToScore();

        //move object to obscure pins from view

        knocked_pins = pin_script.getKnocked ();
        
        updateScore (pin_script.getKnocked ()); // this is pointless. move the code from updateScore() to here

        //display score
        printScore ();
        
        //despawn ball
        GameObject bowler = pinZone.touchedBy;
        if(bowler)
            bowler.SetActive (false);

        //reset remaining pins
        foreach (GameObject pin in pin_script.pins) {
            if (pin != null && pin.activeSelf) {
                pin.transform.rotation = pin.GetComponent<pinScript> ().defaultRot;
                pin.transform.position = pin.GetComponent<pinScript> ().defaultPos;
                pin.GetComponent<Rigidbody> ().velocity = Vector3.zero;
                pin.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
            }
        }

        //despawn knocked pins
        foreach (GameObject p in knocked_pins) {
            if (p != null) {
                p.SetActive (false);
            }
        }







        //remove pin obscuring object



        //respawn ball in ball return

        if(bowler) {

            bowler.GetComponent<Rigidbody>().useGravity = false;

            bowler.GetComponent<Rigidbody>().velocity = Vector3.zero;

            dispenser.MoveBall(bowler);

            bowler.SetActive(true);

        }

        

        //remove pin obscuring object

        //Final roll actions
        if (frame == 10 && displayScore[9,1] == "/"){
            GetComponent<GameStateController> ().finalRoll();
        }
        else if (frame == 10 && roll == 1 && displayScore[9,0] == "X") {
            GetComponent<GameStateController> ().finalRoll();
        }

        //respawn ball in ball return
        bowler.GetComponent<Rigidbody>().useGravity = false;
        bowler.GetComponent<Rigidbody>().velocity = Vector3.zero;
        dispenser.MoveBall(bowler);
        bowler.SetActive (true);

        StartCoroutine (timedWait (0.6f));
        inSetup = false;
    }

    void printScore () {
        //ROLLS//
        for (int j = 0; j < 11; j++) {
            string t = "";
            for (int k = 0; k < 2; k++) {
                t += displayScore[j, k];
                if (j != 9){
                    t += " ";
                }
            }
            if (j == 10) { //frame 11
                rollText[9].text += t;
            } else {
                rollText[j].text = t;
            }
        }

        //TOTALS//
        for (int j = 0; j < 11; j++) {
            if (j == 10) {
                if (total[j] != -1) {
                    int sum = total[9] + total[10];
                    totalText[9].text = "" + sum;
                }
            } else {
                if (total[j] != -1) {
                    totalText[j].text = "" + total[j];
                } else {
                    totalText[j].text = "";
                }
            }
        }

        runningTotal = 0;
        foreach (int frameScore in total)
        {
            if (frameScore != -1){
                runningTotal += frameScore;
            }
        }

        totalText[9].text = "" + runningTotal;
    }

    void applyBonus (int pinFall){
         if (bonus.Count != 0) { //if there's an active bonus
                print("amount of active bonuses: " + bonus.Count);
                List<int> toRemove = new List<int>();
                ArrayList temp = new ArrayList ();
                for (int y = 0; y < bonus.Count; y++) { //I am in pain
                    int[] o = (int[]) bonus[y];
                    print("bonus for frame " + o[0] + ": " + o[1]);
                    if (o[1] != 0) {
                        score[o[0], 3 - o[1]] = pinFall; //score[1] and score[2] need to be modified
                        total[o[0]] += pinFall;

                        o[1] -= 1;
                        temp.Add(o);
                    }
                }
                bonus = temp;
            }
    }

    void updateScore (GameObject[] knocked) {
        int pinFall = knocked.Length;

        pinFall = pin_script.getKnockedInt (); //TODO: replace this with sothing more streamlined

        //print ("Pinfall: " + pinFall);

        if (roll == 0) {
            total[frame] = 0; //set total from -1 to 0
            score[frame, roll] = pinFall;
            total[frame] += pinFall;

            applyBonus(pinFall);

            if (pinFall == 10) { //strike
                //adjust displayScore
                displayScore[frame, roll] = "X";
                displayScore[frame, roll + 1] = " ";

                //enqueue 2 bonus rolls
                int[] b = new int[2];
                b[0] = frame;
                b[1] = 2; //amount of bonus rolls
                bonus.Add (b);

                if (frame != 10) {
                    //skip to next frame
                    advanceFrame ();
                } else {
                    if (displayScore[9, 1] == "/") { //Spare in first 2 rolls of f10 and completed bonus roll
                        //GAME OVER
                        gameOver();
                        return;
                    } else { //f10 first roll was a strike
                        print(displayScore[9, 1] + " != / . this means that you got a strike.");
                        resetPins ();
                        roll++;
                    }
                }

            } else {
                //adjust displayScore
                displayScore[frame, roll] = "" + pinFall;
                roll++;
            }
        } else if (roll == 1) {
            score[frame, roll] = pinFall;
            total[frame] += pinFall;

            // if (bonus.Count != 0) { //if there's an active bonus
            //     print("amount of active bonuses: " + bonus.Count);
            //     for (int y = 0; y < bonus.Count; y++) { //I am in pain
            //         int[] o = (int[]) bonus[y];
            //         print("bonus for frame " + o[0] + ": " + o[1]);
            //         if (o[1] == 0) {
            //             bonus.Remove (o);
            //             continue;
            //         } else {
            //             score[o[0], 3 - o[1]] = pinFall; //score[1] and score[2] need to be modified
            //             total[o[0]] += pinFall;

            //             bonus.Remove (o);
            //             o[1] -= 1;
            //             bonus.Insert (y, o);
            //         }
            //     }
            // }

            applyBonus(pinFall);

            if (pinFall == 10 && frame == 10 && displayScore[10, 0] == "X") { //frame 10 3rd strike
                displayScore[frame, roll] = "X";
                //GAME OVER
                //TRUE ENDING
                gameOver();
                return;
            } else if (total[frame] == 10) { //spare
                displayScore[frame, roll] = "/";

                int[] b = new int[2];
                b[0] = frame;
                b[1] = 1;
                bonus.Add (b);
            } else {
                displayScore[frame, roll] = "" + pinFall;
                if (frame == 9) { //No Mark on frame 10
                    print("no mark on frame 10. Game over.");
                    //GAME OVER
                    gameOver();
                    return;
                }
            }

            if (frame == 10) { //Got a strike on f10-roll1 and completed both bonus rolls
                print("frame 11 complete. Game over.");
                //GAME OVER
                gameOver();
                return;
            } else {
                //continue to next frame   
                advanceFrame ();
            }
        } else {
            print ("HOW?!");
        }
    }

    void advanceFrame () {
        //print("Frame advanced.");
        frame++;
        roll = 0;
        resetPins ();
        for (int z = 0; z < 10; z++){
            knocked_pins[z] = null;
        }
        GetComponent<GameStateController> ().NewFrame (frame);
    }

    void resetPins () {
        foreach (GameObject pin in pin_script.pins) {
            pin.SetActive (true);
            //print(pin);
            pin.GetComponent<Rigidbody> ().velocity = Vector3.zero;
            pin.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
            pin.transform.rotation = pin.GetComponent<pinScript> ().defaultRot;
            pin.transform.position = pin.GetComponent<pinScript> ().defaultPos;
        }
        for (int z = 0; z < knocked_pins.Length; z++){
            if (knocked_pins[z] != null)
                knocked_pins[z] = null;
        }
    }

    public void skipToFrameTen (){
        pin_script.getKnockedInt();
        
        for (int x = 0; x < 10; x++) {
            total[x] = 6;
            for (int y = 0; y < 2; y++) {
                score[x, y] = 6;
                displayScore[x, y] = "6";
            }
        }

        displayScore[9, 0] = "";
        displayScore[9, 1] = "";

        frame = 9;
        roll = 0;
        printScore();
        resetPins();
    }

    void gameOver() {
        print("GAME OVER");

        //lower thing to block pins from view
        
        BallController[] ballControllerBalls = FindObjectsOfType<BallController>();
        foreach (BallController ball in ballControllerBalls)
        {
            ball.gameObject.SetActive(false);
        }

        foreach (GameObject pin in pin_script.pins) {
            pin.SetActive (false);
        }

        //perform spooky actions
        GameStateController._instance.EndGame();
    }
}