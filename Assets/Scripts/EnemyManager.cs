using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyManager : MonoBehaviour {

	public Text enemyTimerText;
	public float enemyTimer = 30f;

	Enemy[] enemies;
	bool visible = true;

	bool Visible {
		get { return visible; }
		set {
			if (visible != value) {
				visible = value;
				foreach (Enemy enemy in enemies)
					enemy.SetVisible (visible);
			}
		}
	}

	void Awake () {
		enemies = GameObject.FindObjectsOfType<Enemy> () as Enemy[];
		enemyTimerText.text = enemyTimer.ToString () + " seconds";
		// StartCoroutine (CoToggleVisible (1f)); // uncomment for blink

	}

	void ToggleVisible () {
		Visible = !Visible;
	}

	void Update () {
		if (Input.GetKey (KeyCode.Q)) {
			if (enemyTimer > 0) {
				enemyTimer -= Time.deltaTime;
				enemyTimerText.text = enemyTimer.ToString () + " seconds";
				Visible = true;
			}
		} else {
			Visible = false;
		}
	}

	IEnumerator CoToggleVisible (float time) {
		yield return new WaitForSeconds (time);
		ToggleVisible ();
		StartCoroutine (CoToggleVisible (time));
	}
}
