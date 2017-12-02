using UnityEngine;

/// <summary>
/// Loading modal - used to handle loading fades
/// </summary>
[RequireComponent(typeof(FadingGroup))]
public class LoadingModal : Modal
{
    private FadingGroup fader;

    [SerializeField]
    protected float fadeTime = 0.5f;

    public static LoadingModal instance
    {
        get;
        private set;
    }

    public bool readyToTransition
    {
        get
        {
            return fader.CurrentFade == Fade.None && gameObject.activeSelf;
        }
    }

    /// <summary>
    /// Getter for Fader - used in game manager
    /// </summary>
    /// <value>The fader.</value>
    public FadingGroup Fader
    {
        get
        {
            return fader;
        }
    }

    /// <summary>
    /// Wraps fade in on FadingGroup
    /// </summary>
    public void FadeIn()
    {
        Show();
        Fader.StartFade(Fade.In, fadeTime);
    }

    /// <summary>
    /// Wraps fade out on FadingGroup
    /// </summary>
    public void FadeOut()
    {
        Show();
        Fader.StartFade(Fade.Out, fadeTime, CloseModal);
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.Log("<color=lightblue>Trying to create a second instance of LoadingModal</color");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        fader = GetComponent<FadingGroup>();
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}