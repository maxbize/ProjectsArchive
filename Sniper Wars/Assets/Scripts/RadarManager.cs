using UnityEngine;
using System.Collections;


public class RadarManager : MonoBehaviour {

    public Player myPlayer;
    public Transform radarBase;
    public RadarSpriteFactory factory;


	// Use this for initialization
	void Start () {
        // Ugly, non-centralized way to get a list of all players
        Player[] players = GameObject.FindObjectsOfType<Player>();
        foreach (Player player in players) {
            RadarSpriteFactory.blipType blipType = player.team == myPlayer.team ?
                RadarSpriteFactory.blipType.ally : RadarSpriteFactory.blipType.enemy;
            GameObject go = factory.makeNewSpriteObj(blipType);

        }
	}
	
	// Update is called once per frame
	void Update () {
	

	}
}
