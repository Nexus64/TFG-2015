using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {
    public static bool firstGame = true;
    public int victoryScene = 0;
    public int gameOverScene = 2;

    public bool debug;
    public int levelID;
    public int levelCount;
    public float endLevelTime;

    private Door doorScript;
    private MainCharacter player;
    private TutorialGUI tutorialScript;
    private LevelGenerator levelGenerator;

    void Awake()
    {
        doorScript = GameObject.FindGameObjectWithTag("Door").GetComponent<Door>();
        tutorialScript = GameObject.FindGameObjectWithTag("TutorialGUI").GetComponent<TutorialGUI>();
        levelGenerator = GetComponent<LevelGenerator>();
    }

    void Start()
    {
        if (!debug)
        {
            levelID = GetLevelId();
        }
        else
        {
            PlayerPrefs.DeleteAll();
        }

        if (firstGame)
        {
            tutorialScript.SetVisible(true);
            firstGame = false;
        }

        levelGenerator.Generate(levelID);
    }

    int GetLevelId()
    {
        return PlayerPrefs.GetInt("level") + 1;
    }

    public void NextStage()
    {
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        PlayerPrefs.Save();
        FlashTransition transition = Camera.main.GetComponent<FlashTransition>();

        if (PlayerPrefs.GetInt("level") < levelCount)
        {
            transition.StartTransition(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            PlayerPrefs.SetInt("level", 0);
            transition.StartTransition(victoryScene);
        }
    }

    public void GameOver()
    {
        FlashTransition transition = Camera.main.GetComponent<FlashTransition>();
        transition.StartTransition(gameOverScene);
    }

    public void DestroyBricks()
    {
        List<Brick> brickList = levelGenerator.GetBrickList();
        foreach (Brick brick in brickList)
        {
            brick.DoDamage(99);
        }
    }

    public float DestroyBoss()
    {
        BossController boss = levelGenerator.GetBoss();

        if (boss != null)
        {
            boss.StartDestruction();
            return endLevelTime;
        }

        return 0;
    }

    public void DamageBoss(int damage)
    {
        BossController boss = levelGenerator.GetBoss();

        if (boss != null)
        {
            boss.DoDamage(damage);
        }
    }

    public void PlayerReady(MainCharacter _player)
    {
        player = _player;
        BossController boss = levelGenerator.GetBoss();

        if (boss != null)
        {
            boss.Activate();
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