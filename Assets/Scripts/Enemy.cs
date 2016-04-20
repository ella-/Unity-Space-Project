using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public GameObject render;

	public void SetVisible (bool visible) {
		render.gameObject.SetActive (visible);
	}
}
