using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    private Dictionary<TeamManager.Team, int> score = new Dictionary<TeamManager.Team, int>();
    private Dictionary<int, Player> players = new Dictionary<int,Player>();
    private UIManager uiManager;

    private int currentId = 0;

	// Use this for initialization
	void Start () {
        score[TeamManager.Team.one] = 0;
        score[TeamManager.Team.two] = 0;
        uiManager = GameObject.FindObjectOfType<UIManager>();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void registerKill(int killerId, int killedId) {
        score[players[killerId].team]++;
        uiManager.updateScoreText(score[TeamManager.Team.one], score[TeamManager.Team.two]);
        uiManager.postKillText(players[killerId], players[killedId]);
    }

    public void registerNewPlayer(Player player) {
        player.id = currentId;
        players[currentId] = player;
        currentId++;
        Vector3 dist = new Vector3(1, 2, 3);
        Vector3 a = Quaternion.identity * dist;
        //Quaternion.a
    }

    public Player getPlayerFromId(int id) {
        return players[id];
    }
}
