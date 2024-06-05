using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public int _enemiesInWave;
    public int _enemiesDestroyedThisWave;

    private SaveSystem _saveSystem;
    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _randoEngine = FindObjectOfType<CardRandoEngine>();
        _waveManager = FindObjectOfType<WaveManager>();
        _saveSystem = FindObjectOfType<SaveSystem>();
        UpdateEnemyCountText();
        UpdateGemCountText();
    }

    public void EnemyKilled()
    {
        Debug.Log("Enemy Killed");
        totalEnemiesDestroyed++;
        _enemiesDestroyedThisWave++;
        CheckWaveCompletion();
        UpdateEnemyCountText();

        if (Random.value <= gemDropChance)
        {
            CollectGem();
        }
    }

    public void EnemyDestroyed()
    {
        Debug.Log("Enemy Destroyed");
        _enemiesDestroyedThisWave++;
        CheckWaveCompletion();
    }

    private void CheckWaveCompletion()
    {
        if (_enemiesDestroyedThisWave == _enemiesInWave)
        {
            _randoEngine.NewWave();
            WaveUpdate();
            AllEnemiesInWaveDestroyed();
        }
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
        currentWave = 1;
        UpdateEnemyCountText();
        UpdateGemCountText();
    }

    public void NumbersOfEnemiesInWave(int enemies)
    {
        _enemiesInWave = enemies;
        Debug.Log("Enemies In Wave " + _waveManager.currentWave + ": " + _enemiesInWave);
    }

    public void AddEnemyToCurrentWave(string enemyType, Vector3 spawnPosition)
    {
        GameObject newEnemy = null;
        Slider newHealthSlider = Instantiate(_waveManager.healthSliderPrefab);
        Vector3 sliderPosition;

        switch (enemyType)
        {
            case "Acolyte":
                newEnemy = Instantiate(_waveManager.enemyPrefab, spawnPosition, Quaternion.identity);
                newHealthSlider.maxValue = newEnemy.GetComponent<Enemy>().maxHealth;
                break;
            case "Kaboom":
                newEnemy = Instantiate(_waveManager.KaboomPrefab, spawnPosition, Quaternion.identity);
                newHealthSlider.maxValue = newEnemy.GetComponent<KaboomEnemy>().maxHealth;
                break;
            case "Golem":
                newEnemy = Instantiate(_waveManager.GolemPrefab, spawnPosition, Quaternion.identity);
                newHealthSlider.maxValue = newEnemy.GetComponent<Enemy>().maxHealth;
                break;
            case "Apostate":
                newEnemy = Instantiate(_waveManager.Apostate_Prefab, spawnPosition, Quaternion.identity);
                newHealthSlider.maxValue = newEnemy.GetComponent<Apostate>().maxHealth;
                break;
            case "Necromancer":
                newEnemy = Instantiate(_waveManager.necromancer, spawnPosition, Quaternion.identity);
                newHealthSlider.maxValue = newEnemy.GetComponent<Necromancer>().maxHealth;
                break;
            case "Aegis":
                newEnemy = Instantiate(_waveManager.aegis, spawnPosition, Quaternion.identity);
                newHealthSlider.maxValue = newEnemy.GetComponent<Aegis>().maxHealth;
                break;
            case "Cleric":
                newEnemy = Instantiate(_waveManager.cleric, spawnPosition, Quaternion.identity);
                newHealthSlider.maxValue = newEnemy.GetComponent<Cleric>().maxHealth;
                break;
        }

        if (newEnemy != null)
        {
            sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 100.0f, 0));
            newHealthSlider.transform.position = sliderPosition;
            newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
            newEnemy.GetComponent<Enemy>().SetHealthSlider(newHealthSlider);
            _waveManager.waves[_waveManager.currentWave].numberOfEnemies++;
            NumbersOfEnemiesInWave(_waveManager.waves[_waveManager.currentWave].numberOfEnemies);
        }
    }

    private void UpdateEnemyCountText()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = totalEnemiesDestroyed.ToString();
            if (totalEnemiesDestroyed > 0)
            {
                StartCoroutine(ChangeTextColour(duration));
            }
        }
        wave.text = currentWave.ToString();
    }

    private void UpdateGemCountText()
    {
        if (gemCountText != null)
        {
            gemCountText.text = totalGemsCollected.ToString();
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
        _saveSystem.AddGem(totalGemsCollected);
        _saveSystem.AddTotalKill(totalEnemiesDestroyed);
        ResetValues();
    }
}
