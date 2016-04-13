using UnityEngine;
using System.Collections;

public class FloatSpin : MonoBehaviour {

	[SerializeField]
	private float rotatespeed = 1.0f;

	[SerializeField]
	private float floatSpeed = 2.5f;

	[SerializeField]
	private float movementDistance = 100.5f;

	private float startingY;
	private bool isMovingUp = true;
	
	// Use this for initialization
	void Start () {
		startingY = transform.position.y;
		transform.Rotate (transform.up, Random.Range (0f, 360f));
		StartCoroutine (Spin ());
		StartCoroutine (Float ());

	}

	private IEnumerator Spin()
	{
		while (true) {
			transform.Rotate (transform.up, 360 * rotatespeed * Time.deltaTime);
			yield return 0;
		}
	}

	private IEnumerator Float()
	{
		while (true) {
			float newY = transform.position.y + (isMovingUp ? 1 : -1) * movementDistance * floatSpeed * Time.deltaTime;
			if (newY > startingY + movementDistance)
			{
				newY = startingY + movementDistance;
				isMovingUp = false;
			}
			else if (newY < startingY)
			{
				newY = startingY;
				isMovingUp = true;
			}
			transform.position = new Vector3(transform.position.x, newY, transform.position.z);
			yield return 0;

		}
	}
}
