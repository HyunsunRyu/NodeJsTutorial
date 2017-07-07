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
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["name"] = System.Guid.NewGuid().ToString();
        Vector3 position = GetRandomPosition();
        data["position"] = position.x.ToString() + "," + position.y.ToString() + "," + position.z.ToString();
        
        socket.Emit("USER_CONNECT", new JSONObject(data));
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(245f, 255f), 0f, Random.Range(245f, 255f)); ;
    }

    private void OnSuccessConnect(SocketIOEvent e)
    {
        JSONObject json = new JSONObject(e.data);

        Debug.Log(json);

        //PlayerController player = playerPool.GetItem();
        //player.InitMainCharacter(e.data["name"].ToString());
        //string[] pos = e.data["position"].ToString().Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        //Vector3 position = new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]);
        //player.transform.position = position;
    }

    private void OnUserConnected(SocketIOEvent e)
    {
        PlayerController player = playerPool.GetItem();
        player.InitMainCharacter(e.data["name"].ToString());
        Debug.LogError(e.data);
        player.transform.position = new Vector3(Random.Range(245f, 255f), 0f, Random.Range(245f, 255f));
    }

    private void OnUserDisConnected(SocketIOEvent e)
    {
    }

    private void OnUserMove(SocketIOEvent e)
    {
    }
}
