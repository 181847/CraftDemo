using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * used to output a animated position and direction component.
 */
public class BezierAnimator : MonoBehaviour {
    [SerializeField]
    private BezierAnimationClip[] clips;

    [SerializeField]
    private int entryIndex;

    // start one animation instance by return a recorder
    public BezierAnimationRecorder InstanceOneRecorder()
    {
        var recorder = new BezierAnimationRecorder();
        recorder.currentClip = clips[entryIndex];
        recorder.currentClip.StartPlay();
        return recorder;
    }
	
	// Update single Recorder
	public void UpdateRecorder(BezierAnimationRecorder recorder) {
        recorder.currentClip.Update();

        if (recorder.currentClip.IsEnd)
        {
            recorder.currentClip = clips[recorder.currentClip.nextClip];
            recorder.currentClip.StartPlay();
        }
    }
}
