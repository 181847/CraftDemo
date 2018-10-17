using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    [SerializeField]
    private GameObject movingPlatform;
    
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _backgroundTransform;

    private static SceneController _instance;
    public static SceneController instance { get { return _instance; } }

    /*
     * this value is used to compute craft movement region, 
     * see more details in Start().
     * in general, this should be a negative value, and less it is,
     * the small region the craft can be in.
     */
    public float backgroundDepthOffset;
    private static float _minX, _maxX, _minZ, _maxZ;
    public static float MinX { get { return _minX; } }
    public static float MaxX { get { return _maxX; } }
    public static float MinZ { get { return _minZ; } }
    public static float MaxZ { get { return _maxZ; } }

    private static GameObject _sMovingPlatform;
    
    /*
     * using SceneManger to figure out whether the position is closed to moving platform,
     * this distance is not accurate, just Z distane in world position.
     */ 
    public static float RoughDistancetoMovingPlatform(Vector3 compareToPosW)
    {
        if (_sMovingPlatform == null)
        {
            return float.MaxValue;
        }
        else
        {
            return Mathf.Abs(_sMovingPlatform.transform.position.z - compareToPosW.z);
        }
    }

	// Use this for initialization
	void Start () {
        _instance = this;

        Debug.Assert(movingPlatform != null);
        _sMovingPlatform = movingPlatform;

        // compute moving region of player.
        Vector3 camPos = _camera.transform.position;
        float depthInCamera = backgroundDepthOffset + camPos.y - _backgroundTransform.position.y;
        Vector3 leftBottom = new Vector3(0, 0, depthInCamera);
        Vector3 leftBottomW = _camera.ScreenToWorldPoint(leftBottom);
        _minX = leftBottomW.x - camPos.x;
        _maxX = -_minX;
        _minZ = leftBottomW.z - camPos.z;
        _maxZ = -_minZ;
    }

    public static void AttachToPlatform(GameObject childObject)
    {
        if (_sMovingPlatform != null)
        {
            childObject.transform.parent = _sMovingPlatform.transform;
        }
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
