using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameFlowController 
{
	public static string sceneToLoad = "Main Game";

	public static void LoadScene(string sceneToLoad)
	{
		GameFlowController.sceneToLoad = sceneToLoad;
		SceneManager.LoadScene("Loading Scene");
	}

}
