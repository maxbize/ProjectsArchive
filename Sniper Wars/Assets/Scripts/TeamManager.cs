using UnityEngine;
using System.Collections;

public class TeamManager : MonoBehaviour
{

    public Transform playerPrefab;
    public Transform teamOneSpawn, teamTwoSpawn;

    // Max distance to spawn from spawn point
    private const float spawnDistX = 8.5f;
    private const float spawnDistZ = 4;

    public enum Team { one, two };

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public static Team newPlayerTeamNumber() {
        Player[] players = GameObject.FindObjectsOfType<Player>();
        int numTeam1 = 0, numTeam2 = 0;
        foreach (Player player in players) {
            switch (player.team) {
                case Team.one:
                    numTeam1++;
                    break;
                case Team.two:
                    numTeam2++;
                    break;
                default:
                    Debug.LogError("Warning, found player with unrecognized team " + player.team);
                    break;
            }
        }
        Debug.Log(numTeam1 + " " + numTeam2);
        if (numTeam1 <= numTeam2) {
            return Team.one;
        }
        else {
            return Team.two;
        }
    }

    public Vector3 newSpawnPoint(Team team) {
        Vector3 spawnOffset = new Vector3(Random.Range(-spawnDistX, spawnDistX), 0, Random.Range(-spawnDistZ, spawnDistZ));
        if (team == Team.one) {
            return teamOneSpawn.position + spawnOffset;
        }
        else {
            return teamTwoSpawn.position + spawnOffset;
        }
    }

    public void spawnPlayer() {
        Team team = newPlayerTeamNumber();
        if (team == Team.one) {
            Debug.Log("Spawned Player on team one");
            Quaternion look = Quaternion.identity;
            Network.Instantiate(playerPrefab, newSpawnPoint(team), look, 0);
        }
        else if (team == Team.two) {
            Debug.Log("Spawned Player on team two");
            Quaternion look = Quaternion.Euler(0, 180, 0);
            Network.Instantiate(playerPrefab, newSpawnPoint(team), look, 0);
        }
    }
}
