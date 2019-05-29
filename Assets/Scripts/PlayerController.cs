using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Animator animator;

    [Header("Base Parameters")]
    [SerializeField] int lvl = 0;
    [SerializeField] int health = 100;
    [SerializeField] int attackDamage = 1;
    [SerializeField] int experience = 0;

    [Header("DebugParams")]
    [SerializeField] float attackRange = 1f;
    [SerializeField] float runSpeed = 1f;

    ExpAmountDisplay expUpdater;
    LevelDisplay levelUpdater;
    LivesAmountDisplay livesUpdater;
    GameController gameData;
    Levels levelData;

    public struct Levels
    {
        public int lvl;
        public int hp;
        public int damage;
        public int exp;
    }
    
    //private Vector3 moveDirection = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        levelUpdater = FindObjectOfType<LevelDisplay>();
        livesUpdater = FindObjectOfType<LivesAmountDisplay>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        gameData = FindObjectOfType<GameController>();
        expUpdater = FindObjectOfType<ExpAmountDisplay>();

        LevelUp(lvl);
    }

    void LevelUp(int lvl)
    {
        levelUpdater.UpdateLevelText(lvl);
        levelData = JsonUtility.FromJson<Levels>("{" + gameData.GetLevel(lvl-1) + "}");
        health = levelData.hp;
        attackDamage = levelData.damage;

        livesUpdater.UpdateLivesAmount(health);
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
        Attack();
        
    }

    private void Moving()
    {
        Vector3 moveDirection = new Vector3(
            Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        controller.Move(moveDirection * Time.deltaTime * runSpeed);
        if(moveDirection != Vector3.zero)
        {
            transform.forward = moveDirection;
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                animator.SetTrigger("Attack");
                StartCoroutine(Hit());
            }
        }
    }

    IEnumerator Hit()
    {
        yield return new WaitForSeconds(0.5f);
        SendDmg();
    }

    private void SendDmg()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(
            Vector3.forward), out hit, attackRange))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(
                Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Hit");
            hit.collider.gameObject.GetComponent<MonsterController>().MonsterReceiveDamage();
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(
                Vector3.forward) * 5, Color.red);
            Debug.Log("NOT Hit");
        }
    }
    public int PlayerDamage()
    {
        return attackDamage;
    }

    public void AddToExp(int expFromMonster)
    {
        experience += expFromMonster;
        expUpdater.UpdateExpAmount(experience);
        if(experience >= levelData.exp)
        {            
            if (lvl < 5)
            {
                lvl++;
                LevelUp(lvl);
            }
        }
    }

    public void PlayerReceiveDamage(GameObject monster)
    {
        health -= monster.GetComponent<MonsterController>().MonsterDamage();
        livesUpdater.UpdateLivesAmount(health);
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        Destroy(gameObject);
        yield return new WaitForSeconds(2);
        FindObjectOfType<SceneLoader>().LoadSceneByIndex(0);
    }
}
