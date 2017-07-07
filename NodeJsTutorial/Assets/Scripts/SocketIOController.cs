using UnityEngine;
using System.Collections.Generic;
using SocketIO;

public class SocketIOController : MonoBehaviour
{
    [SerializeField] private SocketIOComponent socket;

    private void Awake()
    {
        socket.On("open", OpenCallback);
        socket.On("error", ErrorCallback);
        socket.On("close", CloseCallback);
        socket.On("SUCCESS_CONNECT", OnSuccessConnect);
        socket.On("USER_CONNECTED", OnUserConnected);
        socket.On("MOVE", OnUserMove);
    }

    public void Connect()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["name"] = "Tester";
        Vector3 position = new Vector3(0f, 0f, 0f);
        data["postion"] = position.x + "," + position.y + "," + position.z;
        socket.Emit("USER_CONNECT", new JSONObject(data));
    }

    private void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Connect();
        }
    }

    private void OpenCallback(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
    }

    private void TestBoop(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Boop received: " + e.name + " " + e.data);

        if (e.data == null) { return; }

        Debug.Log(
            "#####################################################" +
            "THIS: " + e.data.GetField("this").str +
            "#####################################################"
        );
    }

    private void ErrorCallback(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
    }

    private void CloseCallback(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
    }

    private void OnSuccessConnect(SocketIOEvent e)
    {
        Debug.Log("Success To Connect");
    }

    private void OnUserConnected(SocketIOEvent e)
    {
        Debug.Log("User : " + e.data + " is Connected");
    }

    private void OnUserMove(SocketIOEvent e)
    {
        Debug.Log("User : " + e.data + " is moved");
    }
}
