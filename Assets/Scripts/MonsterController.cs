using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [Header("AttackRange")]
    [SerializeField] float minDist = 10;
    [SerializeField] float maxDist = 1;

    [Header("Parameters")]
    [SerializeField] public int idM;
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

    GameController gameData;
    PlayerController player;
    Animator animator;

    bool dead = false;
    bool hitPlayer = false;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        animator = GetComponent<Animator>();

        gameData = FindObjectOfType<GameController>();
        Debug.Log(gameObject.name);
        SetupMonster(FindObjectOfType<EnemySpawner>().GetMonsterType());
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();

    }

   
    private void SetupMonster(int id)
    {
        damage = gameData.GetMonsterParameters(id)["damage"];
        move_speed = gameData.GetMonsterParameters(id)["move_speed"];
        hp = gameData.GetMonsterParameters(id)["hp"];
        exp = gameData.GetMonsterParameters(id)["exp"];
        resPath = gameData.GetMonsterResPath(id);
    }

    private void ChasePlayer()
    {
        if (player)
        {
            if (!dead)
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
                    Attack();
                }
            }
        }
    }
    private void Attack()
    {
        if (!dead)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= maxDist)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("EnemyAttack"))
                {
                    animator.SetTrigger("Attack1h1");
                    StartCoroutine(Hit());
                    SndDmgToPlayer();
                }
            }
            else
            {
                StopCoroutine(Hit());
            }
        }

    }
    IEnumerator Hit()
    {
        if (player)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Hit");
                if (hit.collider.gameObject.Equals(player)) { hitPlayer = true; }
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(
                    Vector3.forward) * 5, Color.red);
                Debug.Log("NOT Hit");
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void SndDmgToPlayer()
    {
        if (hitPlayer)
        {
            player.PlayerReceiveDamage(gameObject);
            hitPlayer = false;
        }
    }

    public void MonsterReceiveDamage()
    {
        animator.SetTrigger("Hit1");
        hp -= player.PlayerDamage();
        if(hp <= 0)
        {
            StartCoroutine(Die());
        }
        else
        {
            animator.SetTrigger("Hit1");
        }
    }

    IEnumerator Die()
    {
        dead = true;
        animator.SetTrigger("Fall1");
        player.AddToExp(exp);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    public int MonsterDamage()
    {
        return damage;
    }
}
