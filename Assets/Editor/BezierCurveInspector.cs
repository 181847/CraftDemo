using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor {

    private BezierCurve _curve;
    private Transform   _handleTransform;
    private Quaternion  _handleRotation;

    private const int _lineSteps = 10;
    private const float _velocityLen = 1.5f;

    private const float _handleSize = 0.04f;
    private const float _pickSize = 0.06f;

    private int _selectedIndex;

    private static Color[] modeColors =
    {
        Color.white,
        Color.yellow,
        Color.blue
    };

    private void OnSceneGUI()
    {
        _curve = target as BezierCurve;
        _handleTransform = _curve.transform;
        _handleRotation = Tools.pivotRotation == PivotRotation.Local ? _handleTransform.rotation : Quaternion.identity;
        

        Vector3 p0 = ShowPoint(0);

        for (int i = 1; i < _curve.ControlPointCount; i += 3)
        {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);
            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);
            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            p0 = p3;
        }
        ShowDirections();
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        _curve = target as BezierCurve;
        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Loop", _curve.Loop);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_curve, "Toggle Loop");
            _curve.Loop = loop;
            EditorUtility.SetDirty(_curve);
        }

        if (_selectedIndex >= 0 && _selectedIndex < _curve.ControlPointCount)
        {
            DrawSelectedPointInspector();
        }

        _curve = target as BezierCurve;
        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(_curve, "Add Curve");
            _curve.AddCurve();
            EditorUtility.SetDirty(_curve);
        }

        if (GUILayout.Button("Remove Curve"))
        {
            Undo.RecordObject(_curve, "Remove Curve");
            _curve.RemoveCurve();
            EditorUtility.SetDirty(_curve);
        }

        if (GUILayout.Button("Reset Start Point"))
        {
            Undo.RecordObject(_curve, "Remove Curve");
            var origin = _curve.gameObject.transform.position;
            _curve.SetControlPoint(0, _handleTransform.InverseTransformPoint(origin));
            EditorUtility.SetDirty(_curve);
        }

    }

    private void DrawSelectedPointInspector()
    {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        int newIndex = EditorGUILayout.IntField("selected index", _selectedIndex);
        if (EditorGUI.EndChangeCheck() && newIndex >= 0 && newIndex <= _curve.ControlPointCount - 1)
        {
            _selectedIndex = newIndex;
        }

        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", _curve.GetControlPoint(_selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_curve, "Move Point");
            EditorUtility.SetDirty(_curve);
            _curve.SetControlPoint(_selectedIndex, point);
        }

        EditorGUI.BeginChangeCheck();
        var mode = (BezierControlPointMode)EditorGUILayout.EnumPopup("Mode", _curve.GetControlPointMode(_selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_curve, "Change Point Mode");
            _curve.SetControlPointMode(_selectedIndex, mode);
            EditorUtility.SetDirty(_curve);
        }
    }

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = _handleTransform.TransformPoint(_curve.GetControlPoint(index));

        float handleSize = HandleUtility.GetHandleSize(point);
        Handles.color = modeColors[(int)_curve.GetControlPointMode(index)];
        if (index == 0)
        {
            Handles.color = Color.red;
            handleSize *= 2.0f;
        }
        else if (index == _curve.ControlPointCount - 1)
        {
            Handles.color = Color.green;
        }
        if (Handles.Button(point, _handleRotation, handleSize * _handleSize, handleSize * _pickSize,  Handles.DotHandleCap ))
        {
            _selectedIndex = index;
            Repaint();
        }

        if (_selectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, _handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_curve, "Move Point");
                EditorUtility.SetDirty(_curve);
                _curve.SetControlPoint(index, _handleTransform.InverseTransformPoint(point));
            }
        }
        
        return point;
    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        float t = 0.0f;
        Vector3 point, velocity;
        int steps = _lineSteps * _curve.curveCount;
        for (int i = 0; i <= steps; ++i)
        {
            t = i / (float)steps;
            Handles.color = Color.green;
            point = _curve.GetPointW(t);
            velocity = _curve.GetDirectionW(t);
            Handles.DrawLine(point, point + _velocityLen * velocity);
        }
    }

}
