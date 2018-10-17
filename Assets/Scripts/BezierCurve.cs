using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour {

    [SerializeField]
    private Vector3[] points;

    [SerializeField]
    private BezierControlPointMode[] modes; // point index: 0, 1, 2, 3, 4, 5; corresponds to modes index: 0, 0,   1, 1, 1,   2, 2

    [SerializeField]
    private bool loop = false;

    public bool Loop
    {
        get { return loop; }
        set
        {
            loop = value;
            if (loop)
            {
                modes[modes.Length - 1] = modes[0];
                SetControlPoint(ControlPointCount - 1, points[0]);
            }
        }
    }

    public int curveCount = 1;

    public void Reset()
    {
        points = new Vector3[]
        {
            new Vector3(1, 0, 0),
            new Vector3(2, 0, 0),
            new Vector3(3, 0, 0),
            new Vector3(4, 0, 0)
        };

        modes = new BezierControlPointMode[]
        {
            BezierControlPointMode.Free,
            BezierControlPointMode.Free
        };
    }

    public BezierControlPointMode GetControlPointMode(int index)
    {
        return modes[(index + 1) / 3];
    }

    public void SetControlPointMode(int index, BezierControlPointMode mode)
    {
        modes[(index + 1) / 3] = mode;

        if (loop)
        {
            if (index == 0 || index == 1)
            {
                modes[modes.Length - 1] = mode;
            }
            else if (index == ControlPointCount - 1 || index == ControlPointCount - 2)
            {
                modes[0] = mode;
            }
        }

        EnforceMode(index);
    }

    private void EnforceMode(int index)
    {
        int modeIndex = (index + 1) / 3;
        var mode = modes[modeIndex];
        if (mode == BezierControlPointMode.Free)
        {
            return;
        }
        if (loop == false
            && (index == 0 || index == 1 || index == ControlPointCount - 2 || index == ControlPointCount - 1 ))
        {
            return;
        }

        int middleIndex = modeIndex * 3;
        int enforceIndex, fixedIndex;
        if (index <= middleIndex)
        {
            fixedIndex = middleIndex - 1;
            enforceIndex = middleIndex + 1;

            // wrap around to head
            if (enforceIndex >= ControlPointCount)
            {
                enforceIndex = 1;
            }
            if (fixedIndex < 0)
            {
                fixedIndex = ControlPointCount - 2;
            }
        }
        else
        {
            fixedIndex = middleIndex + 1;
            enforceIndex = middleIndex - 1;
            
            // wrap around to tail
            if (loop && enforceIndex < 0)
            {
                enforceIndex = ControlPointCount - 2;
            }
        }

        Vector3 enforceTangent;

        // general for mirrored
        enforceTangent = points[middleIndex] - points[fixedIndex];
        
        if (mode == BezierControlPointMode.Aligned)
        {
            enforceTangent = enforceTangent.normalized * Vector3.Distance(points[middleIndex], points[enforceIndex]);
        }

        points[enforceIndex] = points[middleIndex] + enforceTangent;
    }

    public int ControlPointCount
    {
        get { return points.Length; }
    }

    public Vector3 GetControlPoint(int index)
    {
        return points[index];
    }

    public void SetControlPoint(int index, Vector3 point)
    {
        // if we modify a middle point, also move its predecessor and successor
        if (index % 3 == 0)
        {
            var delta = point - points[index];
            if (index > 0)
            {
                points[index - 1] += delta;
            }
            else if (loop && index == 0)
            {
                // if loop and move head point, also offset the second tail point.
                points[ControlPointCount - 2] += delta;
            }

            if (index < ControlPointCount - 1)
            {
                points[index + 1] += delta;
            }
            else if (loop && index == ControlPointCount - 1)
            {
                // if loop and move tail point, also offset the second head point.
                points[1] += delta;
            }
        }


        if (loop && (index == 0 || index == ControlPointCount - 1))
        {
            points[0] = points[ControlPointCount - 1] = point;
        }
        else
        {
            points[index] = point;
        }

        EnforceMode(index);
    }
    
    /*
     * map global t into specific curve
     * @param ref t, pass in global t, and return the local t in specific curve.
     * @param out i, return the start index of the four points that consist the required bezier curve.
     */
    private void FixT(ref float t, out int i)
    {
        i = 0;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = t * curveCount;
            i = (int)t;
            t = t - i;
            i *= 3;
        }
    }

    public Vector3 GetPointL(float t)
    {
        int i;
        FixT(ref t, out i);
        return Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t);
    }

    public Vector3 GetPointW(float t)
    {
        return transform.TransformPoint(GetPointL(t));
    }

    public Vector3 GetVelocityL(float t)
    {
        int i = 0;
        FixT(ref t, out i);
        return Bezier.GetFirstDerivate(points[i], points[i + 1], points[i + 2], points[i + 3], t);
    }

    public Vector3 GetVelocityW(float t)
    {
        return transform.TransformDirection(GetVelocityL(t));
    }

    public Vector3 GetDirectionW(float t)
    {
        return GetVelocityW(t).normalized;
    }

    public Vector3 GetDirectionL(float t)
    {
        return GetVelocityL(t).normalized;
    }

    public void AddCurve()
    {
        Vector3 newPoint = points[points.Length - 1];
        Array.Resize(ref points, points.Length + 3);
        newPoint.x += 1f;
        points[points.Length - 3] = newPoint;
        newPoint.x += 1f;
        points[points.Length - 2] = newPoint;
        newPoint.x += 1f;
        points[points.Length - 1] = newPoint;

        Array.Resize(ref modes, modes.Length + 1);
        modes[modes.Length - 1] = modes[modes.Length - 2];

        EnforceMode(ControlPointCount - 4);

        if (loop)
        {
            points[ControlPointCount - 1] = points[0];
            modes[modes.Length - 1] = modes[0];
            EnforceMode(0);
        }

        ++curveCount;
    }

    public void RemoveCurve()
    {
        if (curveCount > 1)
        {
            Array.Resize(ref points, points.Length - 3);
            Array.Resize(ref modes, modes.Length - 1);

            if (loop)
            {
                points[ControlPointCount - 1] = points[0];
                modes[modes.Length - 1] = modes[0];
                EnforceMode(0);
            }

            --curveCount;
        }
    }
}

public static class Bezier
{
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;

        return oneMinusT * oneMinusT * p0 + 2f * oneMinusT * t * p1 + t * t * p2;
    }

    public static Vector3 GetFirstDerivate(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return 2f * (1f - t) * (p1 - p0) +
               2f * t * (p2 - p1);
    }

    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * oneMinusT * p0
            + 3f * oneMinusT * oneMinusT * t * p1
            + 3f * oneMinusT * t * t * p2
            + t * t * t * p3;
    }

    public static Vector3 GetFirstDerivate(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        float omtMtM2 = oneMinusT * t * 2f;
        return 
            - 3f * oneMinusT * oneMinusT * p0
            + 3f * (- omtMtM2 + oneMinusT * oneMinusT) * p1
            + 3f * (- t * t + omtMtM2) * p2
            + 3f * t * t * p3;
    }
}