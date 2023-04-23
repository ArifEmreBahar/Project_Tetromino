using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PowerUpCard : MonoBehaviour
{
    [SerializeField]
    Image _image = null;
    [SerializeField]
    Text _description = null;
    [SerializeField]
    Button _button = null;
    PowerUp _powerUp = null;

    public PowerUp PowerUp { get => _powerUp; }
    public Image Image { get => _image; }
    public Text Description { get => _description; }

    public void SetCard(PowerUp powerUp)
    {
        _powerUp = powerUp;
        _image.sprite = powerUp.Sprite;
        _description.text = powerUp.Description;
    }

    public void AddAction(UnityAction<PowerUpCard> buttonAction)
    {
        _button.onClick.AddListener(() => buttonAction(this));
    }

    public void AddAction(UnityAction buttonAction)
    {
        _button.onClick.AddListener(buttonAction);
    }

    public void ClearListeners()
    {
        _button.onClick.RemoveAllListeners();
    }

    public void DisableCard()
    {
        gameObject.SetActive(false);
    }
}
