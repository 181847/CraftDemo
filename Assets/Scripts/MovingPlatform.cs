using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * MovingPlatform is used for platform movement only in positive y direction.
 */
public class MovingPlatform : MonoBehaviour {

    public float speed;
    public bool canMove = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (canMove)
        {
            transform.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
        }
    }

    public void StopMove()
    {
        canMove = false;
    }
}
