using SocketIO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainManager : MonoBehaviour
{
    [SerializeField] private SocketIOComponent socket;
    [SerializeField] private UIManager uiManager;

    private void Awake()
    {
        socket.Connect();

        StartCoroutine(CheckConnected(3f, (bool success) =>
        {
            Debug.Log("Connect : " + success.ToString());

            if (success)
                SetSocket();
            else
                Disconnect();
        }));
    }

    private void SetSocket()
    {
        socket.On("error", ErrorCallback);
        socket.On("SUCCESS_CONNECT", OnSuccessConnect);
        socket.On("USER_CONNECTED", OnUserConnected);
        socket.On("USER_DISCONNECTED", OnUserDisConnected);
        socket.On("MOVE", OnUserMove);

        uiManager.ShowMainPanel();
    }

    private void Disconnect()
    {
        if (uiManager != null)
        {
            uiManager.ShowConnectPanel();
            uiManager.ConnectFail();
        }
    }

    private IEnumerator CheckConnected(float delayTime, System.Action<bool> connect)
    {
        float lastTime = Time.realtimeSinceStartup;
        while (true)
        {
            if (socket.IsConnected)
            {
                connect(true);
                yield break;
            }

            if (Time.realtimeSinceStartup >= lastTime)
            {
                connect(false);
                yield break;
            }

            yield return null;
        }
    }

    private void ErrorCallback(SocketIOEvent e)
    {
        Debug.Log(e.ToString());
        if (socket.IsConnected)
        {
            socket.Close();
            Disconnect();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Connect();
    }

    public void Connect()
    {
    }

    private void OnSuccessConnect(SocketIOEvent e)
    {
    }

    private void OnUserConnected(SocketIOEvent e)
    {
    }

    private void OnUserDisConnected(SocketIOEvent e)
    {
    }

    private void OnUserMove(SocketIOEvent e)
    {
    }
}
