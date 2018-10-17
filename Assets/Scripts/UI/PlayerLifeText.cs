using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeText : MonoBehaviour {

    private Text _text;

	// Use this for initialization
	void Start () {
        _text = GetComponent<Text>();
        Messenger<int>.AddListener(GameEvent.PLAYER_LIFE_CHANGE, OnPlayerLifeChanged);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPlayerLifeChanged(int rest)
    {
        _text.text = "PlayerLife: " + rest;
    }
}
