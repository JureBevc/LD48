using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public static Combat instance;

    public GameObject unitPrefab, playerSpawn, enemySpawn;
    public float unitHorizontalOffset = 1f;

    private int numberOfPlayerUnits = 5, numberOfEnemyUnits = 5;

    private List<Unit> playerUnits = new List<Unit>();
    private List<Unit> enemyUnits = new List<Unit>();

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
    }

    private void CreatePlayerUnit()
    {
        int countDirection = ((playerUnits.Count + 1) / 2) * (playerUnits.Count % 2 == 0 ? 1 : -1);
        Unit unit = Instantiate(unitPrefab, playerSpawn.transform.position + Vector3.right * unitHorizontalOffset * countDirection, Quaternion.identity).GetComponent<Unit>();
        unit.isEnemy = false;
        playerUnits.Add(unit);
    }

    private void CreateEnemyUnit()
    {
        int countDirection = ((enemyUnits.Count + 1) / 2) * (enemyUnits.Count % 2 == 0 ? 1 : -1);
        Unit unit = Instantiate(unitPrefab, enemySpawn.transform.position + Vector3.right * unitHorizontalOffset * countDirection, Quaternion.identity).GetComponent<Unit>();
        unit.isEnemy = true;
        enemyUnits.Add(unit);
    }
}
