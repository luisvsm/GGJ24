using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public AudioClip musicClip;
    public OptionsPanel optionsPanel;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.SwitchMusicClip(musicClip);
        optionsPanel.Hide();

	}

    public void OnStartButtonPress()
    {
		GameFlowController.LoadScene("Main Game", false);
	}

    public void OnExitButtonPress()
    {
        Application.Quit();
    }

    public void OnOptionsButtonPress()
    {
        optionsPanel.Show();

	}
}
