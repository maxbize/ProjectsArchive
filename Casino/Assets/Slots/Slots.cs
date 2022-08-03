using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Slots : MonoBehaviour
{

    private enum symbol { WS, LM, BU, BO, LH, TU, CL, SG, SF, LO, LT };

    private static float symbolTextureWidth = 1;
    private static float symbolTextureHeight = 1;

    private Dictionary<symbol, List<int>> reelMap = new Dictionary<symbol, List<int>>() {
        {symbol.WS, new List<int> {2, 2, 1, 4, 2}},
        {symbol.LM, new List<int> {4, 3, 3, 3, 4}},
        {symbol.BU, new List<int> {3, 4, 3, 8, 5}},
        {symbol.BO, new List<int> {4, 3, 4, 3, 4}},
        {symbol.LH, new List<int> {3, 4, 6, 3, 7}},
        {symbol.TU, new List<int> {4, 3, 6, 6, 6}},
        {symbol.CL, new List<int> {10, 8, 3, 4, 7}},
        {symbol.SG, new List<int> {3, 9, 4, 10, 5}},
        {symbol.SF, new List<int> {10, 3, 10, 7, 8}},
        {symbol.LO, new List<int> {2, 5, 6, 0, 0}},
        {symbol.LT, new List<int> {2, 2, 2, 2, 2}},
    };
    
    // un-shuffled, flattened table
    List<List<symbol>> reels = new List<List<symbol>>();


    // Use this for initialization
    void Start() {
        for (int i = 0; i < reelMap[symbol.WS].Count; i++) {
            reels.Add(new List<symbol>());
        }

        // Generate the raw reels based on the reelMap (must be rectangular)
        foreach (symbol symb in reelMap.Keys) {
            for (int col = 0; col < reelMap[symb].Count; col++) {
                for (int i = 0; i < reelMap[symb][col]; i++) {
                    reels[col].Add(symb);
                }
            }
        }
        setupVisualReels();
    }

    // Update is called once per frame
    void Update() {

    }

    // Start with a basic method - just render all reels at once then lower them
    private void setupVisualReels() {
        for (int col = 0; col < reels.Count; col++) {
            for (int row = 0; row < reels[col].Count; row++) {
                Vector2 pos = new Vector2(symbolTextureWidth * (col - reels.Count / 2.0f),
                    symbolTextureHeight * (row - reels.Count / 2.0f));
                Debug.DrawLine(pos, new Vector3(pos.x, pos.y, 5), Color.red, 30);
            }
        }
    }

    private void shuffleReels() {
        foreach (List<symbol> reel in reels) {
            reel.Shuffle();
        }
    }
}
