using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameFlowController 
{
	public static string sceneToLoad = "Main Game";
	public static string previousScene;
	public static bool loadUsingLoadingBar;


	public static void LoadScene(string sceneToLoad,bool useLoadingBar)
	{
		previousScene = SceneManager.GetActiveScene().name;
		GameFlowController.sceneToLoad = sceneToLoad;
		loadUsingLoadingBar = useLoadingBar;
		SceneManager.LoadSceneAsync("Loading Scene",LoadSceneMode.Additive);
	}

}
