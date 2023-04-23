using UnityEngine;
using UnityEngine.UI;
using static Tomino.Game;

public class HeathManager : MonoBehaviour
{
    public Slider _slider = null;
    
    public event GameEventHandler DeadEvent = delegate { };

    float _currentHealth = 100f;
    float _maxHeath = 100f;

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        AudioPlayer.Instance.PlayPlayerDamageClip();
        UpdateSlider();

        if (_currentHealth <= 0)
            Die();
    }

    public void AddHeath(float amount)
    {
        _currentHealth += amount;

        if (_currentHealth > _maxHeath)
            _currentHealth = _maxHeath;
    }

    public void Die()
    {
        DeadEvent();
        AudioPlayer.Instance.PlayPlayerDeathClip();
    }

    void UpdateSlider()
    {
        if (_slider)
            _slider.value = _currentHealth;
    }
}
