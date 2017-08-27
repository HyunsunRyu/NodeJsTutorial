using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject connectPanel, mainPanel;
    [SerializeField] private Text connectingText;

    public void ShowConnectPanel()
    {
        connectPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void ShowMainPanel()
    {
        connectPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void ConnectFail()
    {
        connectingText.text = "Fail Connect";
    }

    public void ConnectSuccess()
    {
        ShowMainPanel();
    }
}
