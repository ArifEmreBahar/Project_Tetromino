using UnityEngine;

public class PowerUp_Wizard : PowerUpAlly
{
    string _currentDescp = "";
    const string SPRITE_PATH = "card_Wizard";
    const string DESCP0_ENG = "Wizard casts a magic ball to the enemy.";
    const string DESCP1_ENG = "Collect 3:\n Ball+1 \n Damage++ \n Speed++";
    const string DESCP2_ENG = "Collect 6:\n Ball+1 \n Damage++ \n Speed++";

    public override string Description => _currentDescp;
    public override string SpritePath => SPRITE_PATH;

    public PowerUp_Wizard(BlockAlly ally, GameController manager) : base(ally, manager)
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
