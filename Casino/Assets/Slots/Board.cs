using UnityEngine;
using System.Collections.Generic;


// Board is in charge of maintaining the visual goodies of the slots play area
public class Board : MonoBehaviour {

    // Being lazy again. Defined in editor
    public Transform[] reelObjs = new Transform[5];

    // TODO: Remove magic numbers and get texture bounds
    private static float symbolTextureWidth = 2;
    private static float symbolTextureHeight = 2;

    // Any references to slots manager that we need
    private SlotsManager slotsManager;

	// Use this for initialization
	void Start () {
	    
    }



	// Update is called once per frame
	void Update () {
        if (slotsManager != null) {
            for (int col = 0; col < reelObjs.Length; col++) {
                if (reelObjs[col].transform.position.y < -(slotsManager.boardLen - slotsManager.numVisibleRows) * symbolTextureHeight) {
                    reelObjs[col].transform.position = new Vector2(reelObjs[col].transform.position.x,
                        -(slotsManager.boardLen - slotsManager.numVisibleRows) * symbolTextureHeight);
                    reelObjs[col].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    if (col == reelObjs.Length - 1) {
                        StartCoroutine(slotsManager.reelsStopped());
                    }
                }
            }
        }
	}

    public void Init(List<List<Symbol>> reels, SlotsManager slotsManager) {
        this.slotsManager = slotsManager;

        // Find the shortest reel so that we know how far to roll
        foreach (List<Symbol> reel in reels) {
            if (reel.Count < slotsManager.boardLen) {
                slotsManager.boardLen = reel.Count;
            }
        }

        for (int col = 0; col < reels.Count; col++) {
            for (int row = 0; row < reels[col].Count; row++) {
                Vector2 pos = new Vector2(0, symbolTextureHeight * (row));
                Transform newSymbol = slotsManager.symbolFactory.createSymbolObj(reels[col][row], pos);
                newSymbol.parent = reelObjs[col];
            }

            reelObjs[col].position = new Vector2(symbolTextureWidth * (col - reels.Count / 2.0f + 0.5f), 0);
            reelObjs[col].GetComponent<Rigidbody2D>().velocity = new Vector2(0, -50 + 3 * col);
        }
    }
}
