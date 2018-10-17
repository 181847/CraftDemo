using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LineSegment))]
public class LineInspector : Editor {

    private void OnSceneGUI()
    {
        LineSegment lineSeg = target as LineSegment;
        Transform lineTransform = lineSeg.transform;
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?  lineTransform.rotation : Quaternion.identity;

        Vector3 p0 = lineTransform.TransformPoint(lineSeg.p0);
        Vector3 p1 = lineTransform.TransformPoint(lineSeg.p1);
        Handles.color = Color.white;
        Handles.DrawLine(p0, p1);

        EditorGUI.BeginChangeCheck();
        p0 = Handles.DoPositionHandle(p0, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(lineSeg, "Move Point");
            EditorUtility.SetDirty(lineSeg);
            lineSeg.p0 = lineTransform.InverseTransformPoint(p0);
        }

        EditorGUI.BeginChangeCheck();
        p1 = Handles.DoPositionHandle(p1, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(lineSeg, "Move Point");
            EditorUtility.SetDirty(lineSeg);
            lineSeg.p1 = lineTransform.InverseTransformPoint(p1);
        }
    }

}
