using UnityEngine;

public class PowerUpAlly : PowerUp
{
    GameController _manager;
    Sprite _sprite = null;
    bool _canGet = true;
    bool _isInstantiated = false;
    string _descpEng = "Desciption in English";
    string _spritePath = "card_Default";

    protected BlockAlly _ally = null;

    public override string Description => _descpEng;
    public override string SpritePath => _spritePath;
    public override bool CanGet => _canGet;
    public override Sprite Sprite => _sprite;

    public PowerUpAlly(BlockAlly ally, GameController manager)
    {
        _ally = ally;
        _manager = manager;
        _sprite = Resources.Load<Sprite>(SpritePath);
    }

    public override void Activate()
    {
        base.Activate();

        if (!_isInstantiated)
        {
            AllySlot emptySlot = _manager.slotManager.GetEmptySlot();
            _ally = GameObject.Instantiate(_ally, emptySlot.transform);
            emptySlot.IsEmpty = false;
            _manager.allyManager.AddAlly(_ally);
            _isInstantiated = true;
        }
        else
        {
            _ally.Upgrade();
            if (_ally.IsSpecialtyUnlocked)
                _canGet = false;
        }
    }
}
