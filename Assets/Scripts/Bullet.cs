using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * bullet class, represent for any flying hit object
 */
public class Bullet : MonoBehaviour {
    public Vector3 worldDirection;
    public float speed;

    public int damage = 1;

    public PawnType _hitTargetType;
    
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(worldDirection * speed * Time.deltaTime);
    }
}
