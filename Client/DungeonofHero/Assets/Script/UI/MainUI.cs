using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using SJ.GameServer.Service;
using SJ.GameServer.Service.GameService.Message;
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
    protected CanvasGroup splashPanel;

    [SerializeField]
    protected CanvasGroup loginPanel;

    [SerializeField]
    protected CanvasGroup createAccountPanel;

    [SerializeField]
    protected CanvasGroup lobbyPanel;

    [SerializeField]
    protected CanvasGroup createCharacterPanel;

    [SerializeField]
    protected InfoPanel infoPanel;

    /**
     * 
     */
    [SerializeField]
    protected CanvasGroup lobbyHeroDefaultSettingPanel;

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
        ShowPanel(splashPanel);
    }

    public void ShowLoginPanel()
    {
        ShowPanel(loginPanel);
    }

    public void ShowCreateAccount()
    {
        ShowPanel(createAccountPanel);
    }

    public void ShowLobbyHeroCreatePanel()
    {
        ShowPanel(createCharacterPanel);
    }

    public void ShowLobbyPanel()
    {
        ShowPanel(lobbyPanel);
    }

    public void ShowInfoPopup(string label)
    {
        if (infoPanel != null)
            infoPanel.Display(label);     
    }
}