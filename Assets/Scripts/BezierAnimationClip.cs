using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BezierAnimationClip
{
    public string name;
    public BezierCurve curve;

    /*
     * startT, endT is set by the value in the main bezier curve.
     * for example, startT = 0.5f, endT = 0.8f, means this animation clip will only walk between middle and 0.8 portion of the bezier curve.
     */
    public float startT, endT;
    public float startClipProgress;
    public bool startPlayForward;
    public float oneRoundTime;
    public float clipTime;
    public BezierAnimationClipMode mode;
    public int nextClip;
    private float _clipProgress;

    private float _elapsedTime;
    private bool _isGoingForward;
    
    private Vector3 _finalPositionW;
    private Vector3 _finalDirectionW;
    private Vector3 _finalPositionL;
    private Vector3 _finalDirectionL;

    // final position
    public Vector3 FinalPositionW { get { return _finalPositionW; } }
    public Vector3 FinalDirectionW { get { return _finalDirectionW; } }
    public Vector3 FinalPositionL { get { return _finalPositionL; } }
    public Vector3 FinalDirectionL { get { return _finalDirectionL; } }


    public void StartPlay()
    {
        _clipProgress = startClipProgress;
        _elapsedTime = 0f;
        _isGoingForward = startPlayForward;
        UpdatePosAndDir();
    }

    public bool IsEnd { get { return _elapsedTime >= clipTime; } }
    

    // Update is called once per frame
    public void Update()
    {
        _elapsedTime += Time.deltaTime;
        UpdateProgress();
        UpdatePosAndDir();
    }

    private void UpdatePosAndDir()
    {
        float mainProgress = GetMainCurveProgress();
        _finalPositionL     = curve.GetPointL(mainProgress);
        _finalDirectionL    = curve.GetDirectionL(mainProgress);
        _finalPositionW     = curve.transform.TransformPoint(_finalPositionL);
        _finalDirectionW    = curve.transform.TransformDirection(_finalDirectionL);
    }
    
    private void UpdateProgress()
    {
        // terminate progress animation when walk once, and has reached an end.
        if (mode == BezierAnimationClipMode.Once
            && (_clipProgress == 1f && _isGoingForward || _clipProgress == 0f && !_isGoingForward))
        {
            return;
        }

        _clipProgress += (_isGoingForward ? 1f : -1f) * Time.deltaTime / oneRoundTime;

        if (_isGoingForward)
        {
            if (_clipProgress > 1f)
            {
                switch (mode)
                {
                    case BezierAnimationClipMode.Loop:
                        _clipProgress = 0f;
                        break;

                    case BezierAnimationClipMode.Once:
                        _clipProgress = 1f;
                        break;

                    case BezierAnimationClipMode.PingPong:
                        _clipProgress = 1f;
                        _isGoingForward = !_isGoingForward;
                        break;
                }
            }
        }
        else
        {
            if (_clipProgress < 0f)
            {
                switch (mode)
                {
                    case BezierAnimationClipMode.Loop:
                        _clipProgress = 1f;
                        break;

                    case BezierAnimationClipMode.Once:
                        _clipProgress = 0f;
                        break;

                    case BezierAnimationClipMode.PingPong:
                        _clipProgress = 0f;
                        _isGoingForward = !_isGoingForward;
                        break;
                }
            }
        }
    }

    private float GetMainCurveProgress()
    {
        return (1 - _clipProgress) * startT + _clipProgress * endT;
    }
}
