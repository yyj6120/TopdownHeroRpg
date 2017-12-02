using UnityEngine;


//Base class for all modals
public class Modal : MonoBehaviour
{
    [SerializeField]
    protected CanvasGroup canvasGroup;

    public virtual void CloseModal()
    {
        gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        EnableInteractivity();
    }

    protected virtual void EnableInteractivity()
    {
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
        }
    }

    protected virtual void DisableInteractivity()
    {
        if (canvasGroup != null)
        {
            canvasGroup.interactable = false;
        }
    }
}