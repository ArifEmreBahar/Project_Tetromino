using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBullet : Bullet
{
    int _hitStack = 0;
    int _maxHitStack = 3;
    float _hitRadius = 1f;
    float _renderTime = 0.33f;
    int _hitCount = 0;

    LineRenderer _lineRenderer;
    Coroutine _playHitWithDelay;
    List<BlockEnemy> _hitEnemies = new List<BlockEnemy>();

    protected override void OnHit(BlockEnemy enemy)
    {
        enemy.TakeDamage(_damage);
        _hitEnemies.Add(enemy);
        _hitStack++;
        SetLinePos(enemy.transform.position);

        if (_hitEnemies.Count >= _maxHitStack)
            Destroy(gameObject, _renderTime);
        else
            ShootNext(enemy.transform.position);

        if (_playHitWithDelay == null)
            _playHitWithDelay = StartCoroutine(PlayHitWithDelay());
    }

    protected override void PlayEfect()
    {
        AudioPlayer.Instance.PlayJoltFireClip();
    }

    public override void Shoot(Vector3 targetPosition, float damage, float speed)
    {
        _damage = damage;
        _speed = speed;
        _lineRenderer = GetComponent<LineRenderer>();
        SetLinePos(transform.position);
        ShootNext(targetPosition);
        PlayEfect();
    }

    void ShootNext(Vector3 targetPosition)
    {
        // Find all enemies that overlap with a circle centered at the current position of the bullet
        Collider2D[] hits = Physics2D.OverlapCircleAll(targetPosition, _hitRadius);

        // Find the closest enemy that is not in the _hitEnemies list
        BlockEnemy nextEnemy = null;
        float closestDistance = float.MaxValue;
        foreach (Collider2D hit in hits)
        {
            BlockEnemy enemy = hit.GetComponent<BlockEnemy>();
            if (enemy != null && !_hitEnemies.Contains(enemy))
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nextEnemy = enemy;
                }
            }
        }

        if (!nextEnemy)
        {
            Destroy(gameObject, _renderTime);
            return;
        }         

        OnHit(nextEnemy);
    }

    void SetLinePos(Vector3 pos)
    {
        _lineRenderer.positionCount = _hitStack + 1;
        _lineRenderer.SetPosition(_hitStack, pos);
    }

    IEnumerator PlayHitWithDelay()
    {
        while (_hitStack > 0)
        {
            yield return new WaitForSeconds(0.01f);
            AudioPlayer.Instance.PlayEnemyHitClip();
            _hitStack--;
        }
    }
}

