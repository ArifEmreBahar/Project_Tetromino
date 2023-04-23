using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFeel : MonoBehaviour
{
    public void OnButtonHover()
    {
        AudioPlayer.Instance.PlayButtonHoverClip();
    }

    public void OnButtonClick()
    {
        AudioPlayer.Instance.PlayButtonClickClip();
    }

    public void OnCardHover()
    {
        AudioPlayer.Instance.PlayCardHoverClip();
    }

    public void OnCardClick()
    {
        AudioPlayer.Instance.PlayCardClickClip();
    }
}
