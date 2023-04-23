using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousBullet : Bullet
{
    int _hitStack = 0;
    int _maxHitStack = 5;

    protected override void OnHit(BlockEnemy enemy)
    {
        _hitStack++;
        enemy.TakeDamage(_damage);

        if(_hitStack == _maxHitStack)
            Destroy(gameObject);

        AudioPlayer.Instance.PlayEnemyHitClip();
    }

    protected override void PlayEfect()
    {
        AudioPlayer.Instance.PlaySpellFireClip();
    }
}
