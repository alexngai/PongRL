using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

	public float maxSpeed = 5f;
	public string playerTag = "Player";
	public string wallTag = "Wall";
	public float maxSpin = 5f;
	public float spinForce = 0.02f; // As a percentage of current velocity.

	private Rigidbody2D rb2d; 
	private SpriteRenderer srend;
	private float spin = 0f;
	//private float paddleHeight = 2f;

	private ParticleSystem ps;

	private float maxBounceAngleInRadians = 1.2f;

	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		srend = GetComponent<SpriteRenderer> ();
		//Invoke ("StartMovement", 1);
		ps = GetComponentInChildren<ParticleSystem> ();
	}

	void FixedUpdate() {
		// Apply force perpendicular to movement if spin is nonzero.
		if (!Mathf.Approximately (spin, 0f) && !Mathf.Approximately(rb2d.velocity.magnitude, 0f)) {
			Vector2 vel = rb2d.velocity;
			Vector2 forceVector = new Vector2 ((spin > 0 ? 1f : -1f) * vel.y, (spin > 0 ? -1f: 1f) * vel.x);
			forceVector.Normalize ();

			Vector2 newVel = vel + forceVector * spinForce * spin;
			//Debug.Log ("vel: " + newVel.ToString() + " spinning: " + forceVector * spinForce * spin);
			newVel.Normalize ();
			rb2d.velocity = newVel * maxSpeed;
		}
	}

	void StartMovement() {
		Vector2 dir = new Vector2 (Random.Range (0, 2) > 0 ? -1f : 1f, 0f); //Random.Range (-.5f, .5f));
		dir.Normalize ();
		rb2d.velocity = dir * maxSpeed;
		ps.Play ();
	}

	void ResetPositionForNextPoint() {
		ResetBall ();
		Invoke ("StartMovement", 1);
	}

	void ResetBall() {
		rb2d.velocity = Vector2.zero;
		transform.position = Vector2.zero;
		spin = 0;
		srend.color = Color.white;
		ps.Clear ();
		ps.Stop ();
		ps.startColor = Color.white;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		//Debug.Log ("ball collided with", coll.gameObject);

		// Interactions with the paddle.
		if (coll.gameObject.CompareTag (playerTag)) {
			Vector2 vel = rb2d.velocity;
			//Debug.Log (vel.ToString());
			// Change direction based on the part of the paddle that was hit.
			float relY = Mathf.Clamp(gameObject.transform.position.y - coll.gameObject.transform.position.y, -1, 1);
			Vector2 newVel = new Vector2 (vel.x > 0 ? -1:1 * Mathf.Cos (relY * maxBounceAngleInRadians), Mathf.Sin (relY * maxBounceAngleInRadians));
		
			// Add spin.
			if (GameController.enableSpin && !Mathf.Approximately(coll.attachedRigidbody.velocity.y, 0f) && Mathf.Abs(spin) < maxSpin) {
				int spinDir = (vel.x > 0 ? 1 : -1) * (coll.attachedRigidbody.velocity.y > 0 ? -1 : 1);
				spin += spinDir;
				srend.color = Color.Lerp (Color.white, spin > 0 ? Color.cyan : Color.red, Mathf.Abs(spin) / maxSpin);
				ps.startColor = srend.color;

				Debug.Log ("add spin " + spin + " dir: " + spinDir + " color: " + srend.color.ToString());
			}

			rb2d.velocity = newVel * maxSpeed;
		}

		// Interactions with the walls
		else if (coll.gameObject.CompareTag (wallTag)) {
			Vector2 vel = rb2d.velocity;
			vel.y = -vel.y;
			rb2d.velocity = vel;
		}
	}
}
