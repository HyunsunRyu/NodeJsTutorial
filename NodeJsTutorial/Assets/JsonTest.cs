using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
class Test
{
    public int testInt;
}

class Data
{
    public string name;
    public List<string> likes;
    public int level;
    public Test test;
}
public class JsonTest : MonoBehaviour
{
    void Start()
    {
        Data data = new Data();
        data.name = "Park";
        data.level = 10;
        data.likes = new List<string>() { "dog", "cat" };
        data.test = new Test();
        data.test.testInt = 15;
        Debug.Log(JsonUtility.ToJson(data));
    }
}
