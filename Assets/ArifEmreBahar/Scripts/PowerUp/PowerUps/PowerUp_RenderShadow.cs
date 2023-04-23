using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_RenderShadow : PowerUp
{
    BoardView _boardView = null;
    Sprite _sprite = null;
    bool _canGet = true;
    string _descpEng = "This enables rendering of piece shadow";
    string _spritePath = "card_PieceShadow";

    public override string Description => _descpEng;
    public override string SpritePath => _spritePath;
    public override bool CanGet => _canGet;
    public override Sprite Sprite => _sprite;

    public PowerUp_RenderShadow(BoardView boardView)
    {
        _boardView = boardView;
        _sprite = Resources.Load<Sprite>(SpritePath);
    }

    public override void Activate()
    {
        base.Activate();

        _boardView.IsRenderPieceShadow = true;
        _canGet = false;
    }
}
