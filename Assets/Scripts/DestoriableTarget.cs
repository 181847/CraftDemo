using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestoriableTarget : MonoBehaviour {

    [SerializeField]
    private int _health = 10;
    public int Health { get { return _health; } }

    [SerializeField]
    private PawnType _pawnType = PawnType.Enemy;

    [SerializeField]
    private UnityEvent _onDestory;

    [SerializeField]
    private float _delayDestory = 0f;

    public bool IsAlive { get { return _health > 0; } }

    private void OnCollisionEnter(Collision collision)
    {
        if (_health < 0)
        {
            return;
        }

        if (_pawnType == PawnType.Resist)
        {
            return;
        }

        var bullet = collision.collider.GetComponent<Bullet>();
        if (bullet != null && bullet._hitTargetType == _pawnType)
        {
            _health -= bullet.damage;
            if (_pawnType == PawnType.Player)
            {
                var player = GetComponent<CraftController>();
                if (player != null)
                {
                    player.OnLifeChanged(_health);
                }
            }
            Destroy(bullet.gameObject);
            Debug.Log("hp -1, rest = " + _health.ToString());
        }

        HealthCheck();
    }

    private void HealthCheck()
    {
        if (_health <= 0)
        {
            _health = 0;
            switch(_pawnType)
            {
                case PawnType.Enemy:
                    StartCoroutine(DelayDestroy());
                    _onDestory.Invoke();
                    break;

                case PawnType.Player:
                    if (SceneController.instance != null)
                    {
                        SceneController.instance.ReplayGame();
                    }
                    _onDestory.Invoke();
                    break;
            }
        }
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(_delayDestory);
        Destroy(gameObject);
    }
}
