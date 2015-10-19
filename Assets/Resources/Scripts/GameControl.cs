using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameControl : MonoBehaviour {

	public float turnLength = 1.5f;
	int turnCount = 0;

	List<GameObject> vehicles = new List<GameObject> ();
	GameObject activeVehicle;

	GameObject[][] map = new GameObject[16][];

	void Start () {
		for (int x = 0; x < 16; x++) {
			map [x] = new GameObject[9];
			for (int y = 0; y < 9; y++) {
				map [x] [y] = MapSpace.create(gameObject, new Vector3 (x - 7.5f, y - 4f),x,y);
			}
		}
		vehicles.Add (Vehicle.createPlane (gameObject, map [0] [8]));
		vehicles.Add (Vehicle.createTrain (gameObject, map [0] [0]));
		vehicles.Add (Vehicle.createBoat (gameObject, map [15] [8]));
	}
	//I wanted to minimize the use of public variables, so I have a set of exposition functions for interacting with other objects.//
	public void setActiveVehicle(GameObject newActive){																			   //
		activeVehicle = newActive;																								   //
	}																															   //
																																   //
	public GameObject getActiveVehicle(){																						   //
		return activeVehicle;																									   //
	}																															   //
																																   //
	public GameObject getFromMap(int X, int Y){																					   //
		return map [X] [Y];																										   //
	}																															   //
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void doTurn(){
		foreach (GameObject V in vehicles) {
			V.GetComponent<Vehicle> ().stockCargo (10);
			V.GetComponent<Vehicle> ().moveVehicle ();
		}
		//depotVehicles();
		turnCount++;
	}

	void Update () {
		if (Time.time > (turnCount + 1) * turnLength)
			doTurn ();
	}
}
