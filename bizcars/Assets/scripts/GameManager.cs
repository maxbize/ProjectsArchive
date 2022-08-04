using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public int nbPlayers; // Set this in the scene
	public Transform playerCar;

    public int nbAI; // Set this in the scene
	public Transform AI_Car;

	private StartingPosition[] startingPositions = new StartingPosition[CONSTANTS.MAX_NUM_PLAYERS];

	public static Color[] PLAYER_COLORS = new Color[]{
		Color.blue, Color.red, Color.yellow, Color.green
	};

	public float startUpTime = 3.0f;

	// Use this for initialization
	void Start () {
		// Get the starting positions and sort them
		startingPositions = GameObject.Find ("StartingPositions").GetComponentsInChildren<StartingPosition>();
		
		if (!MainMenu.vsAi) {
			nbAI = 0;
			Destroy(FindObjectOfType<ItemsManager>());
        }

		int players = (int) Mathf.Clamp(nbPlayers, 0, startingPositions.Length);
		int ai = (int) Mathf.Clamp(nbAI, 0, startingPositions.Length - players);

		for (int i = 0; i < players + ai; i++) {
			if (i < players) {
				spawnCar (playerCar, i, true);
			}
			else {
				spawnCar (AI_Car, i, false);
			}
		}
	}

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
			SceneManager.LoadScene("Menu");
        }
    }

    void spawnCar(Transform prefab, int playerIndex, bool isHuman) {
		// position the players
		Transform startPosTr;
		for (int i = 0; i < startingPositions.Length; i++) {
			if (startingPositions[i].carNum == playerIndex) {
				startPosTr = startingPositions[i].transform;
				break;
			}
		}
		Transform obj = (Transform) Instantiate(prefab, startingPositions[playerIndex].transform.position, 
		                                        Quaternion.Euler(startingPositions[playerIndex].transform.eulerAngles));
		
		// assign keys to the players
		GameObject car = obj.gameObject;
	    if (isHuman) {
	        CarController controller = car.GetComponent<CarController>();
	        controller.SetPlayerInputNb(playerIndex);
			controller.disableControl(startUpTime);
	    }
	    else {
			AI_CarController carController = car.GetComponent<AI_CarController>();
	        carController.playerNb = playerIndex;
			carController.disableControl(startUpTime);
	    }

		obj.name = "Car Player " + (playerIndex + 1).ToString();
		
		// render a different color for each player
		SpriteRenderer renderer = car.GetComponent<SpriteRenderer>();
		renderer.color = PLAYER_COLORS[playerIndex];
		car.GetComponent<GhostRecorder>().enabled = isHuman && !MainMenu.vsAi;

	}
}
