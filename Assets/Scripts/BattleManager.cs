using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private enum BattleState {Start, Selection, Battle, Won, Lost, Escape}

    [Header("Battle State")]
    [SerializeField] private BattleState battleState;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] partySpawnPoints;
    [SerializeField] private Transform[] enemySpawnPoints;

    private PartyManager partyManager;
    private EnemyManager enemyManager;

    private int currentPlayer;

    [Header("Battlers")]
    [SerializeField] private List<BattleEntities> allBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> playerBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> enemyBattlers = new List<BattleEntities>();

    [Header("UI")]
    [SerializeField] private GameObject[] targetSelectButtons;
    [SerializeField] private GameObject battleMenu;
    [SerializeField] private GameObject selectionMenu;
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private GameObject bottomTextPopUp;
    [SerializeField] private TextMeshProUGUI bottomText;

    private const string WIN_MESSAGE = "The plan was full proof!";
    private const string LOSE_MESSAGE = "Humankind's last hope has been lost. Game Over."; 

    private const int TURN_DURATION = 2;

    // Start is called before the first frame update
    void Start()
    {
        partyManager = GameObject.FindFirstObjectByType<PartyManager>();
        enemyManager = GameObject.FindFirstObjectByType<EnemyManager>();
    
        CreatePartyEntities();
        CreateEnemyEntities();
        ShowBattleMenu();
        DetermineBattleOrder();
    }

    private IEnumerator BattleRoutine() {
        selectionMenu.SetActive(false); // Enemy selection manu disabled 
        battleState = BattleState.Battle; // Change our state to the battle state
        bottomTextPopUp.SetActive(true); // Enable Bottom Text

        // Loop through all battlers; Do their appropriate action
        for (int i = 0; i < allBattlers.Count; i++) {
            if (battleState == BattleState.Battle) {
                switch (allBattlers[i].BattleAction) {
                    case BattleEntities.Action.Attack:
                        // Debug.Log(allBattlers[i].Name + " is attacking: " + allBattlers[allBattlers[i].Target].Name);
                        yield return StartCoroutine(AttackRoutine(i));
                        break;
                    case BattleEntities.Action.Escape:
                        break;
                    default:
                        Debug.Log("Error - incorrect battle action");
                        break;
                }
            }
        }

        // If we haven't won or lost, repeat the game loop by opening battle menu
        if (battleState == BattleState.Battle) {
            bottomTextPopUp.SetActive(false);
            currentPlayer = 0;
            ShowBattleMenu();
        }

        yield return null;
    }

    private IEnumerator AttackRoutine(int i) {
        // Player's Turn
        if (allBattlers[i].isPlayer == true) {
            BattleEntities currAttacker = allBattlers[i];
            // Check if a party member kills an enemy that another member was targeting or if target is outside list bounds (bug checks)
            if (allBattlers[currAttacker.Target].isPlayer == true || currAttacker.Target >= allBattlers.Count) {
                currAttacker.SetTarget(GetRandomEnemy());
            }

            BattleEntities currTarget = allBattlers[currAttacker.Target];

            PlayerAttackEnemyAction(currAttacker, currTarget); // Attack selected enemy
            yield return new WaitForSeconds(TURN_DURATION); // Wait a few seconds then kill the enemy
            
            if (currTarget.CurrHealth <= 0) {
                bottomText.text = string.Format("{0} defeated {1}", currAttacker.Name, currTarget.Name);
                yield return new WaitForSeconds(TURN_DURATION);
                enemyBattlers.Remove(currTarget);
                allBattlers.Remove(currTarget);

                if (enemyBattlers.Count <= 0) {
                    battleState = BattleState.Won;
                    bottomText.text = WIN_MESSAGE;
                    yield return new WaitForSeconds(TURN_DURATION); // Wait a few seconds then kill the enemy
                    SceneManager.LoadScene("Overworld_Plains_Scene");
                }
            }
        }

        // Enemies turn
        if (i < allBattlers.Count && allBattlers[i].isPlayer == false) {
            BattleEntities currAttacker = allBattlers[i];
            currAttacker.SetTarget(GetRandomPartyMember()); // Get a random party member to target
            BattleEntities currTarget = allBattlers[currAttacker.Target];

            EnemyAttackPlayerAction(currAttacker, currTarget); // Attack selected party member
            yield return new WaitForSeconds(TURN_DURATION);

            if (currTarget.CurrHealth <= 0) {
                bottomText.text = string.Format("{0} defeated {1}", currAttacker.Name, currTarget.Name);
                yield return new WaitForSeconds(TURN_DURATION);
                playerBattlers.Remove(currTarget);
                allBattlers.Remove(currTarget);

                // If no players left, lose the battle
                if (playerBattlers.Count <= 0) {
                    battleState = BattleState.Lost;
                    bottomText.text = LOSE_MESSAGE;
                    Debug.Log("Go back to overworld scene");
                }
            }
        }
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
            
            tempBattleVisuals.SetStartingValues(currentParty[i].CurrentHealth, currentParty[i].MaxHealth, currentParty[i].CurrentEnergy, currentParty[i].MaxEnergy, currentParty[i].Level);
            tempEntity.battleVisualsForPlayer = tempBattleVisuals;

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

    public void ShowBattleMenu() {
        actionText.text = playerBattlers[currentPlayer].Name;
        battleMenu.SetActive(true);
    }

    public void ShowSelectionMenu() {
        battleMenu.SetActive(false);
        SetTargetSelectButtons();
        selectionMenu.SetActive(true);
    }

    public void SetTargetSelectButtons() {
        for (int i = 0; i < targetSelectButtons.Length; i++) {
            targetSelectButtons[i].SetActive(false);
        }

        for (int j = 0; j < enemyBattlers.Count; j++) {
            targetSelectButtons[j].SetActive(true);
            targetSelectButtons[j].GetComponentInChildren<TextMeshProUGUI>().text = enemyBattlers[j].Name;
        }
    }

    public void SelectTarget(int currentTarget) {
        // Setting the current members target
        BattleEntities currentPlayerEntity = playerBattlers[currentPlayer];
        currentPlayerEntity.SetTarget(allBattlers.IndexOf(enemyBattlers[currentTarget]));

        // Telling the BattleSystem the Player intends to attack
        currentPlayerEntity.BattleAction = BattleEntities.Action.Attack;
        currentPlayer++;

        if (currentPlayer >= playerBattlers.Count) {
            StartCoroutine(BattleRoutine());
        } else {
            selectionMenu.SetActive(false);
            ShowBattleMenu();
        }
    }

    public void PlayerAttackEnemyAction(BattleEntities currAttacker, BattleEntities currTarget) {
        // Get Damage
        int damage = (int)(currAttacker.Strength * (Random.Range(2.1f, 3.4f))); // update to an algorithm once base battle system is finished
        // Debug.Log("Damage dealt: " + damage);
        currAttacker.battleVisualsForPlayer.PlayAttackAnimation(); // Play Attack Animation
        // Maybe call ChangeEnergy() here if the player uses a move that requres energy

        currTarget.CurrHealth -= damage; // Deal Damage
        currTarget.battleVisualsForEnemy.PlayHitAnimation(); // Play Target Hit animation
        currAttacker.UpdatePlayerUI();
        currTarget.UpdateEnemyUI(); // Update battle UI
    
        bottomText.text = string.Format("{0} attacks {1} for {2} damage", currAttacker.Name, currTarget.Name, damage);
        SavePartyHealth();
    }

    public void EnemyAttackPlayerAction(BattleEntities currAttacker, BattleEntities currTarget) {
        // Get Damage
        int damage = currAttacker.Strength; // update to an algorithm once base battle system is finished
        // Debug.Log("Damage dealt: " + damage);
        currAttacker.battleVisualsForEnemy.PlayAttackAnimation(); // Play Attack Animation
        // Maybe call ChangeEnergy() here if the player uses a move that requres energy

        currTarget.CurrHealth -= damage; // Deal Damage
        currTarget.battleVisualsForPlayer.PlayHitAnimation(); // Play Target Hit animation
        currAttacker.UpdateEnemyUI();
        currTarget.UpdatePlayerUI(); // Update battle UI
    
        bottomText.text = string.Format("{0} attacks {1} for {2} damage", currAttacker.Name, currTarget.Name, damage);
        SavePartyHealth();
    }

    private int GetRandomPartyMember() {
        List<int> partyMembers = new List<int>();
        for (int i = 0; i < allBattlers.Count; i++)
        {
            if (allBattlers[i].isPlayer == true) {
                partyMembers.Add(i);
            }
        }

        return partyMembers[Random.Range(0, partyMembers.Count)];
    }

    private int GetRandomEnemy() {
        List<int> enemies = new List<int>();
        for (int i = 0; i < allBattlers.Count; i++)
        {
            if (allBattlers[i].isPlayer == false) {
                enemies.Add(i);
            }
        }
        
        return enemies[Random.Range(0, enemies.Count)];
    }

    private void SavePartyHealth() {
        for (int i = 0; i < playerBattlers.Count; i++) {
            partyManager.SaveHealth(i, playerBattlers[i].CurrHealth);
        }
    }

    private void DetermineBattleOrder() {
        allBattlers.Sort((bi1, bi2) => -bi1.Initiative.CompareTo(bi2.Initiative)); // Sorts list by initiative in ascending order
    }
}

[System.Serializable]
public class BattleEntities
{
    public enum Action { Attack, Escape }
    public Action BattleAction;
    
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

    public int Target;

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

    public void SetTarget(int target) {
        this.Target = target;
    }

    public void UpdatePlayerUI() {
        battleVisualsForPlayer.ChangeHealth(CurrHealth);
        battleVisualsForPlayer.ChangeEnergy(CurrentEnergy);
    }

    public void UpdateEnemyUI() {
        battleVisualsForEnemy.ChangeHealth(CurrHealth);
    }
}
