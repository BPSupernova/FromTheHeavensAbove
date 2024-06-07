using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterSystem : MonoBehaviour
{
    [SerializeField] private Encounter[] enemiesInScene;
    [SerializeField] private int maxNumOfEnemies;

    private EnemyManager enemyManager;
    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GameObject.FindFirstObjectByType<EnemyManager>();
        enemyManager.GenerateEnemiesByEncounter(enemiesInScene, maxNumOfEnemies);
    }
}

[System.Serializable]
public class Encounter {
    public EnemyInfo Enemy;
    public int levelMin;
    public int levelMax;
}