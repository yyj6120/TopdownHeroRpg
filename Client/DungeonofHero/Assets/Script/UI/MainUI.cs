using UnityEngine;
/**
 * 
 */
public enum MenuPage
{
    Splash,
    Login,
    CreateAccount,
    Lobby,
    LobbyHeroCreatePanel,
    InGame
}

public class MainUI : PersistentSingleton<MainUI>
{
    #region Static config
    public static MenuPage returnPage;
    #endregion
    /**
     * 
     */
    [SerializeField]
    protected CanvasGroup SplashPanel;

    [SerializeField]
    protected CanvasGroup LoginPanel;

    [SerializeField]
    protected CanvasGroup CreateAccountPanel;

    [SerializeField]
    protected CanvasGroup LobbyPanel;

    [SerializeField]
    protected CanvasGroup CreateCharacterPanel;

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
            case MenuPage.Splash:
            default:
                ShowSplashPanel();
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

    public void ShowSplashPanel()
    {
        ShowPanel(SplashPanel);
    }

    public void ShowLoginPanel()
    {
        ShowPanel(LoginPanel);
    }

    public void ShowCreateAccount()
    {
        ShowPanel(CreateAccountPanel);
    }
    public void ShowLobbyHeroCreatePanel()
    {
        ShowPanel(CreateCharacterPanel);
    }

    public void ShowLobbyPanel()
    {
        ShowPanel(LobbyPanel);
    }
}