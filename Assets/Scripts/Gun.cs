using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * any object that can shoot Bullets.
 */
public class Gun : MonoBehaviour {

    /*
     * the bullets that this Shooter can shoot.
     */
    [SerializeField] private GameObject[] presetBullets;
    
    public float requestCoolingTime;
    private float _haveCooledTime;

    [SerializeField]
    private PawnType _shootingAtType = PawnType.Enemy;

    private void Update()
    {
        if (_haveCooledTime < requestCoolingTime)
        {
            _haveCooledTime += Time.deltaTime;
        }
    }

    /*
     * is the gun cooled down from last shooting?
     * this is not necessary. this only provide a facility to help you control the duration of each shooting.
     * If you want, you can call Shoot(...) at any time without cooling down.
     */
    public bool IsCoolDown { get { return _haveCooledTime > requestCoolingTime; } }

    /*
     * shoot a bullet in desired world direction (bullet will be spawned at the center of the gameObject)
     * @param direction world direction of the bullet
     * @param bulletIndex index of the bullet that you want to shoot, start from Zero.
     */
    public void ShootW(Vector3 direction, int bulletIndex = 0)
    {
        ShootW(direction, Vector3.zero, bulletIndex);
    }

    /*
     * shooting bullet
     * @param direction world direction of the bulet
     * @param worldOffset by default, bullet start from the center of current gameObject, add worldOffset to prevent bullet hit them self.
     */
    public void ShootW(Vector3 direction, Vector3 worldOffset, int bulletIndex)
    {
        var newBullet = Instantiate(presetBullets[bulletIndex]) as GameObject;
        var bulletComponent = newBullet.GetComponent<Bullet>();

        Debug.Assert(bulletComponent != null);

        bulletComponent.transform.position = transform.position + worldOffset;
        bulletComponent.worldDirection = direction;
        bulletComponent._hitTargetType = _shootingAtType;

        // recalculate cooling time.
        _haveCooledTime = 0f;
    }

    /*
     * shot bullet around
     * @param direction main direction, bullet will be spread left and right on this direction
     * @param radio region of this circle [0, 360]
     * @param radius bullet start position to the center
     * @param count how many bullets are spawned per function call, 
     *          if the radio is less than 360, each edge of the region will have a bullet, 
     *          if radio is 360(max), bullet will be equally positioned around the circle.
     * @param bulletIndex choose the bullet type
     */
    public void ShootCircleAroundYAxis(Vector3 direction, float radio, float radius, int count, int bulletIndex)
    {
        direction = direction.normalized;
        radio = Mathf.Clamp(radio, 0f, 360f);

        Quaternion rotateDir;
        float deltaRadio = radio / (radio == 360f? count : count - 1);
        float shotRadio = -radio / 2;
        for (int i = 0; i < count; ++i)
        {
            rotateDir = Quaternion.Euler(0, shotRadio, 0);
            Vector3 shotDir = rotateDir * direction;
            Vector3 worldOffset = shotDir * radius;

            ShootW(shotDir, worldOffset, bulletIndex);

            shotRadio += deltaRadio;
        }
    }
}
