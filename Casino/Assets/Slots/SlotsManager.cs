using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlotsManager : MonoBehaviour
{


    public Transform boardPrefab;
    private Transform boardObj;
    public int boardLen = 999; // Hard-coded way over max for easy comparison
    public int numVisibleRows = 3; // number of rows visible
    private bool playingNow = false;

    protected int playBet, playLines;

    // Lines is a map of x,y trail to follow for lines. 0,0 is bottom left
    private List<List<int>> lines = new List<List<int>>() {
        new List<int>() {1,1,1,1,1},
        new List<int>() {0,0,0,0,0},
        new List<int>() {2,2,2,2,2},
        new List<int>() {0,1,2,1,0},
        new List<int>() {2,1,0,1,2},
        new List<int>() {0,0,1,2,2},
        new List<int>() {2,2,1,0,0},
        new List<int>() {1,2,2,2,1},
        new List<int>() {1,0,0,0,1}
    };

    // un-shuffled, flattened table
    List<List<Symbol>> reels = new List<List<Symbol>>();

    public OldWestSymbolFactory symbolFactory;
    public Player player;
    public AudioManager audioManager;

    // Use this for initialization
    void Start() {
        reels = symbolFactory.createReels();
        // Find the shortest reel so that we know how far to roll
        foreach (List<Symbol> reel in reels) {
            if (reel.Count < boardLen) {
                boardLen = reel.Count;
            }
        }
    }

    void Update() {

    }

    public void startPlay(int bet, int numLines, bool freePlay) {
        if (!playingNow) {
            if (!freePlay && !player.requestMoney(bet * numLines)) {
                return; // Player doesn't have enough money
            }
            audioManager.playClip(AudioManager.clips.startPlay);
            playBet = bet;
            playLines = numLines;
            shuffleReels();
            playingNow = true;
            if (boardObj != null) {
                Destroy(boardObj.gameObject);
            }
            boardObj = (Transform)GameObject.Instantiate(boardPrefab);
            boardObj.GetComponent<Board>().Init(reels, this);
        }
    }

    // Use this to notify the slot manager that the reels have stopped
    public virtual IEnumerator reelsStopped() {
        int moneyWon = calculateWinnings(playLines, playBet);
        if (moneyWon > 0) {
            player.acceptMoney(calculateWinnings(playLines, playBet));
            audioManager.playClip(AudioManager.clips.win);
            //yield return new WaitForSeconds(0.5f);
        }
        // Wait half a second before allowing another action
        yield return new WaitForSeconds(0.5f);
        playingNow = false;
    }



    private void shuffleReels() {
        foreach (List<Symbol> reel in reels) {
            reel.Shuffle();
        }
    }

    /*
     * For each line, symbols must line up from left to right, starting in column 0
     */
    private int calculateWinnings(int numLines, int bet) {
        int numBoardBonus = 0;
        int winnings = 0;
        for (int lineNum = 0; lineNum < numLines; lineNum++) {
            int numLineBonus = 0;
            int numBaseSymbol = 1;
            bool keepTallying = true;
            int baseRow = boardLen - numVisibleRows;
            Symbol baseSymbol = reels[0][baseRow + lines[lineNum][0]];
            /*
             * If the first symbol is a wildcard find the first non-wildcard symbol
             */
            if (baseSymbol.winRules == Symbol.WinRules.wildcard) {
                for (int col = 1; col < reels.Count; col++) {
                    Symbol symbol = reels[col][baseRow + lines[lineNum][col]];
                    if (symbol.winRules == Symbol.WinRules.wildcard) {
                        continue;
                    }
                    else if (symbol.winRules == Symbol.WinRules.consecutiveLeftRight) {
                        baseSymbol = symbol;
                    }
                    break;
                }
                // All wildcards! Keep the original symbol
            }
            for (int col = 1; col < reels.Count; col++) {
                Symbol symbol = reels[col][baseRow + lines[lineNum][col]];
                if (keepTallying &&
                    (symbol.symbolType == baseSymbol.symbolType || symbol.winRules == Symbol.WinRules.wildcard)) {
                    numBaseSymbol++;
                }
                else {
                    keepTallying = false;
                }
                if (symbol.winRules == Symbol.WinRules.anywhereBoard
                    && lineNum < 3) { // Don't double register board bonuses
                    numBoardBonus++;
                }
                else if (symbol.winRules == Symbol.WinRules.anywhereLine) {
                    numLineBonus++;
                }
            }
            handleLineBonus(numLineBonus);

            winnings += baseSymbol.multipliers[numBaseSymbol - 1] * bet; // 0 indexed
        }

        handleBoardBonus(numBoardBonus);

        if (winnings > 0) {
            //Debug.Log("Won $" + winnings + " from roll");
        }
        return winnings;
    }

    // Used for debugging / balancing. Just calculates wins and prints statistics
    public void simulatePlay(int numPlays, int bet, int lines) {
        Debug.Log("Simulating " + numPlays + " plays.");
        int numWins = 0;
        int moneyFromBet = 0;
        int money = bet * lines * numPlays;
        for (int playNum = 0; playNum < numPlays; playNum++) {
            shuffleReels();
            moneyFromBet = calculateWinnings(lines, bet);
            money += moneyFromBet;
            money -= lines * bet;
            if (moneyFromBet > 0) {
                numWins++;
            }
        }
        Debug.Log("Finished with " + money + " (" + Mathf.RoundToInt(money * 100 / ((float)bet * lines * numPlays)) + "%) after play. Hit %" + (float)numWins / numPlays * 100);
    }

    protected virtual void handleLineBonus(int numSymbols) {
        // Implemented in base class
    }

    protected virtual void handleBoardBonus(int numSymbols) {
        // Implemented in base class
    }
}
