using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierControlPointHandle : MonoBehaviour {

    [SerializeField]
    private BezierCurve _controlledCurve;
    public int index;
	
	// Update is called once per frame
	void Update () {
        _controlledCurve.SetControlPoint(index, _controlledCurve.transform.InverseTransformPoint(transform.position));
	}
}
