using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UserData
{
    public string id;
    public string name;
    public float[] position;

    public void SetPosition(Vector3 pos)
    {
        if (position == null)
            position = new float[3];
        position[0] = pos.x;
        position[1] = pos.y;
        position[2] = pos.z;
    }

    public Vector3 GetPostion()
    {
        return new Vector3(position[0], position[1], position[2]);
    }
}

public class UserDataList
{
    public List<UserData> users;

    public static UserDataList CreateData(string data)
    {
        UserDataList list = JsonUtility.FromJson<UserDataList>(data);
        return list;
    }
}