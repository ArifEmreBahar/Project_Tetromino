using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Range : PowerUp
{
    AllyManager _allyManager;
    Sprite _sprite = null;
    bool _canGet = true;
    string _descpEng = "Increases the range of all allies on the board";
    string _spritePath = "card_Range";

    public override string Description => _descpEng;
    public override string SpritePath => _spritePath;
    public override bool CanGet => _canGet;
    public override Sprite Sprite => _sprite;

    public PowerUp_Range(AllyManager allyManager)
    {
        _allyManager = allyManager;
        _sprite = Resources.Load<Sprite>(SpritePath);
    }

    public override void Activate()
    {
        base.Activate();

        _allyManager.ModifyRangeAll(1.5f);
        _canGet = false;
    }
}
