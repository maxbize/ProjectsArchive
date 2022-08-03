using UnityEngine;
using System.Collections;

public class OldWestSlotsManager : SlotsManager {

    private const int MAX_AMMO = 6; // Should be obvious ;)
    private int ammo = 0; // Used in mini-game
    private int numFreeSpins = 0;

    public override IEnumerator reelsStopped() {
        yield return StartCoroutine(base.reelsStopped());

        if (numFreeSpins > 0) {
            numFreeSpins--;
            startPlay(playBet, playLines, true);
        }
        yield return new WaitForSeconds(0);
    }

    // Get ammo!
    protected override void handleLineBonus(int numSymbols) {
        if (numSymbols < 1) {
            return;
        }
        ammo += OldWestSymbolFactory.getLineBonusSymbol().multipliers[numSymbols - 1];
        ammo = ammo > MAX_AMMO ? MAX_AMMO : ammo;
    }

    // Free spins!
    protected override void handleBoardBonus(int numSymbols) {
        if (numSymbols < 1) {
            return;
        }
        numFreeSpins += OldWestSymbolFactory.getBoardBonusSymbol().multipliers[numSymbols - 1];
    }
}
