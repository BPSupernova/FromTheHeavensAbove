using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private LayerMask grassLayer;
    [SerializeField] private float timeBetweenBattles = 10f;

    private PlayerControls playerControls;
    private Rigidbody rb;
    private Vector3 movement;
    private bool movingInBattleArea;
    private float battleCountdown;
    private PartyManager partyManager;

    private const string IS_WALK_PARAM = "IsWalking";
    private const string PLAINS_BATTLE_SCENE = "Battle_Plains_Scene";

    private void Awake() {
        playerControls = new PlayerControls();
        battleCountdown = Random.Range(timeBetweenBattles * 0.7f, timeBetweenBattles * 1.3f);
    }

    private void OnEnable() {
        playerControls.Enable();    
    }

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
        partyManager = GameObject.FindFirstObjectByType<PartyManager>();
    
        if (partyManager.GetPosition() != Vector3.zero) {
            transform.position = partyManager.GetPosition();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float x = playerControls.Player.Move.ReadValue<Vector2>().x;
        float z = playerControls.Player.Move.ReadValue<Vector2>().y;

        movement = new Vector3(x, 0, z).normalized;

        animator.SetBool(IS_WALK_PARAM, movement != Vector3.zero);

        if (x != 0 && x < 0) {
            playerSprite.flipX = true;
        }

        if (x != 0 && x > 0) {
            playerSprite.flipX = false;
        }
    }

    private void FixedUpdate() {
        rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1, grassLayer);
        movingInBattleArea = colliders.Length != 0 && movement != Vector3.zero;

        if (movingInBattleArea) {
            battleCountdown -= Time.deltaTime;

            if (battleCountdown <= 0) {
                partyManager.SetPosition(transform.position);
                battleCountdown = Random.Range(timeBetweenBattles * 0.7f, timeBetweenBattles * 1.3f);
                SceneManager.LoadScene(PLAINS_BATTLE_SCENE);
            }
        }
    }
}
