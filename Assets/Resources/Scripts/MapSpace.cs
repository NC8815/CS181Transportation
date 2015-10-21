using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapSpace : MonoBehaviour {

	GameControl myControl;
	bool notDepot = true;
	public int myX,myY;
	List<Vehicle> localVehicles = new List<Vehicle> (); //vehicles at this location.
	List<Vehicle> comingVehicles = new List<Vehicle> ();//vehicles headed for this location.

	public static GameObject prefab = Resources.Load ("Prefabs/MapSpace") as GameObject;
	public static MapSpace create(GameObject controller, Vector3 position, int nX, int nY){
		GameObject go = Instantiate (prefab, position, Random.rotation) as GameObject;
		MapSpace ms = go.GetComponent<MapSpace> ();
		ms.myControl = controller.GetComponent<GameControl> ();
		ms.myX = nX;
		ms.myY = nY;
		return ms;
	}

	public int getX(){return myX;}
	public int getY(){return myY;}

	public void makeDepot(){
		notDepot = false;
		gameObject.GetComponent<Highlight> ().growSpace();
		gameObject.GetComponent<Highlight> ().updateSpaceColor (Color.black);
		gameObject.GetComponent<Renderer> ().material.color = Color.black;
	}

	public void addLocal(Vehicle added){
		localVehicles.Add (added);
		if (notDepot)
			gameObject.GetComponent<Highlight> ().growSpace ();
	}

	public void removeLocal(Vehicle dropped){
		localVehicles.Remove (dropped);
		if (notDepot && localVehicles.Count() == 0)
			gameObject.GetComponent<Highlight> ().shrinkSpace ();
	}

	public void addComing (Vehicle added){
		comingVehicles.Add (added);
		Color spaceColor = new Color (comingVehicles.Sum (V => V.GetComponent<Renderer> ().material.color.r), 
		                              comingVehicles.Sum (V => V.GetComponent<Renderer> ().material.color.g),
		                              comingVehicles.Sum (V => V.GetComponent<Renderer> ().material.color.b),
		                              1f);
		if (notDepot) 
			gameObject.GetComponent<Highlight> ().updateSpaceColor (spaceColor);
	}

	public void removeComing(Vehicle dropped){
		comingVehicles.Remove (dropped);
		Color spaceColor = new Color (comingVehicles.Sum (V => V.GetComponent<Renderer> ().material.color.r), 
		                              comingVehicles.Sum (V => V.GetComponent<Renderer> ().material.color.g),
		                              comingVehicles.Sum (V => V.GetComponent<Renderer> ().material.color.b),
		                              1f);
		if (notDepot) {
			if(comingVehicles.Count() == 0)
				spaceColor.a = 0;
			gameObject.GetComponent<Highlight> ().updateSpaceColor (spaceColor);
		}
	}

	void OnMouseDown(){																			
		bool activeExists = (myControl.getActiveVehicle () != null);								
		bool hasVehicles = (localVehicles.Count () != 0);
		bool alreadyHere = localVehicles.Contains (myControl.getActiveVehicle ());
		bool alreadyComing = comingVehicles.Contains (myControl.getActiveVehicle ());
		List<Vehicle> otherLocal = localVehicles.Except (new List<Vehicle>{ myControl.getActiveVehicle ()}).ToList ();

		if (activeExists) {//if there's an active vehicle, make it come here.
			myControl.getActiveVehicle ().newDestination (this);
			if (alreadyComing) {//if it was already coming here, deactivate it.
				myControl.changeActiveVehicle (null);
				if (alreadyHere && otherLocal.Count != 0) //if it's already here, and there are other vehicles here, activate one.
					myControl.changeActiveVehicle (otherLocal [Random.Range (0, otherLocal.Count)]);
			}
		} else if (hasVehicles)//if there isn't an active vehicle, but there are vehicles here, activate one.
			myControl.changeActiveVehicle (localVehicles [Random.Range (0, localVehicles.Count)]);
		else if (comingVehicles.Count != 0) //if there isn't an active vehicle, and no vehicles are here, but any are coming, activate one of those.
			myControl.changeActiveVehicle (comingVehicles [Random.Range (0, comingVehicles.Count)]);
	}

	void OnMouseOver(){
		gameObject.GetComponent<Light> ().color = Color.white;
		gameObject.GetComponent<Light>().intensity = 2;
	}

	void OnMouseExit(){
		Color spaceColor = new Color (comingVehicles.Sum (V => V.GetComponent<Renderer> ().material.color.r), 
		                              comingVehicles.Sum (V => V.GetComponent<Renderer> ().material.color.g),
		                              comingVehicles.Sum (V => V.GetComponent<Renderer> ().material.color.b),
		                              1f);
		if (notDepot) {
			if (comingVehicles.Count () == 0)
				gameObject.GetComponent<Light>().intensity = 0;
			gameObject.GetComponent<Light> ().color = spaceColor;
		}else
			gameObject.GetComponent<Light> ().color = Color.black;
	}
}
