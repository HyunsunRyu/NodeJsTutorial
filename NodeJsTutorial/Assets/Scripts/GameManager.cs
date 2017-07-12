using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPfb;

    private SocketIOComponent socket;
    private MiniPool<PlayerController> playerPool;

    private Dictionary<string, PlayerController> playerList;

    private void Start()
    {
        GameObject hideRoot = new GameObject("HideRoot");
        hideRoot.transform.parent = transform;

        playerPool = MiniPool<PlayerController>.MakeMiniPool(hideRoot.transform, playerPfb);

        socket = SocketIOComponent.instance;
        
        socket.On("error", ErrorCallback);

        socket.On("SUCCESS_CONNECT", OnSuccessConnect);
        socket.On("USER_CONNECTED", OnUserConnected);
        socket.On("USER_DISCONNECTED", OnUserDisConnected);
        socket.On("MOVE", OnUserMove);

        playerList = new Dictionary<string, PlayerController>();

        socket.Connect();
    }

    private void ErrorCallback(SocketIOEvent e)
    {
        Debug.LogError("error : " + e.name + " / " +  e.data);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Connect();
    }

    public void Connect()
    {
        UserData newUser = new UserData();
        newUser.id = "null";
        newUser.name = "Test";
        newUser.position = GetRandomPosition();

        string data = JsonUtility.ToJson(newUser);
        socket.Emit("USER_CONNECT", new JSONObject(data));
        Debug.Log("emit - user_connect : " + data);
    }

    private float[] GetRandomPosition()
    {
        float[] pos = new float[3];
        pos[0] = Random.Range(245f, 255f);
        pos[1] = 0f;
        pos[2] = Random.Range(245f, 255f);
        return pos;
    }
    
    private void OnSuccessConnect(SocketIOEvent e)
    {
        Debug.Log(e.data.ToString());

        UserDataList userList = JsonUtility.FromJson<UserDataList>(e.data.ToString());
        foreach (UserData user in userList.users)
        {
            Debug.Log(user.name + " // " + user.GetPostion());
        }

        //UserData player = JsonUtility.FromJson<UserData>(e.data.ToString());
        //Debug.Log(player.name + " :: " + player.GetPostion());
    }

    private void OnUserConnected(SocketIOEvent e)
    {
        Debug.Log(e.data.ToString());

        UserData player = JsonUtility.FromJson<UserData>(e.data.ToString());
        Debug.Log(player.name + " :: " + player.GetPostion());
    }

    private void OnUserDisConnected(SocketIOEvent e)
    {
    }

    private void OnUserMove(SocketIOEvent e)
    {
    }
}
