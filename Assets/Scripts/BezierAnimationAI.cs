using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/*
 * this kind of AI require host have BezierAnimation component,
 * using AnimationClips in that component to switch different actions(except motion which is controlled by the Animation).
 */
[RequireComponent(typeof(BezierAnimation))]
public class BezierAnimationAI : MonoBehaviour, IActiveHandle {

    private BezierAnimation _animation;

    [SerializeField]
    private NamedEvent[] _namedEvents;

    private Dictionary<string, UnityEvent> _eventDict = new Dictionary<string, UnityEvent>();
    private int _lastClipNameHasCode;
    private UnityEvent _lastEvent;

    public bool canAct = true;

    // Use this for initialization
    void Start () {
        // initialize events
        _eventDict.Clear();
        foreach (var namedEvent in _namedEvents)
        {
            _eventDict.Add(namedEvent.ClipName, namedEvent.UpdateEvent);
        }

        _animation = GetComponent<BezierAnimation>();

        if (_animation.Recorder != null)
        {
            // setup first event
            string clipName = _animation.Recorder.currentClip.name;
            _lastClipNameHasCode = clipName.GetHashCode();
            if (_eventDict.ContainsKey(clipName))
            {
                _lastEvent = _eventDict[clipName];
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (canAct)
        {
            string clipName = _animation.Recorder.currentClip.name;
            int nameHashCode = clipName.GetHashCode();

            if (nameHashCode != _lastClipNameHasCode || _lastEvent == null)
            {
                _lastClipNameHasCode = nameHashCode;
                if (_eventDict.ContainsKey(clipName))
                {
                    _lastEvent = _eventDict[clipName];
                }
                else
                {
                    _lastEvent = null;
                }
            }

            if (_lastEvent != null)
            {
                _lastEvent.Invoke();
            }
        }
    }
    
    public void Active()
    {
        canAct = true;
    }

    public void Deactive()
    {
        canAct = false;
    }
}

