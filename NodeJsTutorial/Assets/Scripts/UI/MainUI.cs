using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : UIChild
{
    public void OnClickInput()
    {
        MainManager.Instance.SendData();
    }

    public void UpdateData()
    {
    }
}
