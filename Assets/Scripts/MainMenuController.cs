using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public AudioClip musicClip;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.SwitchMusicClip(musicClip);
    }

    public void OnStartButtonPress()
    {
        SceneManager.LoadScene("Main Game");
    }

    public void OnExitButtonPress()
    {
        Application.Quit();
    }

}
