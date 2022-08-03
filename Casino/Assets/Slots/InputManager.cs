using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputManager : MonoBehaviour {

    // Not so sure about limiting the bets to an array
    // Also not so sure about controlling these variables here
    private int[] allowableBets = new int[] { 1, 5, 10 };
    private int maxLines = 9;
    private int minLines = 1;

    private int betIndex = 0;
    private int numLines = 1;

    public Text betText;
    public Text linesText;
    public Text moneyText;

    public SlotsManager slotsManager;


	// Use this for initialization
	void Start () {
        updateBetText();
        updateLinesText();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void changeBet(bool increase) {
        if (increase && betIndex < allowableBets.Length - 1) {
            betIndex++;
        }
        if (!increase && betIndex > 0) {
            betIndex--;
        }
        updateBetText();
    }

    public void changeLines(bool increase) {
        if (increase && numLines < maxLines) {
            numLines++;
        }
        if (!increase && numLines > minLines) {
            numLines--;
        }
        updateLinesText();
    }

    private void updateBetText() {
        betText.text = "Bet: " + allowableBets[betIndex];
    }

    private void updateLinesText() {
        linesText.text = "Lines: " + numLines;
    }

    public void play() {
        slotsManager.startPlay(allowableBets[betIndex], numLines, false);
    }

    public void simulatePlay(int numPlays) {
        slotsManager.simulatePlay(numPlays, allowableBets[betIndex], numLines);
    }

    public void updateMoneyText(int amount) {
        moneyText.text = "Money: " + amount;
    }
}
