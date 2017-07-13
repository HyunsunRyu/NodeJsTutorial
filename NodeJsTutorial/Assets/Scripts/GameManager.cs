using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject userPfb;

    private SocketIOComponent socket;
    private MiniPool<UserController> userObjectPool;

    private Dictionary<string, UserController> userDic;
    private UserController player;

    private void Awake()
    {
        Instance = this;

        GameObject hideRoot = new GameObject("HideRoot");
        hideRoot.transform.parent = transform;

        userObjectPool = MiniPool<UserController>.MakeMiniPool(hideRoot.transform, userPfb);

        socket = SocketIOComponent.instance;

        socket.On("error", ErrorCallback);

        socket.On("SUCCESS_ACCESS", OnSuccessAccess);
        socket.On("SUCCESS_CONNECT", OnSuccessConnect);
        socket.On("USER_CONNECTED", OnUserConnected);
        socket.On("USER_DISCONNECTED", OnUserDisConnected);
        socket.On("MOVE", OnUserMove);

        userDic = new Dictionary<string, UserController>();

        StartCoroutine(TryConnect());
    }

    private IEnumerator TryConnect()
    {
        socket.Connect();
        yield return new WaitForSeconds(0.5f);
        Connect();
    }
    
    private void ErrorCallback(SocketIOEvent e)
    {
        Debug.LogError("error : " + e.name + " / " +  e.data);
    }
    
    private void Connect()
    {
        SpawnData spawnPoint = new SpawnData(GetRandomPosition());

        string data = JsonUtility.ToJson(spawnPoint);
        socket.Emit("USER_ACCESS", new JSONObject(data));
    }

    private float[] GetRandomPosition()
    {
        float[] pos = new float[3];
        pos[0] = Random.Range(245f, 255f);
        pos[1] = 0f;
        pos[2] = Random.Range(245f, 255f);
        return pos;
    }

    private void OnSuccessAccess(SocketIOEvent e)
    {
        Debug.Log("Success Access : " + e.data.ToString());

        UserData playerData = JsonUtility.FromJson<UserData>(e.data.ToString());
        playerData.name = "TEST";
        player = CreateUser(playerData, true);
        userDic.Add(playerData.id, player);

        string data = JsonUtility.ToJson(playerData);
        socket.Emit("USER_CONNECT", new JSONObject(data));
    }
    
    private void OnSuccessConnect(SocketIOEvent e)
    {
        Debug.Log("Success Connect : " + e.data.ToString());

        UserDataList userList = JsonUtility.FromJson<UserDataList>(e.data.ToString());
        foreach (UserData user in userList.users)
        {
            userDic.Add(user.id, CreateUser(user, false));
        }
    }

    private void OnUserConnected(SocketIOEvent e)
    {
        Debug.Log(e.data.ToString());

        UserData userData = JsonUtility.FromJson<UserData>(e.data.ToString());
        userDic.Add(userData.id, CreateUser(userData, false));
    }

    private void OnUserDisConnected(SocketIOEvent e)
    {
        UserData userData = JsonUtility.FromJson<UserData>(e.data.ToString());

        DestroyUser(userData);
    }

    public void MovedPlayer(Vector3 pos)
    {
    }

    private void OnUserMove(SocketIOEvent e)
    {
    }

    private UserController CreateUser(UserData data, bool bIsUser = false)
    {
        UserController user = userObjectPool.GetItem();
        user.Init(data, bIsUser);
        return user;
    }

    private void DestroyUser(UserData data)
    {
        if (!userDic.ContainsKey(data.id))
            Debug.LogError("something is wrong : the user whom has this id is none");
        else
        {
            userObjectPool.ReturnItem(userDic[data.id]);
        }
    }
}
