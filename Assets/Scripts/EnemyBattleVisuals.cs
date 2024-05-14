using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyBattleVisuals : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI levelText;

    private Animator anim;

    private int currentHealth;
    private int maxHealth;
    private int level;

    private const string LEVEL_LABEL = "Level ";

    private const string IS_ATTACK_PARAM = "IsAttack";
    private const string IS_HIT_PARAM = "IsHit";
    private const string IS_DOWN_PARAM = "IsDown";

    private void Start() {
        anim = gameObject.GetComponent<Animator>();
    }

    public void SetStartingValues(int CurrentHealth, int MaxHealth, int level) {
        this.currentHealth = CurrentHealth;
        this.maxHealth = MaxHealth;
        this.level = level;
        levelText.text = LEVEL_LABEL + this.level.ToString();
        UpdateHealthBar();
    }

    public void ChangeHealth(int CurrentHealth) {
        this.currentHealth = CurrentHealth;
        // If health is 0 -> Play death animation & end battle visual
    }

    public void UpdateHealthBar() {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void PlayAttackAnimation() {
        anim.SetTrigger(IS_ATTACK_PARAM);
    }

    public void PlayHitAnimation() {
        anim.SetTrigger(IS_HIT_PARAM);
    }

    public void PlayDownAnimation() {
        anim.SetTrigger(IS_DOWN_PARAM);
    }
}
