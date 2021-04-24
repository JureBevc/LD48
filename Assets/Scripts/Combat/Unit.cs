using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int projectilePoolSize;
    public bool isEnemy { get; set; }
    public float attackTime;

    private List<Projectile> projectiles = new List<Projectile>();
    private int nextProjectileIndex = 0;

    private float timeToAttack = 0;
    void Awake()
    {
        for (int i = 0; i < projectilePoolSize; i++)
        {
            Projectile projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
            projectiles.Add(projectile);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeToAttack += Time.deltaTime;
        if (timeToAttack >= attackTime)
        {
            timeToAttack = 0;
            Attack();
        }
    }

    public void Attack()
    {
        if (isEnemy)
            return;

        projectiles[nextProjectileIndex].Throw(transform.position, AttackTarget());
        nextProjectileIndex += 1;
        if (nextProjectileIndex >= projectilePoolSize)
        {
            nextProjectileIndex = 0;
        }
    }

    public Vector3 AttackTarget()
    {
        if (isEnemy)
        {
            return Combat.instance.playerSpawn.position;
        }
        else
        {
            return Combat.instance.enemySpawn.position;
        }
    }
}
