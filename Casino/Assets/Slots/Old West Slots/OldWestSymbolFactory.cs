using UnityEngine;
using System.Collections.Generic;

public class OldWestSymbolFactory : SymbolFactory
{
    protected override void registerAllSymbols() {
        // Wildcard - match anything but bonus
        registerSymbol(new Symbol(
            Symbol.Type.WS,
            wsSprite,
            new List<int> { 2, 2, 1, 4, 2 },
            Symbol.WinRules.wildcard,
            new List<int>() { 0, 30, 825, 6800, 100000 }
            ));
        registerSymbol(new Symbol(
            Symbol.Type.LM,
            lmSprite,
            new List<int> { 4, 3, 3, 3, 4 },
            Symbol.WinRules.consecutiveLeftRight,
            new List<int> { 0, 0, 30, 130, 720 }
            ));
        registerSymbol(new Symbol(
            Symbol.Type.BU,
            buSprite,
            new List<int> { 3, 4, 3, 8, 5 },
            Symbol.WinRules.consecutiveLeftRight,
            new List<int> { 0, 0, 27, 75, 360 }
            ));
        registerSymbol(new Symbol(
            Symbol.Type.BO,
            boSprite,
            new List<int> { 4, 3, 4, 3, 4 },
            Symbol.WinRules.consecutiveLeftRight,
            new List<int> { 0, 0, 22, 100, 575 }
            ));
        registerSymbol(new Symbol(
            Symbol.Type.LH,
            lhSprite,
            new List<int> { 3, 4, 6, 3, 7 },
            Symbol.WinRules.consecutiveLeftRight,
            new List<int> { 0, 0, 16, 75, 275 }
            ));
        registerSymbol(new Symbol(
            Symbol.Type.TU,
            tuSprite,
            new List<int> { 4, 3, 6, 6, 6 },
            Symbol.WinRules.consecutiveLeftRight,
            new List<int> { 0, 0, 15, 50, 215 }
            ));
        registerSymbol(new Symbol(
            Symbol.Type.CL,
            clSprite,
            new List<int> { 10, 8, 3, 4, 7 },
            Symbol.WinRules.consecutiveLeftRight,
            new List<int> { 0, 0, 7, 30, 105 }
            ));
        registerSymbol(new Symbol(
            Symbol.Type.SG,
            sgSprite,
            new List<int> { 3, 9, 4, 10, 5 },
            Symbol.WinRules.consecutiveLeftRight,
            new List<int> { 0, 0, 12, 30, 135 }
            ));
        registerSymbol(new Symbol(
            Symbol.Type.SF,
            sfSprite,
            new List<int> { 10, 3, 10, 7, 8 },
            Symbol.WinRules.consecutiveLeftRight,
            new List<int> { 0, 0, 3, 15, 50 }
            ));
        // This symbol will be for the line bonus (ammo)
        registerSymbol(new Symbol(
            Symbol.Type.LO,
            loSprite,
            new List<int> { 2, 4, 4, 2, 2 },
            Symbol.WinRules.anywhereLine,
            new List<int> { 0, 1, 2, 4, 6 }
            ));
        // This symbol will be for the board bonus (free spins)
        registerSymbol(new Symbol(
            Symbol.Type.LT,
            ltSprite,
            new List<int> { 2, 2, 2, 2, 2 },
            Symbol.WinRules.anywhereBoard,
            new List<int> { 1, 1, 5, 10, 15, 20, 25, 30, 50, 100 }
            ));
    }
}
