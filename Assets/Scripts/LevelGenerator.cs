using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public static bool firstGame = true;
    public bool debug = false;
    public int levelID = 0;
    public int levelCount = 11;
    public int victoryScene = 0;

    public ColorToPrefab[] colorMappings;
    public Transform bossPrefab;
    public Transform brickWall;
    public Vector3 location = new Vector3(0, 16, 0);

    protected Transform boss;
    protected MainCharacter player;
    protected bool isBossLevel;

    protected List<BasicBrick> brickList;
    protected LevelInfo levelInfo;
    protected Texture2D tileTypes;
    protected Texture2D tileColor;

    protected string levelFileBase = "Levels/level#";
    protected string mapPath = "Levels/";
    protected string subFix = "";
    protected string formName = "level#_form";
    protected string colorName = "level#_color";

    protected Door doorScript;
    protected TutorialGUI tutorialScript;

    // Use this for initialization
    void Start()
    {
        brickList = new List<BasicBrick>();
        doorScript = GameObject.FindGameObjectWithTag("Door").GetComponent<Door>();
        tutorialScript = GameObject.FindGameObjectWithTag("TutorialGUI").GetComponent<TutorialGUI>();

        if (!debug)
        {
            levelID = GetLevelId();
        }
        else
        {
            PlayerPrefs.DeleteAll();
        }

        string levelPath = levelFileBase.Replace("#", levelID.ToString());

        string json = Resources.Load<TextAsset>(levelPath).text;
        LevelInfo levelInfo = JsonUtility.FromJson<LevelInfo>(json);

        isBossLevel = levelInfo.bossID != 0;
        if (!isBossLevel)
        {
            tileTypes = levelInfo.GetTilesTexture(mapPath); // GetFormTexture();
            tileColor = levelInfo.GetColorsTexture(mapPath); // GetColorTexture();
            GenerateLevel();
        }
        else
        {
            GenerateBoss();
        }

        doorScript.SetLive(levelInfo.doorHP);
        if (firstGame)
        {
            tutorialScript.SetVisible(true);
            firstGame = false;
        }
    }

    // Update is called once per frame
    void GenerateLevel()
    {
        bool[,] occupiedMap = GenerateEmptyArray(tileTypes.width, tileTypes.height);
        for (int i = 0; i < tileTypes.width; i++)
        {
            for (int j = 0; j < tileTypes.height; j++)
            {
                if (!occupiedMap[i, j])
                {
                    GenerateTile(i, j, occupiedMap);

                }
            }
        }
    }

    void GenerateBoss()
    {
        boss = Instantiate(bossPrefab, doorScript.transform);
        boss.position += doorScript.transform.position;
        //boss.SetParent(doorScript.transform);
    }

    bool[,] GenerateEmptyArray(int width, int height)
    {
        bool[,] emptyArray = new bool[width, height];
        int bound0 = emptyArray.GetUpperBound(0);
        int bound1 = emptyArray.GetUpperBound(1);
        // ... Loop over bounds.
        for (int i = 0; i <= bound0; i++)
        {
            for (int j = 0; j <= bound1; j++)
            {
                emptyArray[i, j] = false;
            }
        }
        return emptyArray;
    }

    void GenerateTile(int x, int y, bool[,] occupiedMap)
    {
        Color32 pixelColor = tileTypes.GetPixel(x, y);
        if (pixelColor.a != 0)
        {
            foreach (ColorToPrefab colorMapping in colorMappings)
            {
                //Debug.Log("brick " + colorMapping.prefab.name + ": " + colorMapping.color + " == " + pixelColor);
                if (colorMapping.color.Equals(pixelColor))
                {
                    Vector3 position = location + new Vector3(x, y, 0);
                    Transform tile = Instantiate(colorMapping.prefab);
                    tile.transform.SetParent(brickWall);
                    tile.transform.localPosition = position;
                    BasicBrick brick = tile.GetComponent<BasicBrick>();
                    brick.color = tileColor.GetPixel(x, y);
                    OccupedTile(x, y, colorMapping.horSize, colorMapping.verSize, occupiedMap);
                    brickList.Add(brick);
                    break;
                }
            }
        }
    }

    void OccupedTile(int x, int y, int h, int v, bool[,] occupiedMap)
    {
        for (int i = x; i < x + h && i < occupiedMap.GetLength(0); i++)
        {
            for (int j = y; j < y + v && j < occupiedMap.GetLength(1); j++)
            {
                //Debug.Log(i.ToString() + " " + j.ToString());
                occupiedMap[i, j] = true;
            }
        }
    }

    int GetLevelId()
    {
        return PlayerPrefs.GetInt("level") + 1;
    }

    Texture2D GetFormTexture()
    {
        string fullPath = mapPath + formName + subFix;
        fullPath = fullPath.Replace("#", levelID.ToString());

        Texture2D map = (Texture2D)Resources.Load(fullPath, typeof(Texture2D));

        return map;
    }

    Texture2D GetColorTexture()
    {
        string fullPath = mapPath + colorName + subFix;
        fullPath = fullPath.Replace("#", levelID.ToString());
        Texture2D map = (Texture2D)Resources.Load(fullPath, typeof(Texture2D));

        return map;
    }

    public void NextStage()
    {
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        PlayerPrefs.Save();
        FlashTransition transition = Camera.main.GetComponent<FlashTransition>();

        if (PlayerPrefs.GetInt("level") < levelCount)
        {
            transition.StartTransition(SceneManager.GetActiveScene().buildIndex);
        } else
        {
            PlayerPrefs.SetInt("level", 0);
            transition.StartTransition(victoryScene);
        }
    }

    public void DestroyBricks()
    {
        foreach (BasicBrick brick in brickList)
        {
            brick.DoDamage(99);
        }
    }

    public float DestroyBoss()
    {
        float destroyTime = 0;
        if (isBossLevel)
        {
            BossIA bossIA = boss.GetComponent<BossIA>();
            destroyTime = bossIA.StartDestruction();
        }

        return destroyTime;
    }

    public void DamageBoss(int damage)
    {
        if (isBossLevel)
        {
            BossIA bossIA = boss.GetComponent<BossIA>();
            bossIA.DoDamage(damage);
        }
    }

    public void PlayerReady(MainCharacter _player)
    {
        player = _player;
        if (isBossLevel)
        {
            BossIA bossIA = boss.GetComponent<BossIA>();
            bossIA.Activate();
        }
        else
        {
            player.Activate();
        }
    }

    public void BossReady(BossIA bossIA)
    {
        player.Activate();
    }
}
