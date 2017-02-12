﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreScript : MonoBehaviour {

    public CharacterControllerScript CharacterController;

    private Text scoreText;

	// Use this for initialization
	void Start () {
        scoreText = GetComponentInParent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = CharacterController.PodsPickedUp.ToString();
	}
}
