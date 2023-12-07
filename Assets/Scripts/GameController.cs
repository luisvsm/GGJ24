using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public AudioClip musicClip;
	public GameObject pausePanel;

	// Start is called before the first frame update
	void Start()
    {
        AudioManager.instance.SwitchMusicClip(musicClip);
		pausePanel.SetActive(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (pausePanel.activeSelf)
			{
				UnPauseGame();
			}
			else
			{
				PauseGame();
			}
		}
	}

	public void PauseGame()
	{
		Time.timeScale = 0;
		pausePanel.SetActive(true);
	}

	public void UnPauseGame()
	{
		Time.timeScale = 1;
		pausePanel.SetActive(false);
	}

	public void OnResumeButtonPress()
	{
		UnPauseGame();
	}

	public void OnMainMenuButtonPress()
	{
		UnPauseGame();
		SceneManager.LoadScene("Main Menu");
	}

	public void OnExitButtonPress()
	{
		Application.Quit();
	}
}
