using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private const float FADE_TIME = 0.5f;

    public AudioClip musicClip;
    public CanvasGroup fadePanel;
    public OptionsPanel optionsPanel;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.SwitchMusicClip(musicClip);
        optionsPanel.Hide();

	}

    public void OnStartButtonPress()
    {
        fadePanel.gameObject.SetActive(true);
        fadePanel.alpha = 0;
        StartCoroutine(fadeCoroutine());

		IEnumerator fadeCoroutine()
        {
            float timer = 0;

            while (timer < 1)
            {
                timer += Time.deltaTime / FADE_TIME;
				fadePanel.alpha = timer;
                yield return null;
			}

			fadePanel.alpha = 1;
			GameFlowController.LoadScene("Main Game");
		}
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
