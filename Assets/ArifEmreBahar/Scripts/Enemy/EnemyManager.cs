using System.Collections.Generic;
using Tomino;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region VARIABLES
    [SerializeField]
    Transform _spawnParent;

    [Header("Settings")]
    [SerializeField]
    float _currentTimerMax = 2f;
    [SerializeField]
    float _currentSpeedMax = 0.5f;
    [SerializeField]
    float _timeToIncreaseDifficulty = 10f;
    [SerializeField]
    [Range(0, 100)]
    float _mainEnemySpawnChance = 60f;
    [Space]
    [SerializeField]
    BlockEnemy[] enemies;

    List<BlockEnemy> _currentEnemies = new List<BlockEnemy>();
    float _timer = 0f;
    float _timerMax = 10f;
    float _speedMax = 10f;
    float _limitTrack = 0f;
    float _spawnCountDown = 0f;
    float _difficultyCountDown = 0f;
    int _currrentDifficultyIndex = 0;
    int _spawnStack = 0;
    int _tempNamer = 0;
    int _boardWidth = 10;
    int _boardHeight = 10;
    int _gameLevel = 1;
    bool _isPlaying = true;
    #endregion

    #region UNITY

    void Start()
    {
        _timerMax = _currentTimerMax;
        _speedMax = _currentSpeedMax;
        _difficultyCountDown = _timeToIncreaseDifficulty;
    }
    void Update()
    {
        if (!_isPlaying) return;

        HandleTimersByLevel();
        //HandleSpawnTimerByTime();
        HandleDifficulty();
    }
    #endregion

    #region PUBLIC

    public void SetGameBoard(int width, int height)
    {
        _boardWidth = width;
        _boardHeight = height;
    }

    public void Pause()
    {
        _isPlaying = false;
    }

    public void Resume()
    {
        _isPlaying = true;
    }

    public void FrezeeAll()
    {
        //---
    }

    public void ThawAll()
    {
        //---
    }

    public void SetLevel(int level)
    {
        _gameLevel = level;
    }

    public void DestroyAll()
    {
        foreach (BlockEnemy enemy in _currentEnemies)
            Destroy(enemy);

        RemoveAll();
    }

    public void RemoveAll()
    {
        _currentEnemies.Clear();
    }

    public void MoveAll()
    {
        if (_limitTrack < _currentSpeedMax) return;

        foreach (BlockEnemy enemy in _currentEnemies)
            enemy.Move();

        _limitTrack = 0;
    }
    #endregion

    #region PRIVATE

    void HandleSpawnTimerByTime()
    {
        ///
    }

    void HandleTimersByLevel()
    {
        _currentTimerMax = _timerMax / ((_gameLevel * 3) + 1);
        _currentSpeedMax = _speedMax / ((_gameLevel / 2) + 1);

        if (_limitTrack < _currentSpeedMax)
            _limitTrack += Time.deltaTime;

        if (_timer < _currentTimerMax * _boardWidth)
            _timer += Time.deltaTime;

        _spawnStack = (int)(_timer / _currentTimerMax * 3);

        //Debug.Log("_spawnStack "  + _spawnStack + " timer " + _timer + " spawninterval " +  TimerMax);
    }

    public void SpawnEnemyStack()
    {
        for (int i = 0; i < _spawnStack; i++)
        {
            BlockEnemy blockEnemy = Instantiate(enemies[GetSpawnIndexByChance()], _spawnParent);
            blockEnemy.gameObject.name = blockEnemy.gameObject.name + _tempNamer;
            _tempNamer++;
            blockEnemy.Setup();
            blockEnemy.SetGameBoard(_boardWidth, _boardHeight);
            blockEnemy.SetPosition(0, Random.Range(0, _boardWidth));
            blockEnemy.DieEvent += HandleEnemyDeath;
            _currentEnemies.Add(blockEnemy);
        }

        if (_spawnStack == 0) return;
        _spawnStack = 0;
        _timer = 0;
    }

    void HandleEnemyDeath(BlockEnemy blockEnemy)
    {
        _currentEnemies.Remove(blockEnemy);
    }

    int GetSpawnIndexByChance()
    {
        int spawnIndexByChance = _currrentDifficultyIndex;
        float chance = Random.Range(0f, 100f);
        float rate = _mainEnemySpawnChance;

        for (int i = 0; chance > rate; i++)
        {
            rate = (100 - rate) / 2 + rate;
            spawnIndexByChance++;
        }

        if (spawnIndexByChance + 1 > enemies.Length)
            if (enemies.Length != 0)
                spawnIndexByChance = enemies.Length - 1;
            else
                spawnIndexByChance = 0;

        //Debug.Log("Chance: " + chance + " DifficultyIndex: " + spawnIndexByChance);
        return spawnIndexByChance;
    }

    void HandleDifficulty()
    {
        _difficultyCountDown -= Time.deltaTime;
        if (_difficultyCountDown < 0f && enemies.Length > _currrentDifficultyIndex)
        {
            _currrentDifficultyIndex++;
            _difficultyCountDown = _timeToIncreaseDifficulty;
        }
    }

    #endregion
}