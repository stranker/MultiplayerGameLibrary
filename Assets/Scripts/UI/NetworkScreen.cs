using UnityEngine;
using UnityEngine.UI;
using System.Net;

public class NetworkScreen : MonoBehaviourSingleton<NetworkScreen>
{
    public Button connectBtn;
    public Button startServerBtn;
    public InputField portInputField;
    public InputField addressInputField;
    public bool isServer = false;
    private int port;
    private IPAddress ipAddress;
    public InputField usernameField;
    public string username;
    public GameObject usernamePanel;
    public Button confirmUserButton;

    protected override void Initialize()
    {
        connectBtn.onClick.AddListener(OnConnectBtnClick);
        startServerBtn.onClick.AddListener(OnStartServerBtnClick);
    }

    void OnConnectBtnClick()
    {
        ipAddress = IPAddress.Parse(addressInputField.text);
        port = System.Convert.ToInt32(portInputField.text);
        usernamePanel.SetActive(true);
    }

    void OnStartServerBtnClick()
    {
        port = System.Convert.ToInt32(portInputField.text);
        isServer = true;
        SwitchToNextScreen();
    }

    public void OnConfirmUsernameClick()
    {
        SwitchToNextScreen();
    }

    public void OnValueChangedUsername(string text)
    {
        confirmUserButton.gameObject.SetActive(usernameField.text != "");
    }

    public void OnEndEditUsername(string text)
    {
        username = usernameField.text;
    }

    void SwitchToNextScreen()
    {
        if (isServer)
        {
            ChatScreen.Instance.SetUsername("Server");
            NetworkManager.Instance.StartServer(port);
        }
        else
        {
            NetworkManager.Instance.StartClient(ipAddress, port, username);
        }
        ChatScreen.Instance.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
