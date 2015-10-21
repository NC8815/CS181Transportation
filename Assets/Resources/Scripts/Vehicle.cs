using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour {
	//protected means things that extend this class get their own version of this variable. I wanted to clarify the constructors for 
	//boat, train, or airplane, so I made all of these protected to pass them down to the sub-classes, where the constructors can access them.
	//It also represented good practice for classes.
	protected MapSpace myOrigin, myLocation, myDestination;
	protected GameControl myControl;
	protected int myCargo = 0;
	protected int turnsUntilMove;
	protected int myMaxCargo, myMoveRate;
	protected static Vector3 offset = new Vector3 (0, 0, -2f);
	protected static GameObject prefab = Resources.Load ("Prefabs/Vehicle") as GameObject;

	public void stockCargo(int deltaCargo){ //restock the vehicle if it's at its start point. Reflect that with the particles
		if (myLocation == myOrigin) {
			myCargo = Mathf.Clamp (myCargo + deltaCargo, 0, myMaxCargo);
			gameObject.GetComponent<ParticleSystem>().emissionRate = 10 * myCargo / myMaxCargo;
		}
	}

	public void moveVehicle(){
		turnsUntilMove--;//count down to move; we're counting out here because if a vehicle has been stopped for long enough, it should go immediately 
		if (myLocation != myDestination) {
			int newX = myLocation.getX (); //the initial new location is the current location
			int newY = myLocation.getY ();
			int deltaX = myDestination.getX () - myLocation.getX ();//figure out which direction we have to go to reach myDestination
			int deltaY = myDestination.getY () - myLocation.getY ();
			if (deltaX > 0) newX++; 
			if (deltaX < 0) newX--; //if we've gotten here, up to one x difference will be true, and up to one y difference will be true
			if (deltaY > 0) newY++; //and at least one change will occur
			if (deltaY < 0) newY--;
			if (turnsUntilMove < 1) {	//if it's time to move the vehicle, actually move it.
				myLocation.removeLocal (this); 	//tell the old space to drop it.
				myLocation = myControl.getMapSpace (newX, newY);				//get the new space.
				myLocation.addLocal (this);		//tell the new space to add it.
				StopAllCoroutines ();											//animate the change.
				StartCoroutine (changeLocation (myLocation.gameObject));
				turnsUntilMove = myMoveRate;									//reset the move counter.
			}
		}
	}

	public void dropCargo(){ //If the vehicle is at the depot location, drop it's cargo and reflect that with the particles
		if (myLocation == myControl.getDepotSpace () && myCargo > 0) {
			myControl.changeScore(myCargo);
			myCargo = 0;
			gameObject.GetComponent<ParticleSystem> ().emissionRate = 0;
		}
	}

	public void newDestination(MapSpace newD){
		myDestination.removeComing (this);
		myDestination = newD;
		myDestination.addComing (this);
	}
	//this enumerator animates the vehicle motion as it changes location. The location change happens instantly for game logic reasons,
	//but the vehicles take different amounts of time to animate the move, and the slower vehicles have a wait period between actual moves.
	protected IEnumerator changeLocation(GameObject newSpace){
		Vector3 startPosition = gameObject.transform.position;
		Vector3 targetPosition = newSpace.transform.position + Vehicle.offset;
		float changeDuration = myControl.turnLength * myMoveRate;
		for (float t = 0; t < changeDuration; t += Time.deltaTime) {
			gameObject.transform.position = Vector3.Lerp (startPosition, targetPosition, t / changeDuration);
			yield return null;
		}
		gameObject.transform.position = targetPosition;
	}
}
