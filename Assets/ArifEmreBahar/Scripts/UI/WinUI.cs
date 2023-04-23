using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    [SerializeField]
    GameController _gameController;
    [SerializeField]
    Text _text;

    void OnEnable()
    {
        int win = (_gameController.GetMaxLevel() / _gameController.BuffStackDiv);

        if (win <= 0)
            SetActiveAll(false);
        else
        {
            _text.text = (_gameController.GetMaxLevel() / _gameController.BuffStackDiv).ToString();
            SetActiveAll(true);
        }            
    }

    void SetActiveAll(bool active)
    {
        foreach (Transform tr in transform)
        {
            tr.gameObject.SetActive(active);
        }
    }
}
