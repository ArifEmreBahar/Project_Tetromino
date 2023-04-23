using System.Collections;
using System.Collections.Generic;
using Tomino;
using UnityEngine;

public class BlockEnemy : MonoBehaviour
{
    #region VARIABLES
    [SerializeField]
    float _health = 100f;
    [SerializeField]
    int _damage = 10;
    [SerializeField]
    float _moveDist = 0.4f;
    const string TARGET_TAG = "Core";

    bool _synced = false;
    float _internalElapsedTime = 0f;
    float _manuelFallDelay = 0.1f;
    int _boardWidth = 10;
    int _boardHeight = 10;

    Game _game = null;
    SpriteRenderer _spriteRenderer = null;
    RectTransform _battleField = null;

    public delegate void BlockEnemyEventHandler(BlockEnemy blockEnemy);

    /// <summary>
    /// The event triggered when the game is resumed.
    /// </summary>
    public event BlockEnemyEventHandler DieEvent = delegate { };
    #endregion

    #region UNITY
    protected virtual void Awake()
    {
        _battleField = transform.parent.GetComponent<RectTransform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }
    #endregion

    public void Setup()
    {
        //_game = game;

        //if (_game == null) return;
        //_game.PieceFallingEvent += HandleMovement;
        //_game.PieceMovedDownEvent += HandleMovement;

        //Add a lister here to call those anytime when bordersized has changed.
        //SetPosition(0, Random.Range(0, _game.Board.width));
        SetSize();
    }

    public void SetGameBoard(int width, int height)
    {
        _boardWidth = width;
        _boardHeight = height;
    }
    //void OnDisable()
    //{
    //    if (_game == null) return;
    //    _game.PieceFallingEvent -= HandleMovement;
    //    _game.PieceMovedDownEvent -= HandleMovement;
    //}


    #region PUBLIC
    public void TakeDamage(float damageAmount)
    {
        _health -= damageAmount;
        if (_health <= 0)
            Die();
    } 

    public void Move()
    {
        transform.position += Vector3.down * _moveDist;
    }

    public void SetPosition(int row, int column)
    {
        var size = EnemySize(_boardWidth);
        var position = new Vector3(column * size, row * size, 0);
        var offset = new Vector3(size / 2, size / 2, 0);
        transform.localPosition = position + offset - PivotOffset();
    }
    #endregion

    #region PROTECTED
    protected virtual void Die()
    {
        // This is called when the ally's health drops to 0 or below
        DieEvent(this);
        Destroy(gameObject);
    }
    #endregion

    #region PRIVATE
    void SetSize(float size)
    {
        var sprite = _spriteRenderer.sprite;
        var scale = sprite.pixelsPerUnit / sprite.rect.width * size;
        transform.localScale = new Vector3(scale, scale);
    }

    void SetSize()
    {
        float scale = _spriteRenderer.sprite.pixelsPerUnit / _spriteRenderer.sprite.rect.width * EnemySize(_boardWidth);
        transform.localScale = new Vector3(scale, scale);
    }

    Vector3 PivotOffset()
    {
        var pivot = _battleField.pivot;
        var boardSize = _battleField.rect.size;
        return new Vector3(boardSize.x * pivot.x, boardSize.y * pivot.y, 0);
    }

    public float EnemySize(int width)
    {
        var boardWidth = _battleField.rect.size.x;
        return boardWidth / width;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TARGET_TAG))
        {
            HeathManager core = other.GetComponent<HeathManager>();
            if (core != null)
            {
                core.TakeDamage(_damage);
                Die();
            }        
        }
    }
    #endregion
}
