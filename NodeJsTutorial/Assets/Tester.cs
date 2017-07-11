using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Tester : MonoBehaviour
{
    private SocketIOComponent socket;

    private void Start()
    {
        socket = SocketIOComponent.instance;

        socket.On("error", ErrorCallback);

        socket.On("SUCCESS_CONNECT", OnSuccessConnect);
        socket.On("USER_CONNECTED", OnUserConnected);
        socket.On("USER_DISCONNECTED", OnUserDisConnected);
        socket.On("List", OnTestList);
        
        socket.Connect();

        StartCoroutine(Connect());
    }

    private IEnumerator Connect()
    {
        yield return new WaitForSeconds(0.5f);

        TestData testData = new TestData();
        testData.name = "test";
        string data = JsonUtility.ToJson(testData);
        socket.Emit("USER_CONNECT", new JSONObject(data));

        string str = JsonUtility.ToJson(testData);
        Debug.Log("ToJson : " + str);
        TestData dd = JsonUtility.FromJson<TestData>(str);
        Debug.Log(dd.name);

        TestDataList dList = new TestDataList();
        dList.users = new TestData[1];
        dList.users[0] = dd;
        str = JsonUtility.ToJson(dList);
        Debug.Log("ToJson : " + str);
        TestDataList ddd = JsonUtility.FromJson<TestDataList>(str);
        Debug.Log(ddd == null);
        Debug.Log(ddd.users == null);
        Debug.Log("========================");
    }

    private void ErrorCallback(SocketIOEvent e)
    {
        Debug.LogError("error : " + e.name + " / " + e.data);
    }

    private void OnSuccessConnect(SocketIOEvent e)
    {
        Debug.Log(e.name + " : " + e.data.ToString());

        TestData data = JsonUtility.FromJson<TestData>(e.data.ToString());

        Debug.Log(data == null);
        Debug.Log(data.name);
    }

    private void OnTestList(SocketIOEvent e)
    {
        Debug.Log(e.name + " : " + e.data.ToString());

        TestDataList list = JsonUtility.FromJson<TestDataList>(e.data.ToString());

        TestData[] array = JsonHelper.FromJson<TestData>(e.data.ToString());

        Debug.Log(list == null);
        Debug.Log(list.users == null);
        if (list.users != null)
            Debug.Log(list.users.Length);
        
        Debug.Log(array == null);
        if (array != null)
            Debug.Log(array.Length);

    }

    private void OnUserConnected(SocketIOEvent e)
    {
        Debug.LogError(e.data);
    }

    private void OnUserDisConnected(SocketIOEvent e)
    {
    }   
}

public class TestData
{
    public string name;
}

public class TestDataList
{
    public TestData[] users;
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    private class Wrapper<T>
    {
        public T[] Items;
    }
}
