using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gun))]
public class KeepShootingAI : MonoBehaviour {

    protected Gun _gun;

    [SerializeField]
    protected Vector3 _shootDirection = Vector3.back;
    public Vector3 ShootDirection { get { return _shootDirection; } }

	// Use this for initialization
	void Start () {
        _gun = GetComponent<Gun>();
        _shootDirection.y = 0f;
        _shootDirection = _shootDirection.normalized;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShootingAllTheTime()
    {
        if (_gun.IsCoolDown)
        {
            _gun.ShootW(_shootDirection, 0);
        }
    }
}
