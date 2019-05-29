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

    bool death = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        animator = GetComponent<Animator>();

        gameData = FindObjectOfType<GameController>();
        Debug.Log(gameObject.name);
        SetupMonster(idM);
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
        //Attack();

    }

    private int CheckTypeOfMonster(string prefName)
    {
        var monsterId = 1;

        return monsterId;
    }
    
    private void SetupMonster(int id)
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
        if (player)
        {
            if (!death)
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
        if (!death)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= maxDist)
            {
                StartCoroutine( Hit());
            }
            else
            {
                StopCoroutine(Hit());
            }
        }

    }
    IEnumerator Hit()
    {
        
        if (!death)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("EnemyAttack"))
            {
                animator.SetTrigger("Attack1h1");
                SndDmgToPlayer();
            }
        }
        yield return new WaitForSeconds(1);
    }

    private void SndDmgToPlayer()
    {
        if (player)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(
                Vector3.forward), out hit, 2))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(
                    Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Hit");
                player.PlayerReceiveDamage(gameObject);
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(
                    Vector3.forward) * 5, Color.red);
                Debug.Log("NOT Hit");
            }
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
        death = true;
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
