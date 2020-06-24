using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public GameObject player;
    private PlayerController playerController;
    private int gold = 0;
    private int scrap = 0;
    private int maxGold;
    private int ticksAlive;
    private int scoreFromEnemys;
    private int totalscore = 0;
    public Text goldText;
    public Text scrapText;
    public Text gameoverText;
    public Text scoreText;
    public Text restartText;
    public Text pauseText;

    // upgrade menu stuff
    public GameObject menuUI;
    public Text upgradeEnginText;
    public Text upgradeFireRateText;
    public Text sellScrapText;
    public Text enterMenuText;
    public Button upgradeEnginButton;
    public Button upgradeFireRateButton;
    public Button sellScrapButton;
    private int enginLevel = 0;
    private int fireRateLevel = 0;
    private int maxEnginUpgradeLevel = 2;
    private int maxFireRateLevel = 4;

    public bool playerInRangeOfHome = true;

    public enum GameState
    {
        menu,
        play,
        pause,
        gameOver
    }

    public GameState gameState = GameState.play;


    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        menuUI.SetActive(false);
        UpdateText(goldText, "Gold: " + gold);
        UpdateText(scrapText, "Scrap: " + scrap);
        gameoverText.text = "";
        scoreText.text = "";
        restartText.text = "";
        enterMenuText.text = "";
        pauseText.text = "";
        upgradeEnginButton.onClick.AddListener(UpgradeEnginFunction);
        upgradeFireRateButton.onClick.AddListener(UpgradeFireRateFunction);
        sellScrapButton.onClick.AddListener(SellScrapFunction);
        StartCoroutine(SpawnEnemiesAndAstriods());
    }

    void Update()
    {
        if (gameState == GameState.play)
        {
            ticksAlive += 1;
            Time.timeScale = 1;

            // getting into the menu
            if (playerInRangeOfHome)
            {
                enterMenuText.text = "To enter Store press 'M'";
                if (Input.GetKeyDown("m"))
                {
                    enterMenuText.text = "";
                    UpdateMenuText();
                    menuUI.SetActive(true);
                    gameState = GameState.menu;
                }
            }
            else
            {
                enterMenuText.text = "";
            }
        }

        else if (gameState == GameState.menu)
        {
            Time.timeScale = 0;
            if (Input.GetKeyDown("m"))
            {
                enterMenuText.text = "To enter Store press 'M'";
                menuUI.SetActive(false);
                gameState = GameState.play;
            }

        }

        else
        {
            Time.timeScale = 0;
            if (gameState == GameState.gameOver)
            {
                menuUI.SetActive(false);
                if (Input.GetKeyDown(KeyCode.R))
                {
                    string sceneName = SceneManager.GetActiveScene().name;
                    SceneManager.LoadScene(sceneName);
                    Application.LoadLevel(Application.loadedLevel);
                }
            }
        }

        if (Input.GetKeyDown("p"))
        {
            if (gameState == GameState.play)
            {
                pauseText.text = "Game Paused \n press 'P' to resume";
                gameState = GameState.pause;
            }
            else if (gameState == GameState.pause)
            {
                pauseText.text = "";
                gameState = GameState.play;
            }
        }
    }

    void CalculateScore()
    {
        totalscore = (int)(Mathf.Floor(ticksAlive / 60)) + maxGold * 2 + scoreFromEnemys;
    }

    void UpdateMenuText()
    {
        if (enginLevel == maxEnginUpgradeLevel)
        {
            upgradeEnginText.text = "engine lvl: MAX";
        }
        else
        {
            upgradeEnginText.text = "engine lvl: " + enginLevel;
        }

        if (fireRateLevel == maxFireRateLevel)
        {
            upgradeFireRateText.text = "fire rate lvl: MAX";
        }
        else
        {
            upgradeFireRateText.text = "fire rate lvl: " + fireRateLevel;
        }

    }

    void UpdateText(Text t, string s)
    {
        t.text = s;
    }

    void UpgradeEnginFunction()
    {
        if (gold > 59)
        {
            if (enginLevel < maxEnginUpgradeLevel)
            {
                gold -= 60;
                enginLevel += 1;
                playerController.ChangeMovementMode(enginLevel);
                UpdateText(goldText, "Gold: " + gold);
                UpdateMenuText();
            }
        }
    }

    void UpgradeFireRateFunction()
    {
        if (gold > 19)
        {
            if (fireRateLevel < maxFireRateLevel)
            {
                gold -= 20;
                fireRateLevel += 1;
                playerController.ChangeReloadTime(-0.15f);
                UpdateText(goldText, "Gold: " + gold);
                UpdateMenuText();
            }
        }
    }

    void SellScrapFunction()
    {
        if (scrap > 19)
        {
            scrap -= 20;
            gold += 10;
            UpdateText(goldText, "Gold: " + gold);
            UpdateText(scrapText, "Scrap: " + scrap);
        }
    }

    public void AddGold(int amount_of_gold)
    {
        gold += amount_of_gold;
        maxGold += amount_of_gold;
        UpdateText(goldText, "Gold: " + gold);
    }

    public void AddScrap(int amount)
    {
        scrap += amount;
        UpdateText(scrapText, "Scrap: " + scrap);
    }

    public void AddScore(int amount)
    {
        scoreFromEnemys += amount;
    }


    public void ChangeGameState(GameState state)
    {
        gameState = state;
    }


    IEnumerator SpawnEnemiesAndAstriods()
    {
        while (gameState != GameState.menu || gameState != GameState.gameOver)
        {
            int hazardCount = 30;
            Vector3 spawnValues = new Vector3(50, 50, 50);
            int spawnWait = 2;
            int waveWait = 3;
            if (gameState == GameState.play)
            
            {
                yield return new WaitForSeconds(3);
                while (true)
                {
                    for (int i = 0; i < hazardCount; i++)
                    {
                        GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                        Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), Random.Range(-spawnValues.y, spawnValues.y), Random.Range(-spawnValues.z, spawnValues.z));
                        Quaternion spawnRotation = Quaternion.identity;
                        Instantiate(hazard, spawnPosition, spawnRotation);
                        yield return new WaitForSeconds(spawnWait);
                    }
                    yield return new WaitForSeconds(waveWait);
                }
            }
        }
    }

    public void ChangePlayerIsInRangeOfHome(bool b)
    {
        playerInRangeOfHome = b;
    }

    public void GameOver()
    {
        gameState = GameState.gameOver;
        gameoverText.text = "Game over";
        CalculateScore();
        scoreText.text = "Your score is: " + totalscore.ToString();
        restartText.text = "press 'R' to restart";
    }
}
