using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.Serialization.Formatters.Json;

public class MonsterController : MonoBehaviour
{
    [Header("AttackRange")]
    [SerializeField] float minDist = 10;
    [SerializeField] float maxDist = 1;

    [Header("Parameters")]
    [SerializeField] public int id;
    [SerializeField] float posX;
    [SerializeField] float posY;
    [SerializeField] float posZ;
    [SerializeField] float rotY;
    [SerializeField] int damage;
    [SerializeField] int move_speed;
    [SerializeField] int hp;
    [SerializeField] int exp;
    [SerializeField] int period;
    [SerializeField] string resPath;

    [Header("DebugMenu")]
    [SerializeField] int idDbg = 1;

    GameController gameData;
    GameObject player;
    Animator animator;

    bool death = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        animator = GetComponent<Animator>();

        gameData = FindObjectOfType<GameController>();

        SetupMonster();
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
        Attack();

    }
    
    private void SetupMonster()
    {
        damage = gameData.GetMonsterParameters(id)["damage"];
        move_speed = gameData.GetMonsterParameters(id)["move_speed"];
        hp = gameData.GetMonsterParameters(id)["hp"];
        exp = gameData.GetMonsterParameters(id)["exp"];
        period = gameData.GetMonsterPeriod(id);
        resPath = gameData.GetMonsterResPath(id);
    }

    private void ChasePlayer()
    {
        transform.LookAt(player.transform);

        if (Vector3.Distance(transform.position, player.transform.position) >= minDist)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("EnemyAttack"))
            {
                transform.position += transform.forward * move_speed * Time.deltaTime;

                animator.SetFloat("speedh", 1);
            }

        }
        else
        {
            animator.SetFloat("speedh", 0);
        }
    }
    private void Attack()
    {
        if (!death)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= maxDist)
            {
                Hit();
            }
        }

    }
    void Hit()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("EnemyAttack"))
        {
            animator.SetTrigger("Attack1h1");
            
        }
    }
    
    public void ReceiveDamage()
    {
        animator.SetTrigger("Hit1");
        hp -= player.GetComponent<PlayerController>().PlayerDamage();
        if(hp <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        death = true;
        animator.SetTrigger("Fall1");
        player.GetComponent<PlayerController>().AddToExp(exp);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
