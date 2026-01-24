using LUP.RL;
using System.Numerics;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    private BulletData bulletData;
    private GameObject owner;
    private GameObject effectprefab;
    private int damage;
    private Transform target;

    public GameObject effects => effectprefab;
    public float ProjectileRotateSpeed = 100.0f;
    public float LifeTime = 10.0f;

    public void Init(BulletData data, GameObject Owner, int Damage, Transform Target, GameObject effect)
    {
        bulletData = data;
        owner = Owner;
        damage = Damage;
        target = Target;
        effectprefab = effect;
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        Archer player = other.GetComponent <Archer>();
        if(owner == this)
        {
            return;
        }
        else if (enemy)
        {
            enemy.TakeDamage(damage, effectprefab);
            Destroy(gameObject);
        }
        else  if(player)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }

    }

    private void Update()
    {

        if (LifeTime < 0)
        {
            Destroy(gameObject);
        }


        LifeTime -= Time.deltaTime;

        if (target == null)
        {
            transform.position += transform.forward * bulletData.Speed * Time.deltaTime;
            return;
        }
      
        UnityEngine.Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * bulletData.Speed * Time.deltaTime;
        transform.localRotation *= UnityEngine.Quaternion.Euler(ProjectileRotateSpeed * Time.deltaTime, 0f, 0f);

    }
}
