using UnityEngine;

public class PowerUp_Spearman : PowerUpAlly
{
    string _currentDescp = "";
    const string DESCP0_ENG = "Spearman throws a spear to the enemy.";
    const string DESCP1_ENG = "Collect 3:\n Spear+1 \n Damage++ \n Speed++";
    const string DESCP2_ENG = "Collect 6:\n Spear+1 \n Damage++ \n Speed++";
    const string SPRITE_PATH = "card_Spearman";

    public override string Description => _currentDescp;
    public override string SpritePath => SPRITE_PATH;

    public PowerUp_Spearman(BlockAlly ally, GameController manager) : base(ally, manager)
    {
        _currentDescp = DESCP0_ENG;
    }

    public override void Activate()
    {
        base.Activate();

        if (_ally.CurrentLevel == 1)
            _currentDescp = DESCP1_ENG;
        else if (_ally.CurrentLevel == 2)
            _currentDescp = DESCP2_ENG;
    }
}
