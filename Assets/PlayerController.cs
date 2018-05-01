using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public KeyCode moveUp = KeyCode.W;
	public KeyCode moveDown = KeyCode.S;

	public float maxSpeed = 5f;
	public float boundY = 4f;

	private Rigidbody2D rb2d;

	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
	}

	void Update () {
		Vector2 vel = rb2d.velocity;
		if (Input.GetKey (moveUp)) {
			// move up
			vel.y = maxSpeed;
			//Debug.Log ("move up", gameObject);
		} else if (Input.GetKey (moveDown)) {
			// move down
			vel.y = -maxSpeed;
			//Debug.Log ("move down", gameObject);
		} else {
			// stop 
			vel.y = 0f;
		}
		rb2d.velocity = vel;

		Vector2 pos = transform.position;
		if (pos.y > boundY) {
			pos.y = boundY;
		} else if (pos.y < -boundY) {
			pos.y = -boundY;
		}
		transform.position = pos;
	}
}
