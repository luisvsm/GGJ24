using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	//Keys for saving and loading the volume

	public static readonly string MasterVolumeSaveKey = "master_volume";
	public static readonly string MusicVolumeSaveKey = "music_volume";
	public static readonly string SfxVolumeSaveKey = "sfx_volume";

	//Keys for accessing the exposed variables on the audio mixer

	public static readonly string MasterVolumeMixerKey = "master_volume";
	public static readonly string MusicVolumeMixerKey = "music_volume";
	public static readonly string SfxVolumeMixerKey = "sfx_volume";

	//References

	//A static reference to this game object that can be called from any other active script
	public static AudioManager instance;

	//Background Audio sources
	private List<AudioSource> musicAudioSources;

	//A counter showing the current music Audio Source
	private int currentMusicAudioSource;

	//SFX Audio sources
	private List<AudioSource> sfxAudioSources;

	//A counter showing the current sfx Audio Source
	private int currentSfxSource;

	//Master audio mixer
	public AudioMixer masterMixer;


	// Called when the object first gets initialise
	void Awake()
	{
		//Look for all objects of type "AudioManager", and if there is more than one in the scene then this one is not the original, destroy this one.
		if (FindObjectsOfType<AudioManager>().Length > 1)
		{
			Destroy(gameObject);
			return;
		}

		//Load this version of the Audio Manager into the static reference
		instance = this;

		//Mark this game object as not to be destroyed when a new scene is loaded
		DontDestroyOnLoad(this);

		//Load any saved volume levels from player prefs

		//Get the saved Master volume level (returns 1 if nothing has been saved)
		float savedMasterVolumeLevel = PlayerPrefs.GetFloat(MasterVolumeSaveKey, 1);

		//Set the mixer volume of the master volume
		masterMixer.SetFloat(MasterVolumeMixerKey, savedMasterVolumeLevel);

		//Get the saved Music volume level (returns 1 if nothing has been saved)
		float savedMusicVolumeLevel = PlayerPrefs.GetFloat(MusicVolumeSaveKey, 1);
		masterMixer.SetFloat(MusicVolumeMixerKey, savedMusicVolumeLevel);

		//Get the saved SFX volume level (returns 1 if nothing has been saved)
		float savedSfxVolumeLevel = PlayerPrefs.GetFloat(SfxVolumeSaveKey, 1);
		masterMixer.SetFloat(SfxVolumeMixerKey, savedSfxVolumeLevel);

		//Create SFX audio soruces
		sfxAudioSources = new();

		for (int i = 0; i < 5; i++)
		{
			sfxAudioSources.Add(CreateAudioSource("SFX Audio Source " + i,"SFX"));
		}


		//Create Music audio soruces
		musicAudioSources = new();

		for (int i = 0; i < 2; i++)
		{
			musicAudioSources.Add(CreateAudioSource("Music Audio Source " + i, "Music",true));
		}

		AudioSource CreateAudioSource(string name, string mixerGroupName, bool loop = false)
		{
			GameObject newAudioSourceGameObject = new GameObject(name);
			newAudioSourceGameObject.transform.SetParent(transform);
			AudioSource newAudioSource = newAudioSourceGameObject.AddComponent<AudioSource>();

			newAudioSource.loop = loop;
			newAudioSource.dopplerLevel = 0;
			newAudioSource.playOnAwake = false;
			newAudioSource.outputAudioMixerGroup = masterMixer.FindMatchingGroups(mixerGroupName)[0];

			return newAudioSource;
		}

	}

	//Pause the Background Music
	public void PauseMusic()
	{
		musicAudioSources[currentMusicAudioSource].Pause();
	}

	//Play the Background Music
	public void PlayMusic()
	{
		sfxAudioSources[currentMusicAudioSource].Play();
	}


	//-----------------------------------------------------
	// Set Volume levels

	//Change the master volume level
	public void SetMasterVolume(float value)
	{
		//Change the volume on the mixer
		masterMixer.SetFloat(MasterVolumeMixerKey, value);

		//Save the volume to the player prefs
		PlayerPrefs.SetFloat(MasterVolumeSaveKey, value);
	}

	//Change the music volume level
	public void SetMusicVolume(float value)
	{
		//Change the volume on the mixer
		masterMixer.SetFloat(MusicVolumeMixerKey, value);

		//Save the volume to the player prefs
		PlayerPrefs.SetFloat(MusicVolumeSaveKey, value);
	}

	//Change the sfx music volume level
	public void SetSfxVolume(float value)
	{
		//Change the volume on the mixer
		masterMixer.SetFloat(SfxVolumeMixerKey, value);

		//Save the volume to the player prefs
		PlayerPrefs.SetFloat(SfxVolumeSaveKey, value);
	}


	//-----------------------------------------------------
	// Playing SFX

	public void PlaySFX(AudioClip clip,Vector3 position, float spatial = 1)
	{
		AudioSource selectedAudioSource = GetAvaliableAudioSource();

		selectedAudioSource.transform.position = position;

		PlaySFX(clip, spatial);
	}


	// Play a SFX clip with position
	public void PlaySFX(AudioClip clip, float spatial = 0)
	{
		AudioSource selectedAudioSource = GetAvaliableAudioSource();

		//Set the clip to play to the current sfx audio source
		selectedAudioSource.clip = clip;

		//Set the spacial setting
		selectedAudioSource.spatialBlend = spatial;

		//Play the clip
		selectedAudioSource.Play();
	}

	// Play a SFX clip 
	private AudioSource GetAvaliableAudioSource()
	{
		//Increment the current SFX counter
		currentSfxSource++;

		//If the counter gets longer than the number of audio sources, the reset to 0
		if (currentSfxSource >= sfxAudioSources.Count)
		{
			currentSfxSource = 0;
		}

		return sfxAudioSources[currentSfxSource];
	}


	//-----------------------------------------------------
	// Switching Music

	//Switch the current music playing
	public void SwitchMusicClip(AudioClip newAudioClip)
	{
		//Fade the current audio source out
		StartCoroutine(FadeSongOut(musicAudioSources[currentMusicAudioSource]));

		//Increment the current Music counter
		currentMusicAudioSource = (currentMusicAudioSource + 1)% musicAudioSources.Count;

		//Fade the new audio source in
		StartCoroutine(FadeSongIn(musicAudioSources[currentMusicAudioSource], newAudioClip));
	}

	//Coroutine to fade in a song
	private IEnumerator FadeSongIn(AudioSource audioSource, AudioClip song)
	{
		//Set the audiosource volume to 0
		audioSource.volume = 0;

		//Set the audio source's clip to the song we want to play
		audioSource.clip = song;

		//Start playing the song
		audioSource.Play();

		//Timer to track the transition from 0 to 1
		float timer = 0;

		//Loop until timer gets over 1
		while (timer < 1)
		{
			//Add the time that has passed since the last frame to the timer
			timer += Time.deltaTime;

			//Set the audioSource's volume to be whatever the timer currently is
			audioSource.volume = timer;

			//Wait till the next frame to loop around again.
			yield return null;
		}

		//When we get here the timer should have gone from 0 to 1 over a 1 second.

		//Set the audiosource volume to 1 to ensure its set correctly
		audioSource.volume = 1;
	}

	//Coroutine to fade out a song
	private IEnumerator FadeSongOut(AudioSource audioSource)
	{
		//Set the timer to whatever the audiosource currently is
		float timer = audioSource.volume;

		//Loop until timer gets below or equal to  0
		while (timer > 0)
		{

			//Remove the time that has passed since the last frame from the timer
			timer -= Time.deltaTime;

			//Set the audioSource's volume to be whatever the timer currently is
			audioSource.volume = timer;

			//Wait till the next frame to loop around again.
			yield return null;
		}

		//When we get here the timer should have gone from 1 to 0 over a 1 second.

		//Stop the audiosource from playing music
		audioSource.Stop();
	}
}