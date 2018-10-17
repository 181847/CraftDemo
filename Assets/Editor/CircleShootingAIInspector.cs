using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CircleShootingAI))]
public class CircleShootingAIInspector : Editor {

    private const float _handleSize = 0.04f;
    private const float _pickSize = 0.06f;

    private Transform _handleTransform;
    private Quaternion _handleRotation;
    private CircleShootingAI _targ;

    private Vector3 _arcFrom;

    private void OnSceneGUI()
    {
        _targ = target as CircleShootingAI;

        _handleTransform = _targ.transform;
        _handleRotation = Tools.pivotRotation == PivotRotation.Local ? _handleTransform.rotation : Quaternion.identity;


        Vector3 circleNormal = _handleTransform.TransformDirection(Vector3.up);
        _arcFrom = _targ.ShootDirection;
        _arcFrom = Quaternion.Euler(0f, -_targ.radio * 0.5f, 0) * _arcFrom;

        // draw range
        Handles.color = Color.red;
        Handles.DrawSolidArc(_handleTransform.position, circleNormal, _arcFrom, _targ.radio, _targ.radius);

        DrawBulletHoles();
    }

    private void DrawBulletHoles()
    {
        Handles.color = Color.white;
        // draw bullet holes
        float deltaRadio = _targ.radio / (_targ.radio == 360 ? _targ.count : (_targ.count - 1));
        Quaternion deltaRotDirection = Quaternion.Euler(0, deltaRadio, 0);
        Vector3 holePosition = _arcFrom * _targ.radius;
        for (int i = 0; i < _targ.count; ++i)
        {
            Vector3 localHolePos = _handleTransform.position + holePosition;
            float handleSize = HandleUtility.GetHandleSize(localHolePos);
            Handles.Button(localHolePos, _handleRotation, _handleSize * handleSize, _pickSize * handleSize, Handles.DotHandleCap);
            holePosition = deltaRotDirection * holePosition;
        }
    }


}
