using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * this is used to active enemy when moving platform get close.
 */
public class PlatformTrigger : MonoBehaviour {

    [SerializeField]
    private UnityEvent _triggerEvent;

    [SerializeField]
    private float _threshold;

    private bool _isTriggeredOnce;

	// Use this for initialization
	void Start () {
        _isTriggeredOnce = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (_threshold > (SceneController.RoughDistancetoMovingPlatform(transform.position) - SceneController.MaxZ)
            && ! _isTriggeredOnce)
        {
            _triggerEvent.Invoke();
            _isTriggeredOnce = true;
        }
	}

    public void AddListener(UnityAction action)
    {
        _triggerEvent.AddListener(action);
    }
    
}
