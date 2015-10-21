using UnityEngine;
using System.Collections;

public class Boat : Vehicle {
	//this is the constructor for the Boat class.
	public static Vehicle create(GameControl controller, MapSpace startSpace){
		GameObject go = Instantiate (prefab, startSpace.transform.position + Vehicle.offset, Quaternion.identity) as GameObject;
		Boat B = go.AddComponent<Boat> ();
		B.myOrigin = startSpace;
		B.myLocation = startSpace;
		B.myDestination = startSpace;
		B.myControl = controller;
		B.myMaxCargo = 550;
		B.turnsUntilMove = 1;
		B.myMoveRate = 3;
		Color temp = Color.blue; temp.a = 0.5f;
		go.GetComponent<Renderer> ().material.color = temp;
		go.GetComponent<ParticleSystem> ().startColor = Color.blue;
		startSpace.addLocal (B);
		startSpace.addComing (B);
		return B;
	}
}
