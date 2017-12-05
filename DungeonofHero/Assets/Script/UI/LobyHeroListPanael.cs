using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
/**
 * 
 */
public class LobyHeroListPanael : MonoBehaviour
{
    public Button hostButton;
    public NetworkManagerDH netmanager;
    private void Update()
    {
        hostButton.onClick.SetListener(() =>
        {
            netmanager.StartHost();
        }
        );
    }
}