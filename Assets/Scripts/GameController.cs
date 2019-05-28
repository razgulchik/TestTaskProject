using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using BayatGames.Serialization.Formatters.Json;

public class GameController : MonoBehaviour
{
    private string jsonString;
    public GameData gameData;
    Monster monster;

    Dictionary<string, string> dictDebug;

    public class GameData
    {
        public string[] monsters;
        public string[] levels;
    }
    public class Monster
    {
        public int id;
        public float posX;
        public float posY;
        public float posZ;
        public float rotY;
        public int damage;
        public int move_speed;
        public int hp;
        public int exp;
        public int period;
        public string resPath;
    }

    public void Awake()
    {
        jsonString = File.ReadAllText(Application.dataPath + "/StreamingAssets/data.txt");
        gameData = (GameData)JsonFormatter.DeserializeObject(jsonString, typeof(GameData));
        Debug.Log(gameData.monsters[0]);
        Debug.Log(gameData.monsters[1]);
    }

    public Dictionary<string, float> GetMonsterPos(int monsterId)
    {
        Monster monstersData = JsonUtility.FromJson<Monster>(
            "{" + gameData.monsters[monsterId] + "}");
        var monster = new Dictionary<string, float>();
        monster.Add("posX", monstersData.posX);
        monster.Add("posY", monstersData.posY);
        monster.Add("posZ", monstersData.posZ);
        monster.Add("rotY", monstersData.rotY);
        return monster;
    }

    public Dictionary<string, int> GetMonsterParameters(int monsterId)
    {
        Monster monstersData = JsonUtility.FromJson<Monster>(
            "{" + gameData.monsters[monsterId] + "}");
        var monster = new Dictionary<string, int>();
        monster.Add("damage", monstersData.damage);
        monster.Add("move_speed", monstersData.move_speed);
        monster.Add("hp", monstersData.hp);
        monster.Add("exp", monstersData.exp);
        return monster;
    }

    public int GetMonsterPeriod(int monsterId)
    {
        Monster monstersData = JsonUtility.FromJson<Monster>(
            "{" + gameData.monsters[monsterId] + "}");
        return monstersData.period;
    }

    public string GetMonsterResPath(int monsterId)
    {
        Monster monstersData = JsonUtility.FromJson<Monster>(
            "{" + gameData.monsters[monsterId] + "}");
        return monstersData.resPath;
    }

    public string GetLevel(int levelId)
    {
        return gameData.levels[levelId];
    }
}
