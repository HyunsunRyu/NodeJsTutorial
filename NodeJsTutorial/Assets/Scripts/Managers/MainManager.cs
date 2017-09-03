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

        //StartCoroutine(Test());
    }

    private IEnumerator Test()
    {
        socket.Connect();

        socket.On("error", (SocketIOEvent e) => { Debug.Log(e.ToString()); });
        socket.On("test1", (SocketIOEvent e) => { Debug.Log(e.ToString()); });
        socket.On("test2", (SocketIOEvent e) => { Debug.Log(e.ToString()); });
        socket.On("test3", (SocketIOEvent e) => { Debug.Log(e.ToString()); });
        yield return new WaitForSeconds(1f);

        socket.Emit("test1");
        yield return new WaitForSeconds(0.5f);
        socket.Emit("test2");
        yield return new WaitForSeconds(0.5f);
        socket.Emit("test3", new JSONObject("name"), (JSONObject jo)=> { Debug.Log(jo.ToString()); });
        yield return new WaitForSeconds(0.5f);
        socket.Close();
    }

    public void Connect()
    {
        socket.On("error", ErrorCallback);
        socket.On("SUCCESS_CONNECT", OnSuccessConnect);
        socket.On("RECEIVED_DATA", OnReceivedData);
        socket.On("R_INPUTDATE", OnSuccessSendData);

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
        socket.Off("RECEIVED_DATA", OnReceivedData);
        socket.Off("R_INPUTDATE", OnSuccessSendData);

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
        //Debug.Log(e.ToString());
        
        //bConnectError = true;
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

    private void OnReceivedData(SocketIOEvent e)
    {
    }

    public void SendData()
    {
        socket.Emit("INPUTDATE");
    }

    private void OnSuccessSendData(SocketIOEvent e)
    {
        Debug.Log(e.ToString());
    }
}
