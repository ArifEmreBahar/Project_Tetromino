using System.Collections.Generic;
using UnityEngine;
using static Tomino.Game;

public class PowerUpManager : MonoBehaviour
{
    #region VARIABLES
    [SerializeField]
    GameController _gameController;
    [SerializeField]
    AllyHolder _allyHolder;
    [SerializeField]
    List<PowerUpCard> _powerUpCards;


    public event GameEventHandler DisplayEvent = delegate { };
    public event GameEventHandler CardSelectEvent = delegate { };

    List<PowerUp> _powerUps;
    List<PowerUp> _lastPowerUps = new List<PowerUp>();

    bool _firstDraw = false;
    #endregion

    #region GET & SET
    public bool FirstDraw { get => _firstDraw; }
    #endregion

    #region UNITY
    void Awake()
    {
        FillPowerUpList();
    }
    
    void Update()
    {
       
    }
    #endregion

    #region PUBLIC
    public void DisplayPowerUps()
    {
        if (_powerUpCards.Count <= 0) return;

        List<PowerUp> powerUps;

        if (_firstDraw)
            powerUps = GetRandomPowerUps(_powerUpCards.Count);
        else
            powerUps = GetStarterAllyPowerUps(_powerUpCards.Count);


        for (int i = 0; i < _powerUpCards.Count; i++)
        {
            _powerUpCards[i].ClearListeners();
            _powerUpCards[i].SetCard(powerUps[i]);
            _powerUpCards[i].AddAction(powerUps[i].Activate);
            _powerUpCards[i].AddAction(OnCardSelect);
        }

        _firstDraw = true;
        DisplayEvent();
    }
    #endregion

    #region PRIVATE
    void FillPowerUpList()
    {
        _powerUps = new List<PowerUp>
        {
            new PowerUp_Spearman(_allyHolder.Spearman, _gameController),
            new PowerUp_Wizard(_allyHolder.Wizard, _gameController),
            new PowerUp_Prisoner(_allyHolder.Prisoner, _gameController),
            new PowerUp_Display(_gameController.powerUpLayoutFixer),
            new PowerUp_RenderShadow(_gameController.boardView),
            new PowerUp_PieceControl(_gameController.game),
            new PowerUp_Range(_gameController.allyManager)
            //new RangePowerUp("Range Up", 10f, 5f),
            //new DamagePowerUp("Damage Up", 10f, 10f),
            //new CoinPowerUp("Coin Boost", 10f, 100),
            //new AllyPowerUp("New Ally", 10f, new Ally())
        };
    }

    List<PowerUp> GetRandomPowerUps(int count)
    {
        List<PowerUp> powerUps = new List<PowerUp>();

        for (int i = 0; i < count; i++)
        {
            //Note Optimization: Random Number without repeat.
            PowerUp randomPowerUp = _powerUps[UnityEngine.Random.Range(0, _powerUps.Count)];

            if (!powerUps.Contains(randomPowerUp))
                powerUps.Add(randomPowerUp);
            else
                i--;
        }

        return powerUps;
    }

    List<PowerUp> GetStarterAllyPowerUps(int count)
    {
        List<PowerUp> powerUps = new List<PowerUp>();

        for (int i = 0; i < count; i++)
            powerUps.Add(_powerUps[i]);

        return powerUps;
    }

    void OnCardSelect(PowerUpCard powerUpCard)
    {
        if (!powerUpCard.PowerUp.CanGet)
            _powerUps.Remove(powerUpCard.PowerUp);

        HandleCards();
        CardSelectEvent();
    }

    void HandleCards()
    {
        if (_powerUpCards.Count <= _powerUps.Count) return;

        _powerUpCards[0].DisableCard();
        _powerUpCards.RemoveAt(0);
    }
    #endregion
}
