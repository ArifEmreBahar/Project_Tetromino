using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prisoner : BlockAlly
{
    public ChainBullet _bulletPrefab;

    protected override void Setup()
    {
        base.Setup();

        _damage = 80f;
        _speed = 50f;
        _shotFreq = 1f;
    }

    protected override void Attack()
    {
        base.Attack();

        int numOfProjectile = 0;
        if (!_isSpecialtyUnlocked)
            numOfProjectile = _currentLevel == 1 ? 1 : 2;
        else
            numOfProjectile = 3;

        //Create a new bullet and shoot it at the enemy
        foreach (BlockEnemy enemy in GetClosestEnemies(numOfProjectile))
        {
            ChainBullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            bullet.Shoot(enemy.transform.position, _damage, _speed);
        }
    }

    protected override void SetLvlTwoBuffs()
    {
        _damage = 150;
        _shotFreq = 0.8f;

        base.SetLvlTwoBuffs();
    }

    protected override void SetLvlThreeBuffs()
    {
        _damage = 250;
        _shotFreq = 0.6f;

        base.SetLvlThreeBuffs();
    }
}
