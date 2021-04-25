using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int projectilePoolSize;
    public bool isEnemy { get; set; }
    public float attackTime, enemyAttackTime, bossAttackTime;
    public int bossMaxHealth;
    private int bossHealth;
    public float attackTimeVariation;
    private List<Projectile> projectiles = new List<Projectile>();
    private int nextProjectileIndex = 0;

    private float timeToAttack = 0;
    private float nextAttackTimeVariation;

    private Animator animator;
    public GameObject playerAnimation, demon1Animation, demon2Animation, bossAnimation;
    public GameObject playerProjectile, demon1Projectile, demon2Projectile, bossProjectile;
    private GameObject chosenAnimation, chosenProjectile;

    void Awake()
    {

    }

    public void Init(bool isEnemy)
    {
        this.isEnemy = isEnemy;
        if (isEnemy)
        {
            attackTime = enemyAttackTime;
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
    }

    public void InitBoss()
    {
        this.isEnemy = true;
        projectilePoolSize = 20;
        bossHealth = bossMaxHealth;
        chosenAnimation = bossAnimation;
        chosenProjectile = bossProjectile;
        animator = Instantiate(chosenAnimation, transform).GetComponentInChildren<Animator>();
        for (int i = 0; i < projectilePoolSize; i++)
        {
            Projectile projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
            projectile.Init(chosenProjectile);
            projectile.transform.parent = Combat.instance.transform;
            projectiles.Add(projectile);
        }
        nextAttackTimeVariation = Random.Range(-1f, 1f) * attackTimeVariation;
        timeToAttack = 0;
        attackTime = bossAttackTime;
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
        if (timeToAttack >= attackTime + attackTimeVariation - Combat.instance.attackSpeedStage * Combat.instance.attackSpeedPerStage)
        {
            timeToAttack = 0;
            animator.SetTrigger("Attack");
        }
    }

    public void Attack()
    {
        AudioPlayer.instance.PlayAttack();

        if (Map.instance.currentNode.nodeType == NodeType.BOSS_BATTLE && isEnemy)
        {
            for (int i = 0; i < 5; i++)
            {
                projectiles[nextProjectileIndex].Throw(this, transform.position, AttackTarget(), Combat.instance.bossAccuracy);
                nextProjectileIndex += 1;
                if (nextProjectileIndex >= projectilePoolSize)
                {
                    nextProjectileIndex = 0;
                }
            }
        }
        else
        {
            float accuracy = 0;
            if (isEnemy)
            {
                accuracy = Combat.instance.baseAccuracyEnemy - Combat.instance.evasionPerStage * Combat.instance.evasionStage;
            }
            else
            {
                accuracy = Combat.instance.baseAccuracyPlayer + Combat.instance.accuracyPerStage * Combat.instance.accuracyStage;
            }
            projectiles[nextProjectileIndex].Throw(this, transform.position, AttackTarget(), accuracy);
            nextProjectileIndex += 1;
            if (nextProjectileIndex >= projectilePoolSize)
            {
                nextProjectileIndex = 0;
            }
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

    public void DamageBoss()
    {
        bossHealth -= 1;
        GetComponentInChildren<SpriteRenderer>().transform.Find("Canvas/HPFill").GetComponent<Image>().fillAmount = bossHealth * 1.0f / bossMaxHealth;
        if (bossHealth <= 0)
            Kill();
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
}
