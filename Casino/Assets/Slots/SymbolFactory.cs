using UnityEngine;
using System.Collections.Generic;

public class SymbolFactory : MonoBehaviour
{
    public Transform spritePrefab;

    private List<Symbol> symbols = new List<Symbol>();

    // Being lazy and defining all the sprite references manually through Unity UI
    public Sprite wsSprite;
    public Sprite lmSprite;
    public Sprite buSprite;
    public Sprite boSprite;
    public Sprite lhSprite;
    public Sprite tuSprite;
    public Sprite clSprite;
    public Sprite sgSprite;
    public Sprite sfSprite;
    public Sprite loSprite;
    public Sprite ltSprite;

    private static Symbol lineBonusSymbol, boardBonusSymbol;

    protected void registerSymbol(Symbol symbol) {
        symbols.Add(symbol);
        if (symbol.winRules == Symbol.WinRules.anywhereLine) {
            lineBonusSymbol = symbol;
        }
        if (symbol.winRules == Symbol.WinRules.anywhereBoard) {
            boardBonusSymbol = symbol;
        }
    }

    protected virtual void registerAllSymbols() {
        Debug.Log("ERROR: registerAllSymbols called on base class!");
    } 

    public Transform createSymbolObj(Symbol symbol, Vector2 pos) {
        Transform newSymbol = (Transform)GameObject.Instantiate(spritePrefab,
            new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        newSymbol.GetComponent<SpriteRenderer>().sprite = symbol.spriteReference;
        return newSymbol;
    }

    public List<List<Symbol>> createReels() {
        if (symbols.Count == 0) {
            registerAllSymbols();
        }
        List<List<Symbol>> reels = new List<List<Symbol>>();
        for (int col = 0; col < symbols[0].occurances.Count; col++) {
            for (int i = 0; i < symbols.Count; i++) {
                if (i == 0) {
                    reels.Add(new List<Symbol>());
                }
                for (int j = 0; j < symbols[i].occurances[col]; j++) {
                    reels[col].Add(symbols[i]);
                }
            }
        }
        return reels;
    }

    public static Symbol getLineBonusSymbol() {
        return lineBonusSymbol;
    }

    public static Symbol getBoardBonusSymbol() {
        return boardBonusSymbol;
    }
}
