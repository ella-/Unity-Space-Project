using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public float countdownTime;
	public Text timerText;
	public Player player;

	float time;

	void Awake () {
		timerText.text = countdownTime.ToString () + " seconds";
	}

	void Update () {
		if (time < countdownTime) {
			time += Time.deltaTime;
			timerText.text = (countdownTime - time).ToString () + " seconds";
		} else {
			player.Reset ();
			time = 0f;
		}
	}
}
