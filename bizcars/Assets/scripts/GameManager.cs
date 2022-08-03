using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public int nbPlayers = 1;
	public Transform playerCar;

	public int nbAI = 1;
	public Transform AI_Car;

	private double[][] startingPositions = new double[][]{
		new double[]{0.05, 3.06, -0.1},
		new double[]{-0.19, 2.56, -0.1},
		new double[]{-0.68, 3.06, -0.1},
		new double[]{-0.94, 2.56, -0.1}
	};

	public static Color[] PLAYER_COLORS = new Color[]{
		Color.blue, Color.red, Color.yellow, Color.green
	};


	// Use this for initialization
	void Start () {
		int players = (int) Mathf.Clamp(nbPlayers, 0, startingPositions.Length);
		int ai = (int) Mathf.Clamp(nbAI, 0, startingPositions.Length - players);

		int i;
		for (i = 0; i < players + ai; i++) {
			if (i < players) {
				spawnCar (playerCar, i, true);
			}
			else {
				spawnCar (AI_Car, i, false);
			}
		}
	}
	
	void spawnCar(Transform prefab, int playerIndex, bool isHuman) {
			// position the players
			Transform obj = (Transform) Instantiate(prefab, new Vector3((float)this.startingPositions[playerIndex][0], 
		                                                            (float)this.startingPositions[playerIndex][1], 
		                                                            (float)this.startingPositions[playerIndex][2]),
			                                        						Quaternion.identity);
			
			// assign keys to the players
			GameObject car = obj.gameObject;
			if (isHuman) {
				CarController controller = car.GetComponent<CarController> ();
				controller.SetPlayerInputNb (playerIndex);
			}

			obj.Rotate(0, 0, -90f);
			obj.name = "Car Player " + (playerIndex + 1).ToString();
			
			// render a different color for each player
			SpriteRenderer renderer = car.GetComponent<SpriteRenderer>();
			renderer.color = PLAYER_COLORS[playerIndex];
	}
	
}
