using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreMaster : MonoBehaviour {
    
    public pinMasterScript pin_script;
    public int[,] score = new int[11,3]; //scores for each frame including bonus rolls
    public int[] total = new int[11]; //total score for each frame. Used for dispay on HUD
    public string[,] displayScore = new string[11,2]; //string representations of roll scores. Used for dispay on HUD
    //public Queue<int[]> bonus = new Queue<int[]>(); //Used to calculate bonus points from strikes or spares
    public ArrayList bonus = new ArrayList(); //i'm going to try using an arraylist I guess
    public TextMesh hud1;
    public TextMesh hud2;
    public int frame;
    public int roll;
    public bool exampleMethodCall;

    void Start () {
        
        for (int x = 0; x < 11; x++){ //initialize score array to all zeros
            total[x] = 0;
            for (int y = 0; y < 3; y++){
                score[x,y] = 0;
            }
        }

        bonus.Capacity = 2;

        frame = 0;
        roll = 0;
    }

    
    void Update () {
        //ROLL END TRIGGERS//
        
        //if ball has touched a pin (this needs the script to be in the ball)
        if (exampleMethodCall) {
            rollEnd();
            printScore();
            exampleMethodCall = false;
        }
    }

    void rollEnd(){
        // GameObject[] knocked = new GameObject[];
        // knocked = pin_script.getKnocked();

        //move object to obscure pins from view

        updateScore(pin_script.getKnocked()); // this is pointless. move the code from updateScore() to here

        //display score

        //despawn ball

        //despawn knocked pins

        //reset remaining pins

        //remove pin obscuring object

        //respawn ball in ball return
    }

    void printScore() {
        string output = "";
        for (int j=0; j<11; j++){
            output += "\t";
            for (int k=0; k<2; k++){
                output += displayScore[j,k] + " ";
            }
        }
        hud1.text = output;

        output = "";
        for (int j=0; j<11; j++){
            output += "\t";
            if (total[j] != 0){
                output += total[j];
            }
        }
        hud2.text = output;
    }

    void updateScore(GameObject[] knocked){
        int pinFall = knocked.Length;

        pinFall = pin_script.getKnockedInt(); //TODO: replace this with sothing more streamlined

        print("Pinfall: " + pinFall);

        if (roll == 0) {
            score[frame, roll] = pinFall;
            total[frame] += pinFall;
            
            if (bonus.Count != 0){ //if there's an active bonus
                /*
                foreach (int[] o in bonus)
                {
                    if (o[1] == 0){
                        bonus.Remove(o);
                        continue;
                    }
                    else{
                        score[o[0], 3-o[1]] = pinFall;
                        total[o[0]] += pinFall;

                        int[] ImGoingToPunchAHoleInMyWall = o;

                        int id = bonus.IndexOf(o);
                        ImGoingToPunchAHoleInMyWall[1] -= 1;
                        bonus.Remove(id);
                        bonus.Add(ImGoingToPunchAHoleInMyWall);
                        
                        //if it doesn't change, then I'll need to figure something out
                    }

                }
                */
                for (int y=0; y<bonus.Count; y++){ //I am in pain
                    int[] o = (int[])bonus[y];
                    if (o[1] == 0){
                        bonus.Remove(o);
                        continue;
                    }
                    else{
                        score[o[0], 3-o[1]] = pinFall;
                        total[o[0]] += pinFall;

                        bonus.Remove(o);
                        o[1] -= 1;
                        bonus.Insert(y,o);
                    }
                }
            }
            
            if (pinFall == 10){ //strike
                //adjust displayScore
                displayScore[frame, roll] = "X";
                displayScore[frame, roll+1] = " ";

                //enqueue 2 bonus rolls
                int[] b = new int[2];
                b[0] = frame;
                b[1] = 2; //amount of bonus rolls 
                //bonus.Enqueue(b);
                bonus.Add(b);
                
                //skip to next frame
                frame++;
                roll = 0;
            }
            else {
                //adjust displayScore
                displayScore[frame, roll] = "" + pinFall;
                roll++;
            }
        }
        else if (roll == 1) {
            score[frame, roll] = pinFall;
            total[frame] += pinFall;

            if (bonus.Count != 0){ //if there's an active bonus
                foreach (int[] o in bonus)
                {
                    if (o[1] == 0){
                        bonus.Remove(o);
                        continue;
                    }
                    else{
                        score[o[0], 3-o[1]] = pinFall;
                        total[o[0]] += pinFall;
                        o[1] -= 1;
                        //if it doesn't change, then I'll need to figure something out
                    }
                }
            }
            
            if (pinFall == pin_script.pins.Length){ //spare
                displayScore[frame, roll] = "/";

                //enqueue 1 bonus roll
                int[] b = new int[2];
                b[0] = frame;
                b[1] = 1;
                bonus.Add(b);
            }
            else {
                displayScore[frame, roll] = "" + pinFall;
            }

            //continue to next frame
            frame++;
            roll = 0;
        }
        else {
            print("HOW?!");
        }
    }
}