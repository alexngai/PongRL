using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundController : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.name.Equals("Ball")) {
			GameController.Score (transform.name);
		}
	}
}
