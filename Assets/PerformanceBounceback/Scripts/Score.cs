using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public GameManager gameManager;

	private int lastScore;

	private Text text;

	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		text = GetComponentInChildren<Text>();
		lastScore = gameManager.score;
	}
	
	void Update () {
		if (lastScore != gameManager.score) {
			lastScore = gameManager.score;
			text.text = "Score: " + gameManager.score.ToString();
		}
	}

}
