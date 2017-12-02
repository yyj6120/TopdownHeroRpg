using UnityEngine;
using UnityEngine.SceneManagement;
/**
 * 
 */
public class SimpleSplashScreen : MonoBehaviour
{
    /**
     * 
     */
    [SerializeField]
    protected string sceneName = "CreateScene";

    /**
     * 
     */
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

    /**
     * @return
     */
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
            ProgressToNextScene();
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
    private void ProgressToNextScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}