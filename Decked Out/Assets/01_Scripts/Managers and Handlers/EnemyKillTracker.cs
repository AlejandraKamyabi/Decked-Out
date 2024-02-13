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

    private WaveManager _waveManager;
    private GameLoader _loader;
    private CardRandoEngine _randoEngine;
    //Enemy Number Tracking
    int _enemiesInWave;
    int _enemiesDestroyedThisWave;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _randoEngine = FindObjectOfType<CardRandoEngine>();
        _waveManager = FindObjectOfType<WaveManager>();
        UpdateEnemyCountText();
        UpdateGemCountText();
    }

    public void EnemyKilled()
    {
        totalEnemiesDestroyed++;
        _enemiesDestroyedThisWave++;
        if (_enemiesDestroyedThisWave == _enemiesInWave)
        {
            _randoEngine.NewWave();
            WaveUpdate();
            AllEnemiesInWaveDestroyed();
        }
        UpdateEnemyCountText();

        // Check for gem drop
        if (Random.value <= gemDropChance)
        {
            CollectGem();
        }
    }
    public void EnemyDestroyed()
    {
        _enemiesDestroyedThisWave++;
        if (_enemiesDestroyedThisWave == _enemiesInWave)
        {
            _randoEngine.NewWave();
            WaveUpdate();
            AllEnemiesInWaveDestroyed();
        }
    }
    public void NumbersOfEnemiesInWave(int enemies)
    {        
        _enemiesInWave = enemies;
    }
    public void AllEnemiesInWaveDestroyed()
    {
        _waveManager.AllEnemiesInWaveDestroyed();
        _enemiesDestroyedThisWave = 0;
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
        endGameEnemyCountText.text = "Kills: " + totalEnemiesDestroyed.ToString("f0");
        endGameGemCountText.text = "Gems: " + totalGemsCollected.ToString("f0");
        endGameWave.text = "Wave: " + currentWave;

        // Reset values when the game ends
        // You may choose to reset other things, but not gems
        ResetValues();
    }
}

