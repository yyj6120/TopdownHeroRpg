using UnityEngine;
/// <summary>
/// Pulsing user interface group.
/// </summary>
[RequireComponent(typeof(FadingGroup))]
public class PulsingUIGroup : MonoBehaviour
{
    //Fade values
    [SerializeField]
    protected float fadeInTime = 1f, fadeOutTime = 1f, fadeOutValue = 0.5f;

    //Awake fading behaviour
    [SerializeField]
    protected Fade fadeOnAwake = Fade.None;

    //The fading group
    protected FadingGroup fadingGroup;

    protected virtual void Awake()
    {
        if (fadeOnAwake != Fade.None)
        {
            StartPulse(fadeOnAwake);
        }
    }

    /// <summary>
    /// Starts the pulse.
    /// </summary>
    /// <param name="fade">Fade.</param>
    public void StartPulse(Fade fade)
    {
        gameObject.SetActive(true);
        if (fade == Fade.In)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
    }

    /// <summary>
    /// Stops the pulse.
    /// </summary>
    public void StopPulse()
    {
        fadingGroup.StopFade(true);
    }

    protected void FadeIn()
    {
        LazyLoad();
        fadingGroup.StartFade(Fade.In, fadeInTime, FadeOut, false);
    }

    protected void FadeOut()
    {
        LazyLoad();
        fadingGroup.FadeOutToValue(fadeOutTime, fadeOutValue, FadeIn);
    }

    /// <summary>
    /// Lazy loads the fading group
    /// </summary>
    protected void LazyLoad()
    {
        if (fadingGroup != null)
        {
            return;
        }

        fadingGroup = GetComponent<FadingGroup>();
    }
}