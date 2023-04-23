namespace Tomino
{
    /// <summary>
    /// Controls the game logic by handling user input and updating the board state.
    /// </summary>
    public class Game
    {
        public delegate void GameEventHandler();

        /// <summary>
        /// The event triggered when the game is resumed.
        /// </summary>
        public event GameEventHandler ResumedEvent = delegate { };

        /// <summary>
        /// The event triggered when the game is paused.
        /// </summary>
        public event GameEventHandler PausedEvent = delegate { };

        /// <summary>
        /// The event triggered when the game is finished.
        /// </summary>
        public event GameEventHandler FinishedEvent = delegate { };

        /// <summary>
        /// The event triggered when the piece is moved.
        /// </summary>
        public event GameEventHandler PieceMovedEvent = delegate { };

        /// <summary>
        /// The event triggered when the piece is rotated.
        /// </summary>
        public event GameEventHandler PieceRotatedEvent = delegate { };

        /// <summary>
        /// The event triggered when the piece finishes falliing.
        /// </summary>
        public event GameEventHandler PieceFinishedFallingEvent = delegate { };

        /// Other Events
        public event GameEventHandler PieceFallingEvent = delegate { };

        public event GameEventHandler PieceMovedDownEvent = delegate { };

        public event GameEventHandler LevelUpEvent = delegate { };
        public event GameEventHandler LineClearEvent = delegate { };
        int currentLevel = 1;
        int currentLine = 0;

        /// Other Global
        public bool CanPieceFall { get => canPieceFall; set => canPieceFall = value; }
        public bool CanPieceMoveDown { get => canPieceMoveDown; set => canPieceMoveDown = value; }

        bool canPieceFall = false;
        bool canPieceMoveDown = false;

        public Board Board { get => board; }

        /// <summary>
        /// The current score.
        /// </summary>
        public Score Score { get; private set; }

        /// <summary>
        /// The current level.
        /// </summary>
        public Level Level { get; private set; }

        public float ElapsedTime { get => elapsedTime; private set => elapsedTime = value; }

        readonly Board board;
        readonly IPlayerInput input;

        PlayerAction? nextAction = null;
        float elapsedTime;
        bool isPlaying;

        /// <summary>
        /// Creates a game with specified board and input.
        /// </summary>
        /// <param name="board">The board on which the blocks will be placed.</param>
        /// <param name="input">The input used for pooling player events.</param>
        public Game(Board board, IPlayerInput input)
        {
            this.board = board;
            this.input = input;
            PieceFinishedFallingEvent += input.Cancel;
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void Start()
        {
            isPlaying = true;
            ResumedEvent();
            elapsedTime = 0;
            Score = new Score();
            Level = new Level();
            board.RemoveAllBlocks();
            AddPiece();
        }

        public void Reset()
        {
            isPlaying = true;
            elapsedTime = 0;
            board.RemoveAllBlocks();
            AddPiece();
        }

        /// <summary>
        /// Resumes paused game.
        /// </summary>
        public void Resume()
        {
            isPlaying = true;
            ResumedEvent();
        }

        /// <summary>
        /// Pauses started game.
        /// </summary>
        public void Pause()
        {
            isPlaying = false;
            PausedEvent();
        }

        /// <summary>
        /// Sets the player action that the game should process in the next update.
        /// </summary>
        /// <param name="action">The next player action to process.</param>
        public void SetNextAction(PlayerAction action)
        {
            nextAction = action;
        }

        void AddPiece()
        {
            board.AddPiece();
            if (board.HasCollisions())
            {
                isPlaying = false;
                PausedEvent();
                FinishedEvent();
            }
        }

        /// <summary>
        /// Updates the game by processing user input.
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            if (!isPlaying) return;

            input.Update();

            var action = input?.GetPlayerAction();
            if (action.HasValue)
            {
                HandlePlayerAction(action.Value);
            }
            else if (nextAction.HasValue)
            {
                HandlePlayerAction(nextAction.Value);
                nextAction = null;
            }
            else
            {
                HandleAutomaticPieceFalling(deltaTime);
            }
        }

        void HandleAutomaticPieceFalling(float deltaTime)
        {
            elapsedTime += deltaTime;
            if (elapsedTime >= Level.FallDelay)
            {
                PieceFallingEvent();
                if (!board.MovePieceDown())
                {
                    PieceFinishedFalling();
                }
                ResetElapsedTime();
            }
        }

        void HandlePlayerAction(PlayerAction action)
        {
            var pieceMoved = false;
            switch (action)
            {
                case PlayerAction.MoveLeft:
                    pieceMoved = board.MovePieceLeft();
                    break;

                case PlayerAction.MoveRight:
                    pieceMoved = board.MovePieceRight();
                    break;

                case PlayerAction.MoveDown:
                    if (!canPieceMoveDown) break;
                    ResetElapsedTime();
                    if (board.MovePieceDown())
                    {
                        pieceMoved = true;
                        PieceMovedDownEvent();
                        Score.PieceMovedDown();
                    }
                    else
                    {
                        PieceFinishedFalling();
                    }
                    break;

                case PlayerAction.Rotate:
                    var didRotate = board.RotatePiece();
                    if (didRotate) PieceRotatedEvent();
                    break;

                case PlayerAction.Fall:
                    if (!canPieceFall) break;
                    Score.PieceFinishedFalling(board.FallPiece());
                    ResetElapsedTime();
                    PieceFinishedFalling();
                    break;
            }
            if (pieceMoved)
            {
                PieceMovedEvent();
            }
        }

        void PieceFinishedFalling()
        {
            PieceFinishedFallingEvent();
            int rowsCount = board.RemoveFullRows();
            Score.RowsCleared(rowsCount);
            Level.RowsCleared(rowsCount);
            AddPiece();

            if (currentLevel < Level.Number)
                for (int i = currentLevel; i < Level.Number; i++)
                    LevelUpEvent();

            if (currentLine < Level.Lines)
                LineClearEvent();

            currentLevel = Level.Number;
            currentLine = Level.Lines;
        }

        void ResetElapsedTime()
        {
            elapsedTime = 0;
        }
    }
}
