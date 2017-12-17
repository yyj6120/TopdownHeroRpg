using UnityEngine;
using System;
/**
 * 페이드 Enum 정의
 */
public enum Fade
{
    None,
    In,
    Out
}
[RequireComponent(typeof(CanvasGroup))]
public class FadingGroup : MonoBehaviour
{
    /**
     * 
     */
    /**
     * 
     */
    private CanvasGroup canvasGroup;

    /**
     * 
     */
    private float fadeTime = 0f;

    /**
     * 
     */
    private float fadeOutValue = 0f;

    /**
     * 
     */
    private Action finishFade;

    /**
     * 
     */
    private Fade fade = Fade.None;

    /**
     * 
     */
    public float FadeStep
    {
        get
        {
            if (fadeTime == 0f)
            {
                return 1f;
            }

            return Time.unscaledDeltaTime / fadeTime;
        }
    }

    public Fade CurrentFade
    {
        get
        {
            return fade;
        }
    }

    /**
     * @return
     */
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /**
     * @return
     */
    private void Update()
    {
        if (fade == Fade.Out)
        {
            FadeOut();
        }
        else if (fade == Fade.In)
        {
            FadeIn();
        }
    }

    /**
     * @return
     */
    private void FadeOut()
    {
        canvasGroup.alpha -= FadeStep;
        if (canvasGroup.alpha <= fadeOutValue + Mathf.Epsilon)
        {
            canvasGroup.alpha = fadeOutValue;
            if (fadeOutValue == 0)
            {
                gameObject.SetActive(false);
            }

            EndFade();
        }
    }

    /**
     * @return
     */
    private void FadeIn()
    {
        canvasGroup.alpha += FadeStep;
        if (canvasGroup.alpha >= 1 - Mathf.Epsilon)
        {
            canvasGroup.alpha = 1;
            EndFade();
        }
    }

    /**
     * @return
     */
    private void EndFade()
    {
        fade = Fade.None;
        FireEvent();
    }

    /**
     * @return
     */
    private void FireEvent()
    {
        if (finishFade != null)
        {
            finishFade.Invoke();
        }
    }

    /**
     * @param fade 
     * @param fadeTime 
     * @param finishFade 
     * @param reactivate 
     * @return
     */
    public void StartFade(Fade fade, float fadeTime, Action finishFade = null, bool reactivate = true)
    {
        this.fade = fade;
        this.fadeTime = fadeTime;
        this.finishFade = finishFade;
        this.fadeOutValue = 0f;
        if (reactivate)
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = (fade == Fade.In ? 0f : 1f);
        }
    }

    /**
     * @param fadeTime 
     * @param fadeOutValue 
     * @param finishFade 
     * @return
     */
    public void FadeOutToValue(float fadeTime, float fadeOutValue, Action finishFade)
    {
        this.fade = Fade.Out;
        this.fadeTime = fadeTime;
        this.fadeOutValue = fadeOutValue;
        this.finishFade = finishFade;
    }

    /**
     * @param fade 
     * @param fadeTime 
     * @param finishFade 
     * @return
     */
    public void StartFadeOrFireEvent(Fade fade, float fadeTime, Action finishFade)
    {
        StartFade(fade, fadeTime, finishFade, false);
        if (!gameObject.activeInHierarchy)
        {
            FireEvent();
        }
    }

    /**
     * @param setVisible 
     * @return
     */
    public void StopFade(bool setVisible)
    {
        fade = Fade.None;
        gameObject.SetActive(setVisible);
        if (setVisible)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            canvasGroup.alpha = 0f;
        }
    }

}