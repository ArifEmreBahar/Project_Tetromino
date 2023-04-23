using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpLayoutFixer : MonoBehaviour
{
    [SerializeField]
    List<Transform> _close = null;
    [SerializeField]
    List<Transform> _open = null;

    public void FixLayout()
    {
        foreach (Transform transfrom in _close)
        {
            transfrom.gameObject.SetActive(false);
        }
        foreach (Transform transfrom in _open)
        {
            transfrom.gameObject.SetActive(true);
        }
    }
}
