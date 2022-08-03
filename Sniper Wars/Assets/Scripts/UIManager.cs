using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

    private Text ammoText;
    private Text scoreText;
    private Text killText;

    private const string yellowStart = "<color=#ffff00bf>";
    private const string greenStart = "<color=green>"; // Team 1
    private const string blueStart = "<color=blue>"; // Team 2
    private const string colorEnd = "</color>";
    private const string boldStart = "<b>";
    private const string boldEnd = "</b>";


    private string[] killTextVerbs = new string[] { " killed ", " destroyed ", " ended ", " obliterated ", " erased ", " capped " };
    private const int MAX_MSGS = 3;
    public List<string> killMsgs = new List<string>(MAX_MSGS);

    private float lastKillUpdateTime = 0;
    private float killDisplayTime = 3;

    void Update() {
        if (Time.timeSinceLevelLoad - lastKillUpdateTime > killDisplayTime) {
            popKillMessageQueue();
        }
    }

	// Use this for initialization
	void Start () {
        ammoText = GameObject.Find("Ammo Text").GetComponent<Text>();
        scoreText = GameObject.Find("Score Text").GetComponent<Text>();
        killText = GameObject.Find("Kill Text").GetComponent<Text>();
        updateScoreText(0, 0);
    }

    public void updateAmmoText(int ammo, int maxAmmo) {
        ammoText.text = yellowStart + ammo + " / " + maxAmmo + colorEnd;
    }

    public void updateScoreText(int team1score, int team2score) {
        scoreText.text = greenStart + team1score + colorEnd + " : " + blueStart + team2score + colorEnd;
    }

    public void postKillText(Player killer, Player victim) {
        string killerColorstart = killer.team == TeamManager.Team.one ? greenStart : blueStart;
        string victimColorstart = victim.team == TeamManager.Team.one ? greenStart : blueStart;
        pushKillMessageQueue(killerColorstart + killer.displayName + colorEnd 
            + boldStart + killTextVerbs[Random.Range(0, killTextVerbs.Length - 1)] + boldEnd
            + victimColorstart + victim.displayName + colorEnd);
    }

    private void popKillMessageQueue() {
        if (killMsgs.Count > 0) {
            killMsgs.RemoveAt(0);
        }
        refreshMessageDisplay();
    }

    private void pushKillMessageQueue(string msg) {
        if (killMsgs.Count == MAX_MSGS) {
            popKillMessageQueue();
        }
        killMsgs.Insert(killMsgs.Count, msg);
        refreshMessageDisplay();
    }

    private void refreshMessageDisplay() {
        string txt = "";
        for (int i = 0; i < killMsgs.Count; i++) {
            txt += killMsgs[i] + "\n";
        }
        killText.text = txt;
        lastKillUpdateTime = Time.timeSinceLevelLoad;
    }
}
