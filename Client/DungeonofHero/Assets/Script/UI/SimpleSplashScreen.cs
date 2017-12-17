using UnityEngine;
using UnityEngine.SceneManagement;
/**
 * 
 */
public class SimpleSplashScreen : MonoBehaviour
{
    [SerializeField]
    protected float fadeTime = 2f;

    /**
     * 
     */
    [SerializeField]
    protected FadingGroup fadingGroup;

    /**
     * 
     */
    [SerializeField]
    protected PulsingUIGroup pulsingGroup;

    protected void Start()
    {
        fadingGroup.StartFade(Fade.In, fadeTime, StartPulsingText);
    }

    /**
     * @return
     */
    protected void Update()
    {
        if (Input.anyKeyDown)
        {
            ProgressToNextLogin();
        }
    }

    /**
     * @return
     */
    protected void StartPulsingText()
    {
        pulsingGroup.StartPulse(Fade.In);
    }

    /**
     * @return
     */
    private void ProgressToNextLogin()
    {
        MainUI.s_Instance.ShowLoginPanel();
    }
}