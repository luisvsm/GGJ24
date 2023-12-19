using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    /*
     * Scene for the main menu
     * 
     */

    public AudioClip musicClip;
    public OptionsPanel optionsPanel;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.SwitchMusicClip(musicClip);
        optionsPanel.Hide();
	}

    // When the player presses the start button
    public void OnStartButtonPress()
    {
        // Load in the Main Game scene
		GameFlowController.LoadScene("Main Game", false);
	}

    // When the player presses the exit button
    public void OnExitButtonPress()
    {
        // Game quits
        Application.Quit();
    }

    // When the player presses the options button
    public void OnOptionsButtonPress()
    {
        // Show the options panel
        optionsPanel.Show();
	}
}
