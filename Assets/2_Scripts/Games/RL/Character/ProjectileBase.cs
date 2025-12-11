using LUP.RL;
using System.Numerics;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    private BulletData bulletData;
    private GameObject owner;
    private int damage;
    private Transform target;

    public float ProjectileRotateSpeed = 100.0f;

    public void Init(BulletData data, GameObject Owner, int Damage, Transform Target)
    {
        bulletData = data;
        owner = Owner;
        damage = Damage;
        target = Target;
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        Archer player = other.GetComponent <Archer>();

        if (enemy)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        else  if(player)
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        if (target == null)
        {
            transform.position += transform.forward * bulletData.Speed * Time.deltaTime;
            return;
        }
      
        UnityEngine.Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * bulletData.Speed * Time.deltaTime;

        //UnityEngine.Vector3 weaponAngle = transform.localEulerAngles;
        //weaponAngle.x += ProjectileRotateSpeed * Time.deltaTime;
        //transform.localEulerAngles = weaponAngle;

        transform.localRotation *= UnityEngine.Quaternion.Euler(ProjectileRotateSpeed * Time.deltaTime, 0f, 0f);
    }
}
