using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour {

    [SerializeField]
    private CircleShootingAI _leftGun;

    [SerializeField]
    private GameObject _leftTentacle;

    [SerializeField]
    private CircleShootingAI _rightGun;

    [SerializeField]
    private GameObject _rightTentacle;

    [SerializeField]
    private CircleShootingAI _mouthGun;

    [SerializeField]
    private BezierAnimation _bezierAnimation;

    // Use this for initialization
    void Start () {
        _bezierAnimation = GetComponent<BezierAnimation>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void KillLeftTentacle()
    {
        Destroy(_leftTentacle);
    }

    public void KillRightTentacle()
    {
        Destroy(_rightTentacle);
    }

    public void LeftGunShoot()
    {
        _leftGun.CircleShoot();
    }

    public void RightGunShoot()
    {
        _rightGun.CircleShoot();
    }

    public void MouthGunShoot()
    {
        _mouthGun.CircleShoot();
    }

    public void ChangeNextAnimationToEscape()
    {
        _bezierAnimation.Recorder.currentClip.nextClip = 1;
    }

    private void OnDestroy()
    {
        SceneController.instance.ReplayGame();
    }
}
