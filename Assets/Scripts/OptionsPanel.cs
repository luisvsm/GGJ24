using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;

    public void Show(bool instant = false)
    {
        gameObject.SetActive(true);

        masterVolumeSlider.value = AudioManager.instance.GetMasterVolume();
        sfxVolumeSlider.value = AudioManager.instance.GetSfxVolume();
        musicVolumeSlider.value = AudioManager.instance.GetMusicVolume();
	}

    public void Hide(bool instant = false)
    {
		gameObject.SetActive(false);
	}

    public void OnMasterVolumeChange(float value)
    {
        AudioManager.instance.SetMasterVolume(value);
	}

	public void OnSfxVolumeChange(float value)
	{
        AudioManager.instance.SetSfxVolume(value);
	}

	public void OnMusicVolumeChange(float value)
	{
        AudioManager.instance.SetMusicVolume(value);
	}

    public void OnClearSaveDataButtonPress()
    {
        SaveController.ClearData();
        GameFlowController.LoadScene("Main Menu",false);
    }

    public void OnBackButtonPress()
    {
        Hide();
	}
}
