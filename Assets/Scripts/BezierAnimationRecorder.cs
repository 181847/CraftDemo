using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BezierAnimationRecorder {

    public BezierAnimationClip currentClip; // record current clip
    
    public Vector3 FinalPositionW { get { return currentClip.FinalPositionW; } }
    public Vector3 FinalDirectionW { get { return currentClip.FinalDirectionW; } }
    public Vector3 FinalPositionL { get { return currentClip.FinalPositionL; } }
    public Vector3 FinalDirectionL { get { return currentClip.FinalDirectionL; } }

}
