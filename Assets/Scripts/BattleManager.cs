using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform[] partySpawnPoints;
    [SerializeField] private Transform[] enemySpawnPoints;

    private PartyManager partyManager;
    private EnemyManager enemyManager;

    [Header("Battlers")]
    [SerializeField] private List<BattleEntities> allBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> playerBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> enemyBattlers = new List<BattleEntities>();

    // Start is called before the first frame update
    void Start()
    {
        partyManager = GameObject.FindFirstObjectByType<PartyManager>();
        enemyManager = GameObject.FindFirstObjectByType<EnemyManager>();
    
        CreatePartyEntities();
        CreateEnemyEntities();
    }

    private void CreatePartyEntities()
    {
        List<PartyMember> currentParty = new List<PartyMember>();
        currentParty = partyManager.GetCurrentParty();

        for (int i = 0; i < currentParty.Count; i++)
        {
            BattleEntities tempEntity = new BattleEntities();
            tempEntity.SetEntityValues(currentParty[i].MemberName, currentParty[i].Level,
                currentParty[i].CurrentHealth, currentParty[i].MaxHealth,
                currentParty[i].CurrentEnergy, currentParty[i].MaxEnergy,
                currentParty[i].Strength, currentParty[i].Smarts,
                currentParty[i].Initiative, true
            );

            PlayerBattleVisuals tempBattleVisuals = Instantiate(currentParty[i].MemberBattleVisualPrefab,
            partySpawnPoints[i].position, Quaternion.identity).GetComponent<PlayerBattleVisuals>();
            
            Debug.Log("Goose");
            tempBattleVisuals.SetStartingValues(currentParty[i].CurrentHealth, currentParty[i].MaxHealth, currentParty[i].CurrentEnergy, currentParty[i].MaxEnergy, currentParty[i].Level);
            Debug.Log("Geese");
            tempEntity.battleVisualsForPlayer = tempBattleVisuals;
            Debug.Log("Geese");

            allBattlers.Add(tempEntity);
            playerBattlers.Add(tempEntity);
        }
    }

    private void CreateEnemyEntities()
    {
        List<Enemy> currentEnemies = new List<Enemy>();
        currentEnemies = enemyManager.GetCurrentEnemies();

        for (int i = 0; i < currentEnemies.Count; i++)
        {
            BattleEntities tempEntity = new BattleEntities();
            tempEntity.SetEntityValues(currentEnemies[i].EnemyName, currentEnemies[i].Level,
                currentEnemies[i].CurrentHealth, currentEnemies[i].MaxHealth,
                currentEnemies[i].CurrentEnergy, currentEnemies[i].MaxEnergy,
                currentEnemies[i].Strength, currentEnemies[i].Smarts,
                currentEnemies[i].Initiative, false
            );

            EnemyBattleVisuals tempBattleVisuals = Instantiate(currentEnemies[i].EnemyVisualPrefab, enemySpawnPoints[i].position, Quaternion.identity).GetComponent<EnemyBattleVisuals>();
            
            tempBattleVisuals.SetStartingValues(currentEnemies[i].CurrentHealth, currentEnemies[i].MaxHealth, currentEnemies[i].Level);
            tempEntity.battleVisualsForEnemy = tempBattleVisuals;

            allBattlers.Add(tempEntity);
            enemyBattlers.Add(tempEntity);
        }
    }
}

[System.Serializable]
public class BattleEntities
{
    public string Name;
    public int Level;
    public int CurrHealth;
    public int MaxHealth;
    public int CurrentEnergy;
    public int MaxEnergy;
    public int Strength;
    public int Smarts;
    public int Initiative;
    // public int CurrentExp;
    // public int MaxExp;

    public bool isPlayer;

    public PlayerBattleVisuals battleVisualsForPlayer;
    public EnemyBattleVisuals battleVisualsForEnemy;
    // public GameObject MemberBattleVisualPrefab;
    // public GameObject MemberOverworldVisualPrefab;

    public void SetEntityValues(string name, int level, int currentHealth, int maxHealth, int currentEnergy, int maxEnergy, int strength, int smarts, int initiative, bool isPlayer)
    {
        this.Name = name;
        this.Level = level;
        this.CurrHealth = currentHealth;
        this.MaxHealth = maxHealth;
        this.CurrentEnergy = currentEnergy;
        this.MaxEnergy = maxEnergy;
        this.Strength = strength;
        this.Smarts = smarts;
        this.Initiative = initiative;
        this.isPlayer = isPlayer;
    }
}
