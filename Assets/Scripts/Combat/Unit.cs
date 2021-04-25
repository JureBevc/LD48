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

    private Animator animator;
    public GameObject playerAnimation, demon1Animation, demon2Animation;
    public GameObject playerProjectile, demon1Projectile, demon2Projectile;
    private GameObject chosenAnimation, chosenProjectile;
    void Awake()
    {

    }

    public void Init(bool isEnemy)
    {
        this.isEnemy = isEnemy;
        if (isEnemy)
        {
            if (Random.Range(0f, 1f) > 0.5f)
            {
                chosenAnimation = demon1Animation;
                chosenProjectile = demon1Projectile;
            }
            else
            {
                chosenAnimation = demon2Animation;
                chosenProjectile = demon2Projectile;
            }
            animator = Instantiate(chosenAnimation, transform).GetComponentInChildren<Animator>();
        }
        else
        {
            chosenAnimation = playerAnimation;
            chosenProjectile = playerProjectile;
            animator = Instantiate(chosenAnimation, transform).GetComponentInChildren<Animator>();
        }
        for (int i = 0; i < projectilePoolSize; i++)
        {
            Projectile projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
            projectile.Init(chosenProjectile);
            projectile.transform.parent = Combat.instance.transform;
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
            animator.SetTrigger("Attack");
        }
    }

    public void Attack()
    {
        AudioPlayer.instance.PlayAttack();
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

        AudioPlayer.instance.PlayHit();
        foreach (Projectile p in projectiles)
        {
            Destroy(p.gameObject);
        }
        Destroy(this.gameObject);
    }

    public void RemoveActiveProjectiles()
    {
        foreach (Projectile p in projectiles)
        {
            p.ResetProjectile();
        }
    }

    private void SetUnitSprite()
    {
        // Set the correct unit animation
    }
}
