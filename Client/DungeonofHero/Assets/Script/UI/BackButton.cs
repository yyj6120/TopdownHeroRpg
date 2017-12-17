using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class BackButton : MonoBehaviour
{
    private static List<BackButton> buttonStack = new List<BackButton>();

    protected Button _backButton;

    protected virtual void OnEnable()
    {
        _backButton = GetComponent<Button>();

        buttonStack.Add(this);
    }

    protected virtual void OnDisable()
    {
        buttonStack.Remove(this);
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (buttonStack.Count > 0 && buttonStack[buttonStack.Count - 1] == this && _backButton.interactable && _backButton.enabled)
            {
                OnBackPressed();
            }
        }
    }

    protected virtual void OnBackPressed()
    {
        _backButton.onClick.Invoke();
    }
}