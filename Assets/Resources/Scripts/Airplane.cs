using UnityEngine;
using System.Collections;

public class Airplane : Vehicle {
	//this is the constructor for the Airplane class.
	public static Vehicle create(GameControl controller, MapSpace startSpace){
		GameObject go = Instantiate (prefab, startSpace.transform.position + Airplane.offset, Quaternion.identity) as GameObject;
		Airplane A = go.AddComponent<Airplane> ();
		A.myOrigin = startSpace;
		A.myLocation = startSpace;
		A.myDestination = startSpace;
		A.myControl = controller;
		A.myMaxCargo = 90;
		A.turnsUntilMove = 1;
		A.myMoveRate = 1;
		Color temp = Color.red; temp.a = 0.5f;
		go.GetComponent<Renderer> ().material.color = temp;
		go.GetComponent<ParticleSystem> ().startColor = Color.red;
		startSpace.addLocal (A);
		startSpace.addComing (A);
		return A;
	}
}
