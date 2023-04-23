using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearman : BlockAlly
{
    public Bullet _bulletPrefab;

    protected override void Setup()
    {
        base.Setup();

        _damage = 100f;
        _speed = 300f;
        _shotFreq = 1.33f;
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
            Bullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            bullet.Shoot(enemy.transform.position, _damage, _speed);
        }
    }

    protected override void SetLvlTwoBuffs()
    {
        _damage = 200;
        _shotFreq = 0.9f;

        base.SetLvlTwoBuffs();
    }

    protected override void SetLvlThreeBuffs()
    {              
        _damage = 300;
        _shotFreq = 0.6f;

        base.SetLvlThreeBuffs();
    }

    protected override void EnableSpeciality()
    {
        base.EnableSpeciality();
        _detectionRadius = _detectionRadius % 10f;
    }
}
