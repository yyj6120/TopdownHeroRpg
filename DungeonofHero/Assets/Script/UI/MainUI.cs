using UnityEngine;
/**
 * 
 */
public enum MenuPage
{
    Home,
    LobbyHeroCreatePanel
}

public class MainUI : Singleton<MainUI>
{
    #region Static config
    public static MenuPage returnPage;
    #endregion
    [SerializeField]
    protected CanvasGroup defaultPanel;
    /**
     * 
     */
    [SerializeField]
    protected CanvasGroup LobbyHeroCreatePanel;

    /**
     * 
     */
    [SerializeField]
    protected CanvasGroup LobbyHeroDefaultSettingPanel;

    private CanvasGroup currentPanel;

    public void Start()
    {
        switch (returnPage)
        {
            case MenuPage.Home:
            default:
                ShowDefaultPanel();
                break;
            case MenuPage.LobbyHeroCreatePanel:
                ShowLobbyHeroCreatePanel();
                break;
        }
    }

    public void ShowPanel(CanvasGroup newPanel)
    {
        if (currentPanel != null)
        {
            currentPanel.gameObject.SetActive(false);
        }

        currentPanel = newPanel;
        if (currentPanel != null)
        {
            currentPanel.gameObject.SetActive(true);
        }
    }

    public void ShowDefaultPanel()
    {
        ShowPanel(defaultPanel);
    }

    public void ShowLobbyHeroCreatePanel()
    {
        ShowPanel(LobbyHeroCreatePanel);
    }
}