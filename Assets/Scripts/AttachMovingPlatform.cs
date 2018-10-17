using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformTrigger))]
public class AttachMovingPlatform : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var platformTrigger = GetComponent<PlatformTrigger>();
        platformTrigger.AddListener(Attach);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Attach()
    {
        SceneController.AttachToPlatform(gameObject);
    }
}
