using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public TeamManager.Team team;
    public Transform myGraphics;
    public int id;
    public string displayName;

    private int health = 100;

	// Use this for initialization
	void Start () {
	    // We can figure out our team from our spawn location. Kinda hacky...
        if (transform.position.z < 0) {
            team = TeamManager.Team.one;
            myGraphics.renderer.material.color = Color.green;
        }
        else {
            team = TeamManager.Team.two;
            myGraphics.renderer.material.color = Color.blue;
        }
        FindObjectOfType<GameManager>().registerNewPlayer(this);
        if (networkView.isMine) {
            networkView.RPC("registerDisplayName", RPCMode.AllBuffered, PlayerPrefs.GetString(PlayerPrefsKeys.displayName));
        }
	}
	

    [RPC]
    public void registerDamage(int damage, int shooterId) {
        health -= damage;
        Debug.Log("Got hit for: " + damage);

        if (health < 0) {
            FindObjectOfType<GameManager>().registerKill(shooterId, id);
        }

        if (health < 0 && Network.isServer) {
            networkView.RPC("respawn", RPCMode.All, GameObject.FindObjectOfType<TeamManager>().newSpawnPoint(team));
        }
    }

    [RPC]
    public void respawn(Vector3 pos) {
        transform.position = pos;
        health = 100;
    }

    [RPC]
    public void registerDisplayName(string displayName) {
        this.displayName = displayName;
    }
}
