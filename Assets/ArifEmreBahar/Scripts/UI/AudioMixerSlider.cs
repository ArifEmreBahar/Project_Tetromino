using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerSlider : MonoBehaviour
{
    string _parameterEfect = "Effect";
    string _parameterMusic = "Music";
    [SerializeField]
    Slider _sliderMusic;
    [SerializeField]
    Slider _sliderEffect;

    void Start()
    {
        // Load saved value from PlayerPrefs
        _sliderMusic.value = PlayerPrefs.GetFloat(_parameterMusic, 0.5f);
        _sliderEffect.value = PlayerPrefs.GetFloat(_parameterEfect, 0.5f);
        // Set audio mixer parameter to saved value
        AudioPlayer.Instance.audioMusic.volume = _sliderMusic.value;
        AudioPlayer.Instance.audioEffect.volume = _sliderEffect.value;
    }

    public void SetMusicLevel(float sliderValue)
    {
        // Set audio mixer parameter to slider value
        AudioPlayer.Instance.audioMusic.volume = sliderValue;

        // Save new value to PlayerPrefs
        PlayerPrefs.SetFloat(_parameterMusic, sliderValue);
        PlayerPrefs.Save();
    }

    public void SetEffectLevel(float sliderValue)
    {
        // Set audio mixer parameter to slider value
        AudioPlayer.Instance.audioEffect.volume = sliderValue;

        // Save new value to PlayerPrefs
        PlayerPrefs.SetFloat(_parameterEfect, sliderValue);
        PlayerPrefs.Save();
    }
}
