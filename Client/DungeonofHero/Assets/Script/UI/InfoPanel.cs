using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{

    [SerializeField]
    protected Text infoText;
    [SerializeField]
    protected Button cancelButton;

    public void Display(string info)
    {
        infoText.text = info;
        cancelButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
            });

        gameObject.SetActive(true);
    }
}