using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockAlly : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    #region VARIABLES

    protected float _health = 100f;
    protected float _range = 10f;
    protected float _damage = 1f;
    protected float _speed = 10f;
    protected float _shotFreq = 1f;
    protected float _shotTimer = 0f;
    protected float _detectionRadius = 2.5f;

    protected int _tier = 1;
    protected int _palCount = 1;
    protected int _currentLevel = 1;
    protected bool _isSpecialtyUnlocked = false;
    protected readonly int SPECIALTY_LEVEL = 3;

    protected bool _canShot = true;
    protected List<BlockEnemy> _enemiesInRange = new List<BlockEnemy>();

    protected bool _isPlaying = true;

    [SerializeField]
    SpriteRenderer _rangeDisplay = null;

    #endregion

    #region PROPERTIES

    public bool IsSpecialtyUnlocked { get => _isSpecialtyUnlocked; }
    public int CurrentLevel { get => _currentLevel; }
    public float DetectionRange { get => _detectionRadius; set { _detectionRadius = value; UpdateRangeDisplay(); } }

    #endregion

    #region UNITY

    void Start()
    {
        Setup();
        UpdateRangeDisplay();
    }

    void Update()
    {
        if (_isPlaying)
            HandleAttack();
    }

    #endregion

    #region PUBLIC

    public void Upgrade()
    {
        _palCount++;

        HandleLevel();
        HandleUpgrades();
    }

    public void Pause()
    {
        _isPlaying = false;
    }

    public void Resume()
    {
        _isPlaying = true;
    }

    #endregion

    #region PROTECTED

    protected virtual void Setup()
    {
        _shotTimer = _shotFreq;
    }

    protected virtual void SetLvlTwoBuffs()
    {
        UpdateRangeDisplay();
    }

    protected virtual void SetLvlThreeBuffs()
    {
        UpdateRangeDisplay();
    }

    protected virtual void EnableSpeciality()
    {
        _isSpecialtyUnlocked = true;
    }

    protected virtual void Attack()
    {
        _canShot = false;

        //Note Optimization: Create bullet pooling.
    }

    protected void TakeDamage(int damageAmount)
    {
        _health -= damageAmount;
        if (_health <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected List<BlockEnemy> GetClosestEnemies(int numEnemies)
    {
        List<BlockEnemy> closestEnemies = new List<BlockEnemy>();
        float[] closestDistances = new float[numEnemies];
        for (int i = 0; i < numEnemies; i++)
        {
            closestDistances[i] = Mathf.Infinity;
        }

        foreach (BlockEnemy enemy in _enemiesInRange)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            for (int i = 0; i < numEnemies; i++)
            {
                if (distanceToEnemy < closestDistances[i])
                {
                    closestDistances[i] = distanceToEnemy;
                    closestEnemies.Insert(i, enemy);
                    if (closestEnemies.Count > numEnemies)
                    {
                        closestEnemies.RemoveAt(numEnemies);
                    }
                    break;
                }
            }
        }

        return closestEnemies;
    }

    #endregion

    #region PRIVATE

    void HandleUpgrades()
    {
        if (_currentLevel == 2)
            SetLvlTwoBuffs();
        else if (_currentLevel == 3)
            SetLvlThreeBuffs();

        if (_currentLevel == SPECIALTY_LEVEL)
            EnableSpeciality();
    }

    void HandleLevel()
    {
        if (_currentLevel != 2 && _palCount >= 3 && _palCount < 9)
            _currentLevel = 2;
        else if (_currentLevel != 3 && _palCount >= 9)
            _currentLevel = 3;
    }

    void HandleAttack()
    {
        if (CanShot())
        {
            UpdateEnemiesInRange();
            Attack();
        }
    }

    bool CanShot()
    {
        if (!_canShot)
        {
            _shotTimer += Time.deltaTime;

            if (_shotTimer >= _shotFreq)
            {
                _shotTimer = 0f;
                _canShot = true;
            }

        }

        return _canShot;
    }

    void UpdateEnemiesInRange()
    {
        _enemiesInRange.Clear(); // Clear the list of enemies before updating
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _detectionRadius);
        foreach (Collider2D collider in hitColliders)
        {
            BlockEnemy blockEnemy = collider.GetComponent<BlockEnemy>();
            if (blockEnemy != null)
                _enemiesInRange.Add(blockEnemy);
        }
    }

    void UpdateRangeDisplay()
    {
        if (!_rangeDisplay) return;

        _rangeDisplay.transform.localScale = new Vector3(_detectionRadius, _detectionRadius, _detectionRadius);
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.white;
    //    Gizmos.DrawSphere(transform.position, _detectionRadius);
    //}

    #endregion

    #region INTERFACE

    public void OnPointerEnter(PointerEventData eventData)
    {
        _rangeDisplay.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _rangeDisplay.enabled = false;
    }

    #endregion
}
