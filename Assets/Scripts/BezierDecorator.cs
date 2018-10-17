using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierDecorator : MonoBehaviour {

    [SerializeField]
    private BezierCurve _curve;

    [SerializeField]
    private BezierFollower[] _parts;

    public int frequency;
    public bool lookForward;

    private void Awake()
    {
        if (frequency <= 0 || _parts == null || _parts.Length == 0)
        {
            return;
        }


        int countParts = frequency * _parts.Length;
        float stepSize;
        if (_curve.Loop || countParts == 1)
        {
            stepSize = 1f / countParts;
        }
        else
        {
            stepSize = 1f / (countParts - 1);
        }

        for (int i = 0; i < countParts; ++i)
        {
            BezierFollower part = Instantiate(_parts[i % _parts.Length]) as BezierFollower;
            float t = i * stepSize;
            Vector3 position = _curve.GetPointW(t);
            part._curve = _curve;
            part.t = t;
            part.transform.position = position;
            if (lookForward)
            {
                part.transform.LookAt(position + _curve.GetDirectionW(t));
            }
            part.transform.parent = transform;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
