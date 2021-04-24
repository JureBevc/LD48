using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int projectilePoolSize;
    public bool isEnemy { get; set; }
    public float attackTime;
    public float attackTimeVariation;
    private List<Projectile> projectiles = new List<Projectile>();
    private int nextProjectileIndex = 0;

    private float timeToAttack = 0;
    private float nextAttackTimeVariation;
    void Awake()
    {
        for (int i = 0; i < projectilePoolSize; i++)
        {
            Projectile projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
            projectiles.Add(projectile);
        }

        nextAttackTimeVariation = Random.Range(-1f, 1f) * attackTimeVariation;
        timeToAttack = Random.Range(0, attackTime);
        SetUnitSprite();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!Combat.instance.combatActive)
            return;

        timeToAttack += Time.deltaTime;
        if (timeToAttack >= attackTime + attackTimeVariation)
        {
            timeToAttack = 0;
            Attack();
        }
    }

    public void Attack()
    {
        projectiles[nextProjectileIndex].Throw(this, transform.position, AttackTarget(), 0.1f);
        nextProjectileIndex += 1;
        if (nextProjectileIndex >= projectilePoolSize)
        {
            nextProjectileIndex = 0;
        }
        nextAttackTimeVariation = Random.Range(-1f, 1f) * attackTimeVariation;
    }

    public Vector3 AttackTarget()
    {
        if (isEnemy)
        {
            List<Unit> units = Combat.instance.GetPlayerUnits();
            return units[Random.Range(0, units.Count)].transform.position;
        }
        else
        {
            List<Unit> units = Combat.instance.GetEnemyUnits();
            return units[Random.Range(0, units.Count)].transform.position;
        }
    }

    public void Kill()
    {
        if (isEnemy)
            Combat.instance.GetEnemyUnits().Remove(this);
        else
            Combat.instance.GetPlayerUnits().Remove(this);
        Destroy(this.gameObject);
    }

    private void SetUnitSprite()
    {
        // Set the correct unit animation
    }
}
