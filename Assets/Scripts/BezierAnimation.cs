using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierAnimation : MonoBehaviour, IActiveHandle {

    [SerializeField]
    private BezierAnimator _animator;

    private BezierAnimationRecorder _recorder;
    public BezierAnimationRecorder Recorder { get { return _recorder; } }

    [SerializeField]
    private bool _lookForward = true;

    [SerializeField]
    private float _delay = 0.0f;

    [SerializeField]
    private BezierAnimPosMode _posMode = BezierAnimPosMode.MatchWorldPos;

    [SerializeField]
    private Vector3 _localOffset;

    public bool canPlay;
    
	// Use this for initialization
	void Start () {
        _recorder = _animator.InstanceOneRecorder();
        if (canPlay)
        {
            StartCoroutine(DelayPlayAnimation());
        }
	}

    private IEnumerator DelayPlayAnimation()
    {
        canPlay = false;
        yield return new WaitForSeconds(_delay);
        canPlay = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (canPlay)
        {
            _animator.UpdateRecorder(_recorder);

            switch (_posMode)
            {
                case BezierAnimPosMode.MatchWorldPos:
                    transform.position = _recorder.FinalPositionW;
                    if (_lookForward)
                    {
                        transform.LookAt(transform.position + _recorder.FinalDirectionW);
                    }
                    break;

                case BezierAnimPosMode.MatchLocalPos:
                    transform.localPosition = _recorder.FinalPositionL + _localOffset;
                    if (_lookForward)
                    {
                        // transform local direction by this object.
                        Vector3 directionInWorld;
                        if (transform.parent == null)
                        {
                            // if gameObject has no parent, then treat the lcoal direction as world direction.
                            directionInWorld = _recorder.FinalDirectionL;
                        }
                        else
                        {
                            // transform local direction into the parent space.
                            directionInWorld = transform.parent.transform.TransformDirection(_recorder.FinalDirectionL);
                        }
                        transform.LookAt(transform.position + directionInWorld);
                    }
                    break;
            }
        }
    }

    public void Active()
    {
        canPlay = true;
    }

    public void Deactive()
    {
        canPlay = false;
    }
}
