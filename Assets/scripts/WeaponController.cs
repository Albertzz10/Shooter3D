using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform barrel;

    [Header("ammo")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private bool infiniteAmmo;

    [Header("Performance")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shootRate;
    [SerializeField] private int damage;

    private objectPool ObjectPool;
    private float lastShootTime;

    private bool isPlayer;

    private void Awake()
    {
        isPlayer = GetComponent<CharacterController>() != null;

        ObjectPool = GetComponent<objectPool>();
    }

    public bool CanShoot()
    {
        if(Time.time - lastShootTime >= shootRate)
        {
            if(currentAmmo > 0 || infiniteAmmo)
            {
                return true;
            }
        }
        return false;
    }

    public void Shoot()
    {
        //Update last shoot time
        lastShootTime = Time.time;

        //reduce the ammo
        if (!infiniteAmmo) currentAmmo--;
        
        //Get a new bullet
        GameObject bullet = ObjectPool.GetGameObject();

        //locate the bullet and set start position and rotation
        bullet.transform.position = barrel.position;
        bullet.transform.rotation = barrel.rotation;

        //asign damage to bullet
        bullet.GetComponent<BulletController>().Damage = damage;

        //give velocity to bullet
        bullet.GetComponent<Rigidbody>().linearVelocity = barrel.forward * bulletSpeed;
    }


}
