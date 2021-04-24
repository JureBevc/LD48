using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public static Combat instance;

    public GameObject unitParent;
    public GameObject unitPrefab;
    public Transform playerSpawn, enemySpawn;
    public float unitHorizontalOffset = 1f;

    private int numberOfPlayerUnits = 5, numberOfEnemyUnits = 6;

    private List<Unit> playerUnits = new List<Unit>();
    private List<Unit> enemyUnits = new List<Unit>();

    public bool combatActive { get; set; }

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCombat();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartCombat()
    {
        for (int i = 0; i < numberOfPlayerUnits; i++)
        {
            CreatePlayerUnit();
        }
        for (int i = 0; i < numberOfEnemyUnits; i++)
        {
            CreateEnemyUnit();
        }
        combatActive = true;
    }

    private void CreatePlayerUnit()
    {
        int countDirection = ((playerUnits.Count + 1) / 2) * (playerUnits.Count % 2 == 0 ? 1 : -1);
        Unit unit = Instantiate(unitPrefab, playerSpawn.transform.position + Vector3.up * Random.Range(-0.4f, 0.4f) + Vector3.right * unitHorizontalOffset * countDirection, Quaternion.identity).GetComponent<Unit>();
        unit.isEnemy = false;
        unit.transform.parent = unitParent.transform;
        playerUnits.Add(unit);
    }

    private void CreateEnemyUnit()
    {
        int countDirection = ((enemyUnits.Count + 1) / 2) * (enemyUnits.Count % 2 == 0 ? 1 : -1);
        Unit unit = Instantiate(unitPrefab, enemySpawn.transform.position + Vector3.up * Random.Range(-0.4f, 0.4f) + Vector3.right * unitHorizontalOffset * countDirection, Quaternion.identity).GetComponent<Unit>();
        unit.isEnemy = true;
        unit.transform.parent = unitParent.transform;
        enemyUnits.Add(unit);
    }

    public List<Unit> GetPlayerUnits()
    {
        return playerUnits;
    }
    public List<Unit> GetEnemyUnits()
    {
        return enemyUnits;
    }

    public void DamageEnemy()
    {
        if (enemyUnits.Count <= 0)
            return;
        enemyUnits[Random.Range(0, enemyUnits.Count)].Kill();
        CheckForCombatOver();
    }

    public void DamagePlayer()
    {
        if (playerUnits.Count <= 0)
            return;
        playerUnits[Random.Range(0, playerUnits.Count)].Kill();
        CheckForCombatOver();
    }

    public void CheckForCombatOver()
    {
        if (playerUnits.Count == 0)
        {
            // Game over
            Debug.Log("GAME OVER");
            combatActive = false;
            return;
        }

        if (enemyUnits.Count == 0)
        {
            // Win battle
            Debug.Log("BATTLE WON");
            combatActive = false;
            return;
        }
    }
}
