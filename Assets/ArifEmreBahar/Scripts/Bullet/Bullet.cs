using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected float _damage = 10f;
    protected float _speed = 100f;
    protected float _lifeTime = 5f;

    bool _isFired = false;
    Vector3 _targetPos = Vector3.zero;

    Rigidbody2D rigidBody = null;

    //public float Damage { get => _damage; set => _damage = value; }
    //public float Speed { get => _speed; set => _speed = value; }
    //public float LifeTime { get => _lifeTime; set => _lifeTime = value; }
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public virtual void Shoot(Vector3 targetPosition, float damage, float speed)
    {
        Destroy(gameObject, _lifeTime);

        if (!rigidBody) return;

        _damage = damage;
        _speed = speed;

        _isFired = true;

        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, directionToTarget);
        // Apply force towards the target at the specified speed
        rigidBody.AddForce(directionToTarget * _speed, ForceMode2D.Force);

        PlayEfect();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // If we hit an enemy, damage it and destroy ourselves
        BlockEnemy enemy = collision.GetComponent<BlockEnemy>();
        if (enemy != null)
            OnHit(enemy);
    }

    protected virtual void OnHit(BlockEnemy enemy)
    {
        AudioPlayer.Instance.PlayEnemyHitClip();
        enemy.TakeDamage(_damage);
        Destroy(gameObject);
    }

    protected virtual void PlayEfect()
    {
        AudioPlayer.Instance.PlayBulletFireClip();
    }
}