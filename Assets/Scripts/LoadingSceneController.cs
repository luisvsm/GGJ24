using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    public Image loadingBarFill;

	private void Awake()
	{
        StartCoroutine(LoadLevelCorourine());
	}

	private IEnumerator LoadLevelCorourine()
    {
        AsyncOperation loadLevel = SceneManager.LoadSceneAsync(GameFlowController.sceneToLoad);

        while (!loadLevel.isDone)
        {
            loadingBarFill.fillAmount = Mathf.Clamp01(loadLevel.progress / 0.9f);
            yield return null;
        }
    }
}
