using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    private const float FADE_DURATION = 1;

    public CanvasGroup fadePanel;
    public Image loadingBarFill;
    public CanvasGroup loadingGraphics;

	private void Awake()
	{
        StartCoroutine(LoadLevelCorourine());
        loadingGraphics.alpha = 0;
	}

	private IEnumerator LoadLevelCorourine()
    {
        yield return StartCoroutine(FadePanel(true));

		SceneManager.UnloadSceneAsync(GameFlowController.previousScene);
		AsyncOperation loadLevel = SceneManager.LoadSceneAsync(GameFlowController.sceneToLoad,LoadSceneMode.Additive);

		loadingGraphics.alpha = GameFlowController.loadUsingLoadingBar?1:0;

		while (!loadLevel.isDone)
        {
			if (GameFlowController.loadUsingLoadingBar)
			{
				loadingBarFill.fillAmount = Mathf.Clamp01(loadLevel.progress / 0.9f);
			}
            yield return null;
        }

		loadingGraphics.alpha = 0;

		yield return StartCoroutine(FadePanel(false));

        SceneManager.UnloadSceneAsync("Loading Scene");
	}

    private IEnumerator FadePanel(bool fadeIn)
    {
        float startValue = fadePanel.alpha;
        float endValue = fadeIn ? 1 : 0;

        float duration = Mathf.Abs(startValue - endValue) * FADE_DURATION;
		float timer = 0;

        while (timer < 1)
        {
            yield return null;
            timer += Time.deltaTime / duration;

            fadePanel.alpha = Mathf.Lerp(startValue, endValue, timer);
        }

        fadePanel.alpha = endValue;
	}
}
