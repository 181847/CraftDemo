using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollower : MonoBehaviour {

    public BezierCurve _curve;
    public float t;
    public bool lookFowrad = true;
	
	// Update is called once per frame
	void Update () {
        Vector3 position = _curve.GetPointW(t);
        transform.position = position;
        if (lookFowrad)
        {
            transform.LookAt(position + _curve.GetDirectionW(t));
        }
	}
}
