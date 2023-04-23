using Tomino;
using UnityEngine;

public class PowerUp_PieceControl : PowerUp
{
    Game _game = null;
    Sprite _sprite = null;
    int _stack = 0;
    bool _canGet = true;
    string _currentDescp = "";
    const string DESCP0_ENG = "Improve Piece control";
    const string DESCP1_ENG = "Collect 2:\n Fall the piece instantly \n -Space-";
    const string SPRITE_PATH = "card_HelpingHand";

    public override string Description => _currentDescp;
    public override string SpritePath => SPRITE_PATH;
    public override bool CanGet => _canGet;
    public override Sprite Sprite => _sprite;

    public PowerUp_PieceControl(Game game)
    {
        _game = game;
        _currentDescp = DESCP0_ENG;
        _sprite = Resources.Load<Sprite>(SpritePath);
    }

    public override void Activate()
    {
        base.Activate();

        if (!_game.CanPieceMoveDown)
            _game.CanPieceMoveDown = true;
        else if(_stack >= 2 && !_game.CanPieceFall)
        {
            _game.CanPieceFall = true;
            _canGet = false;
        }

        _stack++;

        if (_stack == 1)
            _currentDescp = DESCP1_ENG;
    }
}
