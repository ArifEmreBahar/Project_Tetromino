using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Display : PowerUp
{
    PowerUpLayoutFixer _fixer = null;
    Sprite _sprite = null;
    bool _canGet = true;
    string _descpEng = "Makes some helper panels visible";
    string _spritePath = "card_Display";

    public override string Description => _descpEng;
    public override string SpritePath => _spritePath;
    public override bool CanGet => _canGet;
    public override Sprite Sprite => _sprite;

    public PowerUp_Display(PowerUpLayoutFixer fixer)
    {
        _fixer = fixer;
        _sprite = Resources.Load<Sprite>(SpritePath);
    }

    public override void Activate()
    {
        base.Activate();

        _fixer.FixLayout();
        _canGet = false;
    }
}
