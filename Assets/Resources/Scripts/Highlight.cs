using UnityEngine;
using System.Collections;

public class Highlight : MonoBehaviour {

	//this behavior attaches to both spaces and vehicles, and governs their changes in size, color, and transparency.

	public float changeDuration;

	public float currentAlpha;

	float[] spaceScaleRange = {0.8f, 1};
	float spaceHaloSize = 0.2f;

	float[] vehicleScaleRange = {0.5f, 0.6f};
	float[] vehicleTranRange = {0.5f, 1};

	void Start(){
		changeDuration = GameObject.Find ("GameController").GetComponent<GameControl> ().turnLength / 2f;
	}

	public void growVehicle(){ StopCoroutine ("scaleVehicle"); StartCoroutine ("scaleVehicle",1); }
	public void shrinkVehicle(){ StopCoroutine ("scaleVehicle"); StartCoroutine ("scaleVehicle",0); }

	public void growSpace(){ StopCoroutine ("scaleSpace"); StartCoroutine ("scaleSpace",1); }
	public void shrinkSpace(){ StopCoroutine ("scaleSpace"); StartCoroutine ("scaleSpace",0); }
	public void updateSpaceColor(Color newColor){ StopCoroutine ("changeColor"); StartCoroutine ("changeColor", newColor); }

	IEnumerator scaleVehicle(int index){
		Vector3 startScale = gameObject.transform.localScale;
		Color startColor = gameObject.GetComponent<Renderer> ().material.color;
		Vector3 targetScale = new Vector3 (vehicleScaleRange [index], vehicleScaleRange [index], vehicleScaleRange [index]);
		Color targetColor = gameObject.GetComponent<Renderer> ().material.color;
		targetColor.a = vehicleTranRange [index];
		for (float t = 0; t < changeDuration; t += Time.deltaTime) {
			gameObject.transform.localScale = Vector3.Lerp (startScale, targetScale, t / changeDuration);
			gameObject.GetComponent<Renderer> ().material.color = Color.Lerp (startColor, targetColor, t / changeDuration);
			yield return null;
		}
		gameObject.transform.localScale = targetScale;
		gameObject.GetComponent<Renderer> ().material.color = targetColor;
	}

	IEnumerator scaleSpace(int index){
		Vector3 startScale = gameObject.transform.localScale;
		float startLRange = gameObject.GetComponent<Light> ().range;
		Vector3 targetScale = new Vector3 (spaceScaleRange [index], spaceScaleRange [index], spaceScaleRange [index]);
		float targetLRange = spaceScaleRange[index] + spaceHaloSize;
		for (float t = 0; t < changeDuration; t += Time.deltaTime) {
			gameObject.transform.localScale = Vector3.Lerp (startScale, targetScale, t / changeDuration);
			gameObject.GetComponent<Light> ().range = Mathf.Lerp (startLRange, targetLRange, t / changeDuration);
			yield return null;
		}
		gameObject.transform.localScale = targetScale;
		gameObject.GetComponent<Light> ().range = targetLRange;
		yield break;
	}

	IEnumerator changeColor (Color targetColor){
		Color startColor = gameObject.GetComponent<Light> ().color;
		for (float t = 0; t < changeDuration; t += Time.deltaTime) {
			gameObject.GetComponent<Light> ().color = Color.Lerp (startColor, targetColor, t / changeDuration);
			gameObject.GetComponent<Light> ().intensity = Mathf.Lerp (startColor.a, 2*targetColor.a, t / changeDuration);
			yield return null;
		}
		gameObject.GetComponent<Light> ().color = targetColor;
		gameObject.GetComponent<Light> ().intensity = targetColor.a;
		yield break;
	}

	void Update(){
		currentAlpha = gameObject.GetComponent<Renderer> ().material.color.a;
	}
}
