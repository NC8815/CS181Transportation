using UnityEngine;
using System.Collections;

public class Highlight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public float changeDuration;

	public void setDuration(float newD){
		changeDuration = newD;
	}

	IEnumerator rescale (float newScale){
		Vector3 startScale = gameObject.transform.localScale;
		Vector3 targetScale = new Vector3 (newScale, newScale, newScale);
		for (float t = 0; t < changeDuration; t += Time.deltaTime) {
			gameObject.transform.localScale = Vector3.Lerp (startScale, targetScale, t / changeDuration);
			yield return null;
		}
		gameObject.transform.localScale = targetScale;
	}

	IEnumerator recolor (Color newColor){
		Color initial = gameObject.GetComponent<Light> ().color;
		float[] startColor = {initial.r, initial.b, initial.g};
		float[] targetColor = {newColor.r, newColor.b, newColor.g};
		for (float t = 0; t < changeDuration; t += Time.deltaTime){
			Color tempColor = new Color (Mathf.Lerp (startColor [0], targetColor [0], t / changeDuration),
			                            Mathf.Lerp (startColor [1], targetColor [1], t / changeDuration),
			                            Mathf.Lerp (startColor [2], targetColor [2], t / changeDuration));
			gameObject.GetComponent<Light> ().color = tempColor;
			yield return null;
		}
		gameObject.GetComponent<Light> ().color = newColor;
	}

	IEnumerator rerange (float newRange){
		float startRange = gameObject.GetComponent<Light> ().range;
		for (float t = 0; t < changeDuration; t += Time.time) {
			float tempRange = Mathf.Lerp (startRange, newRange, t / changeDuration);
			gameObject.GetComponent<Light> ().range = tempRange;
			if (tempRange > gameObject.transform.localScale.x)
				gameObject.GetComponent<Light>().enabled = true;
			else
				gameObject.GetComponent<Light> ().enabled = false;
			yield return null;
		}
		gameObject.GetComponent<Light> ().range = newRange;
	}

	public void expand(Color newColor){
		StopAllCoroutines ();
		StartCoroutine (rerange (1.2f));
		StartCoroutine (rescale (1f));
		StartCoroutine (recolor (newColor));
	}

	public void contract(Color newColor){
		StopAllCoroutines ();
		StartCoroutine (rerange (0f));
		StartCoroutine (rescale (0.8f));
		StartCoroutine (recolor (newColor));
	}


	
	// Update is called once per frame
	void Update () {
	
	}
}
