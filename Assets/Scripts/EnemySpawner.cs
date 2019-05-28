using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] int monsterId = 0;
    [SerializeField] GameObject enemy;
    int period = 5;
    GameController gameData;

    // Start is called before the first frame update
    void Start()
    {
        gameData = gameData = FindObjectOfType<GameController>();
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            Vector3 enemyPosition = new Vector3(
                gameData.GetMonsterPos(monsterId)["posX"],
                gameData.GetMonsterPos(monsterId)["posY"],
                gameData.GetMonsterPos(monsterId)["posZ"]);
            Debug.Log(enemyPosition);
            Instantiate(enemy, enemyPosition, transform.rotation);
            yield return new WaitForSeconds(gameData.GetMonsterPeriod(monsterId));
            monsterId += 1;
            if (monsterId > 2)
            {
                monsterId = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
