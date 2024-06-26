using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyInfo[] allEnemies;
    [SerializeField] private List<Enemy> currentEnemies;

    private static GameObject instance;

    private const float LEVEL_MODIFIER = 0.5f;
    
    private void Awake() {
        if (instance != null) {
            Destroy(this.gameObject);
        } else {
            instance = this.gameObject;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void GenerateEnemiesByEncounter(Encounter[] encounters, int maxNumOfEnemies) {
        currentEnemies.Clear();
        int numEnemies = Random.Range(1, maxNumOfEnemies + 1);

        for (int i = 0; i < numEnemies; i++)
        {
            Encounter tempEncounter = encounters[Random.Range(0, encounters.Length)];
            GenerateEnemy(tempEncounter.Enemy.EnemyName, Random.Range(tempEncounter.levelMin, tempEncounter.levelMax + 1));
        }
    }

    private void GenerateEnemy(string enemyName, int level) {
        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (enemyName == allEnemies[i].EnemyName) {
                Enemy newEnemy = new Enemy();

                newEnemy.EnemyName = allEnemies[i].EnemyName;
                newEnemy.Level = level;
                float levelModifier = (LEVEL_MODIFIER * newEnemy.Level);

                newEnemy.CurrentHealth = Mathf.RoundToInt(allEnemies[i].BaseHealth + (allEnemies[i].BaseHealth * levelModifier));
                newEnemy.MaxHealth = Mathf.RoundToInt(allEnemies[i].BaseHealth + (allEnemies[i].BaseHealth * levelModifier));
                newEnemy.CurrentEnergy = Mathf.RoundToInt(allEnemies[i].BaseEnergy +(allEnemies[i].BaseEnergy * levelModifier));
                newEnemy.MaxEnergy = Mathf.RoundToInt(allEnemies[i].BaseEnergy +(allEnemies[i].BaseEnergy * levelModifier));
                newEnemy.Strength = allEnemies[i].BaseStrength;
                newEnemy.Smarts = allEnemies[i].BaseSmarts;
                newEnemy.Initiative = allEnemies[i].BaseInitiative;
                newEnemy.EnemyVisualPrefab = allEnemies[i].EnemyVisualPrefab;

                currentEnemies.Add(newEnemy);  
            }
        }
    }

    public List<Enemy> GetCurrentEnemies() {
        return this.currentEnemies;
    }
}

[System.Serializable]
public class Enemy {
    public string EnemyName;
    public int Level;
    public int CurrentHealth;
    public int MaxHealth;
    public int CurrentEnergy;
    public int MaxEnergy;
    public int Strength;
    public int Smarts;
    public int Initiative;

    public GameObject EnemyVisualPrefab;
}