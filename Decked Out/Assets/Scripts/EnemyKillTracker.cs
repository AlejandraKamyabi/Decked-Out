using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyKillTracker : MonoBehaviour
{
    public TextMeshProUGUI enemyCountText;
    public TextMeshProUGUI gemCountText;
    public TextMeshProUGUI wave;
    public TextMeshProUGUI endGameEnemyCountText;
    public TextMeshProUGUI endGameGemCountText;
    public TextMeshProUGUI endGameWave;
    public int totalEnemiesDestroyed = 0;
    public int totalGemsCollected = 0;
    public int currentWave = 1;
    public float duration;
    public float gemDropChance = 0.01f;
    private GameLoader _loader;
    private WaveManager mouse;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        UpdateEnemyCountText();
        UpdateGemCountText();
        mouse = ServiceLocator.Get<WaveManager>();
    }

    public void EnemyDestroyed()
    {
        totalEnemiesDestroyed++;
        UpdateEnemyCountText();

        // Check for gem drop
        if (Random.value <= gemDropChance)
        {
            CollectGem();
        }
    }

    private void CollectGem()
    {
        totalGemsCollected++;
        UpdateGemCountText();
    }

    public void WaveUpdate()
    {
        currentWave++;
        UpdateEnemyCountText();
    }

    public void resetWave()
    {
        currentWave = 1;
        UpdateEnemyCountText();
    }

    public void ResetValues()
    {
        totalEnemiesDestroyed = 0;
        // Do not reset gems here
        currentWave = 1;
        UpdateEnemyCountText();
        UpdateGemCountText();
    }

    private void UpdateEnemyCountText()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = "Kills: " + totalEnemiesDestroyed.ToString();
            if (totalEnemiesDestroyed > 0)
            {
                StartCoroutine(ChangeTextColour(duration));
            }
        }
        wave.text = "Wave: " + currentWave.ToString();
    }

    private void UpdateGemCountText()
    {
        if (gemCountText != null)
        {
            gemCountText.text = "Gems: " + totalGemsCollected.ToString();
        }
    }

    IEnumerator ChangeTextColour(float duration)
    {
        enemyCountText.color = Color.red;

        yield return new WaitForSeconds(duration);

        enemyCountText.color = Color.white;
    }

    public void EndGame()
    {
        endGameEnemyCountText.text = "Kills: " + totalEnemiesDestroyed.ToString();
        endGameGemCountText.text = "Gems: " + totalGemsCollected.ToString();
        endGameWave.text = "Wave: " + currentWave.ToString();

        // Reset values when the game ends
        // You may choose to reset other things, but not gems
        ResetValues();
    }
}

