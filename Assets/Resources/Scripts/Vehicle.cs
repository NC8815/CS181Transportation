using UnityEngine;
using System.Collections;

public enum vehicleType{
	plane,
	train,
	boat
}

public class Vehicle : MonoBehaviour {



	vehicleType myType;
	GameObject myOrigin, myLocation, myDestination;
	public static GameObject prefab = Resources.Load ("Prefabs/Vehicle") as GameObject;
	GameControl myControl;
	int myCargo = 0;
	int turnsUntilMove = 0;
	int myMaxCargo, myMoveRate;
	static Vector3 offset = new Vector3 (0, 0, -2f);

	public static GameObject createPlane(GameObject controller, GameObject startSpace){
		GameObject go = Instantiate (prefab, startSpace.transform.position + Vehicle.offset, Quaternion.identity) as GameObject;
		Vehicle v = go.GetComponent<Vehicle> ();
		v.myType = vehicleType.plane;
		v.myOrigin = startSpace;
		v.myLocation = startSpace;
		v.myDestination = startSpace;
		v.myControl = controller.GetComponent<GameControl> ();
		v.myMaxCargo = 90;
		v.myMoveRate = 1;
		go.GetComponent<Light> ().color = Color.red;
		go.GetComponent<ParticleSystem> ().startColor = Color.red;
		startSpace.GetComponent<MapSpace> ().addLocal (go);
		startSpace.GetComponent<MapSpace> ().addComing (go);
		return go;
	}

	public static GameObject createTrain(GameObject controller, GameObject startSpace){
		GameObject go = Instantiate (prefab, startSpace.transform.position + Vehicle.offset, Quaternion.identity) as GameObject;
		Vehicle v = go.GetComponent<Vehicle> ();
		v.myType = vehicleType.train;
		v.myOrigin = startSpace;
		v.myLocation = startSpace;
		v.myDestination = startSpace;
		v.myControl = controller.GetComponent<GameControl> ();
		v.myMaxCargo = 200;
		v.myMoveRate = 2;
		go.GetComponent<Light> ().color = Color.green;
		go.GetComponent<ParticleSystem> ().startColor = Color.green;
		startSpace.GetComponent<MapSpace> ().addLocal (go);
		startSpace.GetComponent<MapSpace> ().addComing (go);
		return go;
	}

	public static GameObject createBoat(GameObject controller, GameObject startSpace){
		GameObject go = Instantiate (prefab, startSpace.transform.position + Vehicle.offset, Quaternion.identity) as GameObject;
		Vehicle v = go.GetComponent<Vehicle> ();
		v.myType = vehicleType.boat;
		v.myOrigin = startSpace;
		v.myLocation = startSpace;
		v.myDestination = startSpace;
		v.myControl = controller.GetComponent<GameControl> ();
		v.myMaxCargo = 550;
		v.myMoveRate = 3;
		go.GetComponent<Light> ().color = Color.blue;
		go.GetComponent<ParticleSystem> ().startColor = Color.blue;
		startSpace.GetComponent<MapSpace> ().addLocal (go);
		startSpace.GetComponent<MapSpace> ().addComing (go);
		return go;
	}

	public void stockCargo(int deltaCargo){
		if (myLocation == myOrigin) {
			myCargo = Mathf.Clamp (myCargo + deltaCargo, 0, myMaxCargo);
			gameObject.GetComponent<ParticleSystem>().emissionRate = 10 * myCargo / myMaxCargo;
		}
	}

	public void moveVehicle(){
		if (myLocation != myDestination) {
			MapSpace loc = myLocation.GetComponent<MapSpace>();
			MapSpace dest = myDestination.GetComponent<MapSpace>();
			int deltaX = Mathf.Clamp(dest.getX () - loc.getX (),-1,1);
			int deltaY = Mathf.Clamp(dest.getY () - loc.getY (),-1,1);
			GameObject newLocation = myControl.getFromMap(loc.getX() + deltaX, loc.getY() + deltaY);
			if (turnsUntilMove == 0){
				StopAllCoroutines();
				StartCoroutine(changePosition(newLocation.transform.position));
				myLocation.GetComponent<MapSpace>().removeLocal(gameObject);
				myLocation = newLocation;
				myLocation.GetComponent<MapSpace>().addLocal(gameObject);
				turnsUntilMove = myMoveRate;
			}
			turnsUntilMove--;
		}
	}

	public int dropCargo(){
		int deltaCargo = myCargo;
		myCargo = 0;
		gameObject.GetComponent<ParticleSystem> ().emissionRate = 0;
		return deltaCargo;
	}

	public void newDestination(GameObject newD){
		myDestination.GetComponent<MapSpace> ().removeComing (gameObject);
		myDestination = newD;
		myDestination.GetComponent<MapSpace> ().addComing (gameObject);
	}

	IEnumerator changePosition(Vector3 newPosition){
		Vector3 startPosition = gameObject.transform.position;
		Vector3 targetPosition = newPosition + Vehicle.offset;
		float changeDuration = myControl.turnLength;
		for (float t = 0; t < changeDuration; t += Time.deltaTime) {
			gameObject.transform.position = Vector3.Lerp (startPosition, targetPosition, t / changeDuration);
			yield return null;
		}
		gameObject.transform.position = targetPosition;
	}





	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
