
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public static Combat instance;

    public GameObject unitParent;
    public GameObject unitPrefab;
    public Transform playerSpawn, enemySpawn;
    public GameObject gameOver, gameWon;
    public float unitHorizontalOffset = 1f;

    public int startingPlayerUnits;

    private int numberOfPlayerUnits = 12, numberOfEnemyUnits = 3;

    private List<Unit> playerUnits = new List<Unit>();
    private List<Unit> enemyUnits = new List<Unit>();

    public bool combatActive { get; set; }

    public int startingCoins;
    public int collectedCoins { get; set; }
    public TextMeshProUGUI moneyText, unitText;

    // Enemy balance
    [Header("Combat balance")]
    public int normalEnemiesBase, normalEnemiesPerLevel;
    public int hardEnemiesBase, hardEnemiesPerLevel;
    public float baseAccuracyPlayer, baseAccuracyEnemy, bossAccuracy;

    // Power ups
    [Header("Powerups")]
    public float accuracyPerStage, evasionPerStage, attackSpeedPerStage, holyInterventionEnemyRatio, divineBlessingAccuracyBonus;
    public int accuracyStage { get; set; }
    public int evasionStage { get; set; }
    public int attackSpeedStage { get; set; }
    public bool divineBlessing { get; set; }
    public bool divineJudgement { get; set; }
    public bool holyIntervention { get; set; }


    public Combat()
    {
        instance = this;
    }

    private void Awake()
    {
        collectedCoins = startingCoins;
        UpdateMoneyText();
        numberOfPlayerUnits = startingPlayerUnits;
        for (int i = 0; i < numberOfPlayerUnits; i++)
        {
            CreatePlayerUnit();
        }
        gameObject.SetActive(false);
    }
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartCombat(int n)
    {
        numberOfEnemyUnits = n;
        if (holyIntervention && Map.instance.currentNode.nodeType != NodeType.BOSS_BATTLE)
        {
            numberOfEnemyUnits = Mathf.RoundToInt(n * holyInterventionEnemyRatio);
        }

        if (Map.instance.currentNode.nodeType == NodeType.BOSS_BATTLE)
        {
            CreateBossUnit();
        }
        else
        {
            for (int i = 0; i < numberOfEnemyUnits; i++)
            {
                CreateEnemyUnit();
            }
        }

        combatActive = true;
    }

    public void CreatePlayerUnit()
    {
        int countDirection = ((playerUnits.Count + 1) / 2) * (playerUnits.Count % 2 == 0 ? 1 : -1);
        Unit unit = Instantiate(unitPrefab, playerSpawn.transform.position + Vector3.up * Random.Range(-0.4f, 0.4f) + Vector3.right * unitHorizontalOffset * countDirection, Quaternion.identity).GetComponent<Unit>();
        unit.Init(false);
        unit.transform.parent = unitParent.transform;
        playerUnits.Add(unit);
        UpdateUnitText();
    }

    private void CreateEnemyUnit()
    {
        int countDirection = ((enemyUnits.Count + 1) / 2) * (enemyUnits.Count % 2 == 0 ? 1 : -1);
        Unit unit = Instantiate(unitPrefab, enemySpawn.transform.position + Vector3.up * Random.Range(-0.4f, 0.4f) + Vector3.right * unitHorizontalOffset * countDirection, Quaternion.identity).GetComponent<Unit>();
        unit.Init(true);
        unit.transform.parent = unitParent.transform;
        enemyUnits.Add(unit);
    }

    private void CreateBossUnit()
    {
        int countDirection = ((enemyUnits.Count + 1) / 2) * (enemyUnits.Count % 2 == 0 ? 1 : -1);
        Unit unit = Instantiate(unitPrefab, enemySpawn.transform.position + Vector3.up * Random.Range(-0.4f, 0.4f) + Vector3.right * unitHorizontalOffset * countDirection, Quaternion.identity).GetComponent<Unit>();
        unit.InitBoss();
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

        if (Map.instance.currentNode.nodeType == NodeType.BOSS_BATTLE)
        {
            enemyUnits[Random.Range(0, enemyUnits.Count)].DamageBoss();
        }
        else
        {
            enemyUnits[Random.Range(0, enemyUnits.Count)].Kill();
        }
        collectedCoins += 1;
        UpdateMoneyText();
        CheckForCombatOver();
    }

    public void DamagePlayer()
    {
        if (playerUnits.Count <= 0)
            return;
        playerUnits[Random.Range(0, playerUnits.Count)].Kill();
        UpdateUnitText();
        CheckForCombatOver();
    }

    public void KillUnits(int n)
    {
        if (playerUnits.Count <= 0)
            return;
        playerUnits[Random.Range(0, playerUnits.Count)].Kill();
        UpdateUnitText();
    }

    public void CheckForCombatOver()
    {
        if (playerUnits.Count == 0)
        {
            // Game over
            Debug.Log("GAME OVER");
            combatActive = false;
            SceneTransition.instance.StartTransition(new System.Action(() =>
            {
                gameObject.SetActive(false);
                gameOver.SetActive(true);
            }));
            return;
        }

        if (enemyUnits.Count == 0)
        {
            // Win battle
            //Debug.Log("BATTLE WON");
            combatActive = false;

            foreach (Unit unit in playerUnits)
            {
                unit.RemoveActiveProjectiles();
            }

            if (Map.instance.currentNode.nodeType == NodeType.BOSS_BATTLE)
            {
                SceneTransition.instance.StartTransition(new System.Action(() =>
                {
                    gameObject.SetActive(false);
                    gameWon.SetActive(true);
                }));
                return;
            }

            System.Action action = new System.Action(TransitionCallback);
            SceneTransition.instance.StartTransition(action);
            return;
        }
    }

    private void TransitionCallback()
    {
        gameObject.SetActive(false);
        Map.instance.gameObject.SetActive(true);
    }

    public void UpdateUnitText()
    {
        unitText.text = "" + playerUnits.Count;
    }

    public void UpdateMoneyText()
    {
        moneyText.text = "" + collectedCoins;
    }
}
