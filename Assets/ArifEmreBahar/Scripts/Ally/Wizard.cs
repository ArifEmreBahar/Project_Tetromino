using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : BlockAlly
{
    public ContinuousBullet _bulletPrefab;

    protected override void Setup()
    {
        base.Setup();

        _damage = 100f;
        _speed = 50f;
        _shotFreq = 3f;
        _detectionRadius = 1.5f;
    }

    protected override void Attack()
    {
        base.Attack();

        int numOfProjectile = 0;
        if (!_isSpecialtyUnlocked)
            numOfProjectile = _currentLevel == 1 ? 1 : 2;
        else
            numOfProjectile = 3;

        foreach (BlockEnemy enemy in GetClosestEnemies(numOfProjectile))
        {
            ContinuousBullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            bullet.Shoot(enemy.transform.position, _damage, _speed);
        }
    }

    protected override void SetLvlTwoBuffs()
    {
        _damage = 150;
        _speed = 60f;
        _shotFreq = 1.5f;
        _detectionRadius *= 1.33f;

        base.SetLvlTwoBuffs();
    }

    protected override void SetLvlThreeBuffs()
    {
        _damage = 200;
        _speed = 70f;
        _shotFreq = 0.5f;
        _detectionRadius *= 1.2f;

        base.SetLvlThreeBuffs();
    }
}
