using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DestoriableTarget))]
public class CraftController : MonoBehaviour {

    public float speed;

    private DestoriableTarget _playerHealth;
    private Gun _gun;
    private CircleShootingAI _circleShootHelper;

	// Use this for initialization
	void Start () {

        // initialize shooter
        _gun = GetComponent<Gun>();
        _circleShootHelper = GetComponent<CircleShootingAI>();

        _playerHealth = GetComponent<DestoriableTarget>();

        StartCoroutine(DelayUpdateLifeInfo());
    }

    public IEnumerator DelayUpdateLifeInfo()
    {
        yield return new WaitForSeconds(0.1f);
        var destroyComponent = GetComponent<DestoriableTarget>();
        Messenger<int>.Broadcast(GameEvent.PLAYER_LIFE_CHANGE, destroyComponent.Health);
    }
	
	// Update is called once per frame
	void Update () {
        if (_playerHealth.IsAlive)
        {
            UpdateMovement();
            ShootingActions();
        }
	}

    private void UpdateMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (vertical != 0 || horizontal != 0)
        {
            horizontal = horizontal * Time.deltaTime * speed + transform.localPosition.x;
            vertical = vertical * Time.deltaTime * speed + transform.localPosition.z;

            horizontal = Mathf.Clamp(horizontal, SceneController.MinX, SceneController.MaxX);
            vertical = Mathf.Clamp(vertical, SceneController.MinZ, SceneController.MaxZ);

            transform.localPosition = new Vector3(horizontal, transform.localPosition.y, vertical);
        }
    }

    private void ShootingActions()
    {
        if (Input.GetMouseButton(0) && _gun.IsCoolDown)
        {
            Vector3 bulletDirection = transform.TransformDirection(Vector3.forward);
            _gun.ShootW(bulletDirection);
        }
        else if (Input.GetMouseButton(1) && _gun.IsCoolDown)
        {
            _circleShootHelper.CircleShoot();
        }
    }

    //private void OnGUI()
    //{
    //    GUI.TextArea(new Rect(0, 0, 500, 100), "minX = " + SceneController.MinX + "  maxX = " + SceneController.MaxX + "  minY = " + SceneController.MinZ + "  maxY = " + SceneController.MaxZ);
    //}

    public void OnLifeChanged(int rest)
    {
        Messenger<int>.Broadcast(GameEvent.PLAYER_LIFE_CHANGE, rest);
    }

}
