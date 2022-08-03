using UnityEngine;
using System.Collections.Generic;

public struct Symbol {

    public enum Type { WS, LM, BU, BO, LH, TU, CL, SG, SF, LO, LT };
    public Type symbolType;

    // For now assume there's only one anywhereLine and one anywhereBoard symbol per set
    public enum WinRules { consecutiveLeftRight, anywhereLine, anywhereBoard, wildcard };
    public WinRules winRules;    

    public Sprite spriteReference;

    // Number of occurances in each reel, left to right
    public List<int> occurances;

    // Multiplier per number of occurances. e.g. {0, 0, 5, 30, 50} meaning 50x bonus for 5 occurances
    public List<int> multipliers;

    public Symbol(Type symbolType, Sprite sprite, List<int> occurances, WinRules winRules, List<int> multipliers) {
        this.symbolType = symbolType;
        this.spriteReference = sprite;
        this.occurances = occurances;
        this.winRules = winRules;
        this.multipliers = multipliers;
    }
}
