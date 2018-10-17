using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gun))]
public class CircleShootingAI : KeepShootingAI
{
    public float radio;
    public float radius;
    public int count;

    [SerializeField]
    private int _bulletIndex = 0;

	// Use this for initialization
	void Start ()
    {
        _gun = GetComponent<Gun>();
        _shootDirection.y = 0f;
        _shootDirection = _shootDirection.normalized;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CircleShoot()
    {
        if (_gun.IsCoolDown)
        {
            _gun.ShootCircleAroundYAxis(_shootDirection, radio, radius, count, _bulletIndex);
        }
    }
}
