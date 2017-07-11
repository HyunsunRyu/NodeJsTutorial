using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public string name;
    public string position;
}

public class UserDataList
{
    public List<UserData> userDataList;

    public static UserDataList CreateData(string data)
    {
        UserDataList list = JsonUtility.FromJson<UserDataList>(data);
        return list;
    }
}