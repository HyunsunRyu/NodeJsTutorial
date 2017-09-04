using SocketIO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainManager : Singleton<MainManager>
{
    [SerializeField] private SocketIOComponent socket;

    private const float tryConnectTime = 10f;
    
    private float startConnectErrorTime;
    private bool bConnectError;

    protected override void Init()
    {
        UIManager.Instance.OpenUI<ConnectServerUI>();
        UIManager.Instance.CloseUI<MainUI>();
    }

    public void Connect()
    {
        socket.On("error", ErrorCallback);
        socket.On("SUCCESS_CONNECT", OnSuccessConnect);
        socket.On("INPUTTIME", OnSuccessSendData);

        socket.Connect();

        StartCoroutine(WaitConnectProcess(3f, (bool success) =>
        {
            bConnectError = false;
            StartCoroutine(CheckRetryConnectProcess());
        }));
    }
    
    private void Disconnect()
    {
        socket.Off("error", ErrorCallback);
        socket.Off("SUCCESS_CONNECT", OnSuccessConnect);
        socket.Off("INPUTTIME", OnSuccessSendData);

        socket.Close();

        if (UIManager.Instance.IsOpened<ConnectingUI>())
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
                if (!UIManager.Instance.IsOpened<ConnectingUI>())
                    UIManager.Instance.OpenUI<ConnectingUI>();
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
        bConnectError = true;
    }
    
    private void OnSuccessConnect(SocketIOEvent e)
    {
        bConnectError = false;

        if (UIManager.Instance.IsOpened<ConnectServerUI>())
            UIManager.Instance.CloseUI<ConnectServerUI>();
        if (!UIManager.Instance.IsOpened<MainUI>())
            UIManager.Instance.OpenUI<MainUI>();

        UIManager.Instance.GetUI<MainUI>().UpdateUI();
    }
    
    public void SendData()
    {
        socket.Emit("INPUTTIME");
    }

    private void OnSuccessSendData(SocketIOEvent e)
    {
        InputTime inputTime = JsonUtility.FromJson<InputTime>(e.data.ToString());
    }
}
