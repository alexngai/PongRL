using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static bool enableSpin = true;

	public static int playerScoreL = 0;
	public static int playerScoreR = 0;

	public const int winScore = 10;
	public GUISkin layout;

	static GameObject ball;

	private static bool isStarted = false;

	// Use this for initialization
	void Start () {
		ball = GameObject.FindGameObjectWithTag ("Ball");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void Score (string wallID) {
		if (wallID.Equals ("RightBound")) {
			playerScoreL++;
		} else {
			playerScoreR++;
		}

		if (playerScoreL >= winScore || playerScoreR >= winScore) {
			// Maintain score values and display reset game button.
			isStarted = false;
			ball.SendMessage ("ResetBall", 0f, SendMessageOptions.RequireReceiver);
		} else {
			// Reset ball for next point.
			ball.SendMessage ("ResetPositionForNextPoint", 0f, SendMessageOptions.RequireReceiver);
		}
	}

	void OnGUI () {
		GUI.skin = layout;
		GUI.Label (new Rect (Screen.width / 2 - 150 - 10, 20, 200, 200), "" + playerScoreL);
		GUI.Label (new Rect (Screen.width / 2 + 150 + 10, 20, 200, 200), "" + playerScoreR);

		if (!isStarted && GUI.Button (new Rect (Screen.width / 2 - 60, 35, 120, 53), "START")) {
			playerScoreL = 0;
			playerScoreR = 0;
			isStarted = true;
			ball.SendMessage ("ResetPositionForNextPoint", 0.5f, SendMessageOptions.RequireReceiver);
		}
	}
}
