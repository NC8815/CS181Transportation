using UnityEngine;
using System.Collections;

public class Train : Vehicle {
	//this is the constructor for the Train class.
	public static Vehicle create(GameControl controller, MapSpace startSpace){
		GameObject go = Instantiate (prefab, startSpace.transform.position + Vehicle.offset, Quaternion.identity) as GameObject;
		Train T = go.AddComponent<Train> ();
		T.myOrigin = startSpace;
		T.myLocation = startSpace;
		T.myDestination = startSpace;
		T.myControl = controller;
		T.myMaxCargo = 200;
		T.turnsUntilMove = 1;
		T.myMoveRate = 2;
		Color temp = Color.green; temp.a = 0.5f;
		go.GetComponent<Renderer> ().material.color = temp;
		go.GetComponent<ParticleSystem> ().startColor = Color.green;
		startSpace.addLocal (T);
		startSpace.addComing (T);
		return T;
	}
	


}
