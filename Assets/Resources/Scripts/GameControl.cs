using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameControl : MonoBehaviour {

	public float turnLength = 1.5f;
	int turnCount = 0;
	int cargoPerTurn = 10;
	 
	List<Vehicle> vehicles = new List<Vehicle> ();
	Vehicle activeVehicle;

	MapSpace depotSpace;
	int depotScore = 0;

	MapSpace[,] map = new MapSpace[16,9];

	void Start () {
		for (int x = 0; x < 16; x++) {
			for (int y = 0; y < 9; y++) {
				map [x,y] = MapSpace.create(gameObject, new Vector3 (x - 7.5f, y - 4f),x,y);
			}
		}
		vehicles.Add (Airplane.create (this, map [0,8]));
		vehicles.Add (Train.create (this, map [0,0]));
		vehicles.Add (Boat.create (this, map [15,8]));
		depotSpace = map [15, 0];
		depotSpace.makeDepot ();
	}
	//Utility functions so things don't have to be public.
	public Vehicle getActiveVehicle(){return activeVehicle;}																															   //																															   //
	public MapSpace getMapSpace(int x, int y){return map [x,y];}
	public MapSpace getDepotSpace(){return depotSpace;}

	public void changeActiveVehicle(Vehicle newActive){
		if (activeVehicle != null) activeVehicle.gameObject.GetComponent<Highlight> ().shrinkVehicle ();
		activeVehicle = newActive;
		if (activeVehicle != null) activeVehicle.gameObject.GetComponent<Highlight> ().growVehicle ();
	}
	
	public void changeScore(int delta){
		depotScore += delta;
	}
	//if it's time to do a turn... do one.
	void Update () {
		if (Time.time > (turnCount + 1) * turnLength)
			doTurn ();
	}

	void doTurn(){
		foreach (Vehicle V in vehicles) {
			V.stockCargo (cargoPerTurn);
			V.moveVehicle ();
			V.dropCargo();
		}
		turnCount++;
	}
}