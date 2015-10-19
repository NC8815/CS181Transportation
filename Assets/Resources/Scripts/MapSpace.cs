using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapSpace : MonoBehaviour {

	GameControl controls;
	int X,Y;
	List<GameObject> localVehicles = new List<GameObject> (); //vehicles at this location.
	List<GameObject> comingVehicles = new List<GameObject> ();//vehicles headed for this location.

	public static GameObject prefab = Resources.Load ("Prefabs/MapSpace") as GameObject;
	public static GameObject create(GameObject controller, Vector3 position, int nX, int nY){
		GameObject go = Instantiate (prefab, position, Quaternion.AngleAxis (15, Random.onUnitSphere)) as GameObject;
		MapSpace ms = go.GetComponent<MapSpace> ();
		Highlight hl = go.GetComponent<Highlight> ();
		ms.controls = controller.GetComponent<GameControl> ();
		ms.X = nX;
		ms.Y = nY;
		hl.setDuration (ms.controls.turnLength);
		return go;
	}

	public int getX(){
		return X;
	}
	
	public int getY(){
		return Y;
	}

	public void addLocal(GameObject added){
		localVehicles.Add (added);
	}

	public void removeLocal(GameObject dropped){
		localVehicles.Remove (dropped);
	}

	public void addComing (GameObject added){
		comingVehicles.Add (added);
		Color spaceColor = new Color (comingVehicles.Sum (V => V.GetComponent<Light> ().color.r), 
		                              comingVehicles.Sum (V => V.GetComponent<Light> ().color.g),
		                              comingVehicles.Sum (V => V.GetComponent<Light> ().color.b));
		gameObject.GetComponent<Highlight> ().expand (spaceColor);
	}

	public void removeComing(GameObject dropped){
		comingVehicles.Remove (dropped);
		Color spaceColor = new Color (comingVehicles.Sum (V => V.GetComponent<Light> ().color.r), 
		                              comingVehicles.Sum (V => V.GetComponent<Light> ().color.g),
		                              comingVehicles.Sum (V => V.GetComponent<Light> ().color.b));
		if (comingVehicles.Count () == 0)
			gameObject.GetComponent<Highlight> ().contract (spaceColor);
		else
			gameObject.GetComponent<Highlight> ().StartCoroutine ("recolor", spaceColor);
	}

	void OnMouseDown(){
		bool activeExists = (controls.getActiveVehicle () != null);
		bool spaceEmpty = (localVehicles.Count () == 0);
		List<GameObject> activeInList = new List<GameObject> ();
		if (activeExists) {
//			if (spaceEmpty || !localVehicles.Contains(controls.getActiveVehicle()))
			controls.getActiveVehicle().GetComponent<Vehicle>().newDestination(gameObject);
			if (localVehicles.Contains(controls.getActiveVehicle())){
				activeInList.Add(controls.getActiveVehicle());
				List<GameObject> temp = localVehicles.Except(activeInList).ToList();
				if (temp.Count() > 0)
					controls.setActiveVehicle(temp[Random.Range (0,temp.Count())]);
				else
					controls.setActiveVehicle(null);
			}
		} else if (!spaceEmpty) {
			GameObject randNth = localVehicles[Random.Range(0,localVehicles.Count())];
			controls.setActiveVehicle(randNth);
		}
	}
}
