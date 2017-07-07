using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public string data;

    void Start()
    {
        string t = data.Replace(@"\\\", "");
        JSONObject json = new JSONObject(t);

        JSONObject test = json.GetField("userList");
        Debug.Log(test);
    }
}
