using UnityEngine;
using Tomino;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    #region VARIABLES
    public Camera currentCamera;
    public Game game;
    public BoardView boardView;
    public PieceView nextPieceView;
    public ScoreView scoreView;
    public LevelView levelView;
    public AlertView alertView;
    public SettingsView settingsView;
    //public AudioPlayer audioPlayer;
    public GameObject screenButtons;
    //public AudioSource musicAudioSource;
    [Space]
    public EnemyManager enemyManager;
    public AllyManager allyManager;
    public PowerUpManager powerUpManager;
    public HeathManager heathManager;
    public UIManager uIManager;
    public SlotManager slotManager;
    public PowerUpLayoutFixer powerUpLayoutFixer;
    public Transform cardPanel;
    public Transform uiPanel;

    UniversalInput universalInput;

    bool isPlaying = false;
    bool isCardDisplayed = true;
    bool isGameStarted = false;
    int boardWidth = 10;
    int boardHeight = 10;
    int displayStack = 0;
    int buffStackDiv = 5;
    const string MAX_LEVEL_PREF = "MAX_LEVEL_PREF";

    Coroutine consecutiveDisplay = null;
    #endregion

    #region PROPERTIES

    public int BuffStackDiv { get => buffStackDiv; }

    #endregion

    #region UNITY

    void Awake()
    {
        HandlePlayerSettings();
        Settings.ChangedEvent += HandlePlayerSettings;

        Board board = new Board(boardWidth, boardHeight);

        boardView.SetBoard(board);
        nextPieceView.SetBoard(board);

        universalInput = new UniversalInput(new KeyboardInput(), boardView.touchInput);

        game = new Game(board, universalInput);
        game.FinishedEvent += OnTetrisFinished;
        game.PieceFinishedFallingEvent += AudioPlayer.Instance.PlayPieceDropClip;
        game.PieceRotatedEvent += AudioPlayer.Instance.PlayPieceRotateClip;
        game.PieceMovedEvent += AudioPlayer.Instance.PlayPieceMoveClip;
        game.LineClearEvent += AudioPlayer.Instance.PlayLineClearClip;
        game.LevelUpEvent += OnLevelUp;
        game.PieceMovedDownEvent += OnPieceMovedDown;
        game.PieceFallingEvent += OnPieceFalling;

        scoreView.game = game;
        levelView.game = game;

        enemyManager.SetGameBoard(boardWidth, boardHeight);
        
        heathManager.DeadEvent += OnGameFinished;

        powerUpManager.DisplayEvent += OnCardDisplay;
        powerUpManager.CardSelectEvent += OnCardSelect;

        uIManager.OnStartButtonEvent += StartGame;
        uIManager.OnEscapeButtonEvent += OnEscapeButtonTap;

        Time.timeScale = 0;
        
    }

    void Update()
    {
        game.Update(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.U))
            game.Level.RowsCleared(1);
    }

    #endregion

    #region PUBLIC

    public int GetMaxLevel()
    {
        return PlayerPrefs.GetInt(MAX_LEVEL_PREF);
    }

    public int GetBuffStack()
    {
        return PlayerPrefs.GetInt(MAX_LEVEL_PREF) / buffStackDiv;
    }
    #endregion

    #region PRIVATE

    void StartGame()
    {
        if (!isGameStarted)
        {
            game.Start();
            HandleLevelUpgrades();
        }         
        else
            ResumeGame();

        if (!powerUpManager.FirstDraw)
            powerUpManager.DisplayPowerUps();

        isGameStarted = true;
    }

    void PauseGame()
    {
        // To Pause the game without freezing, code here.

        //game.Pause(); //enemyManager.PauseAll();   //allyManager.PauseAll(); //ShowPauseView();

        Time.timeScale = 0;
        isPlaying = false;
    }

    void ResumeGame()
    {
        // To Resume the game without freezing, code here.

        //game.Resume(); //enemyManager.ResumeAll();  //allyManager.ResumeAll();  //ShowPauseView();

        Time.timeScale = 1;
        isPlaying = true;
    }

    void OnPieceFalling()
    {
        enemyManager.SpawnEnemyStack();
        enemyManager.MoveAll();
    }

    void OnPieceMovedDown()
    {
        enemyManager.SpawnEnemyStack();
        enemyManager.MoveAll();
    }

    void OnLevelUp()
    {
        if (consecutiveDisplay == null)
            consecutiveDisplay = StartCoroutine(ConsecutiveDisplay());

        displayStack++;

        if(PlayerPrefs.GetInt(MAX_LEVEL_PREF) <= game.Level.Number)
            PlayerPrefs.SetInt(MAX_LEVEL_PREF, game.Level.Number);
    }

    void OnCardDisplay()
    {
        PauseGame();
        isCardDisplayed = true;
        cardPanel.gameObject.SetActive(true);
    }

    void OnCardSelect()
    {
        ResumeGame();
        isCardDisplayed = false;
        StartCoroutine(LateDisable());
    }

    void OnEscapeButtonTap()
    {
        if (isPlaying)
            PauseGame();
        else if (!isCardDisplayed)
            ResumeGame();

        uiPanel.gameObject.SetActive(!isPlaying);
    }

    void OnMoveLeftButtonTap()
    {
        game.SetNextAction(PlayerAction.MoveRight);
    }

    void OnMoveRightButtonTap()
    {
        game.SetNextAction(PlayerAction.MoveLeft);
    }

    void OnMoveDownButtonTap()
    {
        if(isPlaying)
            game.SetNextAction(PlayerAction.MoveDown);
    }

    void OnFallButtonTap()
    {
        if (isPlaying)
            game.SetNextAction(PlayerAction.Fall);
    }

    void OnRotateButtonTap()
    {
        game.SetNextAction(PlayerAction.Rotate);
    }

    void OnGameFinished()
    {
        isGameStarted = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Note Later: To reset the game without restarting the scene, code here.
        //enemyManager.DestroyAll();  //allyManager.DestroyAll();   //game.Start();  //game.Pause();
    }

    void OnTetrisFinished()
    {
        game.Reset();
    }

    void OnDisplayCards()
    {
        Time.timeScale = 0;

        //Note Later: To display the game without freezing the scene, code here.
        //game.Pause(); //enemyManager.PauseAll();
    }

    void HandlePlayerSettings()
    {
        screenButtons.SetActive(Settings.ScreenButonsEnabled);
        boardView.touchInput.Enabled = !Settings.ScreenButonsEnabled;
        //musicAudioSource.gameObject.SetActive(Settings.MusicEnabled);
    }

    void HandleLevelUpgrades()
    {
        int buffCound = PlayerPrefs.GetInt(MAX_LEVEL_PREF) / buffStackDiv;

        for (int i = 0; i < buffCound + 1; i++)
            OnLevelUp();
    }

    IEnumerator LateDisable()
    {
        yield return null;
        cardPanel.gameObject.SetActive(false);
    }

    IEnumerator ConsecutiveDisplay()
    {
        powerUpManager.DisplayPowerUps();
        enemyManager.SetLevel(game.Level.Number);
        AudioPlayer.Instance.PlayLevelUpClip();

        while (cardPanel.gameObject.activeSelf)
            yield return null;

        displayStack--;

        if (displayStack > 0)
            yield return consecutiveDisplay = StartCoroutine(ConsecutiveDisplay());
        else
        {
            consecutiveDisplay = null;
            StopCoroutine(ConsecutiveDisplay());
        }         
    }

    #endregion
}
