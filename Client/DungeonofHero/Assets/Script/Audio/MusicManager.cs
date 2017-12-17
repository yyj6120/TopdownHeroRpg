using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
//This class manages the playing of all music in the game. It assumes that music swaps when scenes are transitioned.
public class MusicManager : PersistentSingleton<MusicManager>
{
    //The menu music track
    [SerializeField]
    protected AudioClip menuMusic;

    //Field to represent the dedicated audiosource for this manager.
    private AudioSource musicSource;

    public AudioSource MusicSource
    {
        get { return musicSource; }
    }

    //Property to allow the start of music to be delayed.
    [SerializeField]
    protected float startDelay = 0.5f;

    //Property to set the rate that music fades in. If 0, music starts at full volume.
    [SerializeField]
    protected float fadeRate = 2f;

    //Audiosource volume on instantiation, in case we want to tweak the volume directly in editor.
    private float originalVolume;

    //Proportion of fading.
    private float fadeLevel = 1f;

    //Get references to the local audiosource on start, subscribe to the scene manager change event, and start the menu music (since Start will occur in the main menu).
    protected void Start()
    {
        musicSource = GetComponent<AudioSource>();
        SceneManager.activeSceneChanged += OnSceneChanged;

        originalVolume = musicSource.volume;

        PlayMusic(menuMusic);
    }

    //Volume fade-in logic happens here, assuming the relevant parameters are set.
    protected void Update()
    {
        if (fadeLevel < 1f && fadeRate > 0f)
        {
            fadeLevel = Mathf.Lerp(fadeLevel, 1f, Time.deltaTime * fadeRate);

            if (fadeLevel >= 0.99f)
            {
                fadeLevel = 1f;
            }

            musicSource.volume = originalVolume * fadeLevel;
        }
    }

    //This method is subscribed to the activeSceneChanged event, and will fire whenever the scene changes.
    private void OnSceneChanged(Scene scene1, Scene newScene)
    {
        if (musicSource != null)
        {
            // Make sure to reset pitch
            musicSource.pitch = 1;
        }

        //If we're transitioning to the menu scene, play the menu music (if it is not already playing). Otherwise pull and autoplay the current level's music.
        if (newScene.name == "LobbyScene")
        {
            if (musicSource.clip != menuMusic)
            {
                PlayMusic(menuMusic, true);
            }
        }
        else
        {
          //  PlayMusic(GameSettings.s_Instance.map.levelMusic, true);
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayCurrentMusic()
    {
        musicSource.Play();
    }

    private void PlayMusic(AudioClip music, bool fadeIn = false, bool loop = true)
    {
        musicSource.Stop();

        musicSource.loop = loop;
        musicSource.clip = music;
        musicSource.PlayDelayed(startDelay);

        if (fadeIn)
        {
            fadeLevel =- startDelay;
        }
    }
    //Unsubscribe from the SceneManager on destruction.
    protected override void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
        base.OnDestroy();
    }
}
