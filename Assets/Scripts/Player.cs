using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	Vector3 startPosition;

	void Awake () {
		startPosition = transform.position;
	}

	public void Reset () {
		transform.position = startPosition;
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.tag == "Enemy") {
			Reset ();
		} else if (collider.gameObject.tag == "EndLevel") {
//			SceneManager.LoadScene ("SCENEName");
			Debug.Log ("end game");
		}
	}
}
