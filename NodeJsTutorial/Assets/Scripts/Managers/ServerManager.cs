using SocketIO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerManager : Singleton<ServerManager>
{
    [SerializeField] private SocketIOComponent socket;

    private const float tryConnectTime = 10f;
    
    private float startConnectErrorTime;
    private bool bConnectError;

    protected override void Init()
    {
        UIManager.Instance.OpenUI<ConnectServerUI>();
    }

    public void Connect()
    {
        socket.On("error", ErrorCallback);
        socket.On("SUCCESS_CONNECT", OnSuccessConnect);
        socket.On("USER_CONNECTED", OnUserConnected);
        socket.On("USER_DISCONNECTED", OnUserDisConnected);
        socket.On("MOVE", OnUserMove);

        socket.Connect();

        StartCoroutine(WaitConnectProcess(3f, (bool success) =>
        {
            Debug.Log("Connect : " + success.ToString());
            
            bConnectError = false;
            
            StartCoroutine(CheckRetryConnectProcess());
        }));
    }
    
    private void Disconnect()
    {
        socket.Off("error", ErrorCallback);
        socket.Off("SUCCESS_CONNECT", OnSuccessConnect);
        socket.Off("USER_CONNECTED", OnUserConnected);
        socket.Off("USER_DISCONNECTED", OnUserDisConnected);
        socket.Off("MOVE", OnUserMove);

        socket.Close();

        UIManager.Instance.CloseUI<ConnectingUI>();
    }

    private IEnumerator WaitConnectProcess(float delayTime, System.Action<bool> connect)
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

    private IEnumerator CheckRetryConnectProcess()
    {
        bool bNeedToCheck = false;
        while (true)
        {
            if (bNeedToCheck != bConnectError)
            {
                if (!bNeedToCheck)
                    startConnectErrorTime = Time.realtimeSinceStartup;

                bNeedToCheck = bConnectError;
            }

            if (bNeedToCheck && Time.realtimeSinceStartup >= startConnectErrorTime + tryConnectTime)
            {
                bConnectError = false;
                bNeedToCheck = false;
                Disconnect();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void ErrorCallback(SocketIOEvent e)
    {
        Debug.Log(e.ToString());
        
        bConnectError = true;
    }
    
    private void OnSuccessConnect(SocketIOEvent e)
    {
        Debug.Log(e.ToString());

        bConnectError = false;
        
        UIManager.Instance.CloseUI<ConnectServerUI>();

        UIManager.Instance.OpenUI<MainUI>();
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
