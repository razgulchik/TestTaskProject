using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] int monsterId = 0;
    [SerializeField] GameObject enemy;
    GameController gameData;
    PlayerController player;

    int i;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        gameData = gameData = FindObjectOfType<GameController>();
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        if (player)
        {
            while (true)
            {
                Vector3 enemyPosition = new Vector3(
                    gameData.GetMonsterPos(monsterId)["posX"],
                    gameData.GetMonsterPos(monsterId)["posY"],
                    gameData.GetMonsterPos(monsterId)["posZ"]);
                Debug.Log(enemyPosition);
                enemy = Resources.Load(gameData.GetMonsterResPath(monsterId), typeof(GameObject)) as GameObject;
                var newEnemy = Instantiate(enemy, enemyPosition,
                    Quaternion.Euler(0, gameData.GetMonsterPos(monsterId)["rotY"], 0)) 
                    as GameObject;
                i = monsterId;
                monsterId++;
                if (monsterId > 2)
                {
                    monsterId = 0;
                }
                yield return new WaitForSeconds(gameData.GetMonsterPeriod(monsterId));

            }
        }
    }

    public int GetMonsterType()
    {
        return i;
    }
}
