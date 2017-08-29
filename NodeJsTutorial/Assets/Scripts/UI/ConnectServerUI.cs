using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectServerUI : UIChild
{
    public void OnClickConnectButton()
    {
        ServerManager.Instance.Connect();
    }
}
