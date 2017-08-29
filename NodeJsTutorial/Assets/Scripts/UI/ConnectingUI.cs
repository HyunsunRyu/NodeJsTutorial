using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConnectingUI : UIChild
{
    [SerializeField] private Text text;

    private const string strConnecting = "Connecting";
    private int count;
    private IEnumerator connecting = null;

    public override void UpdateUI()
    {
        count = 0;
        connecting = Connecting();
        StartCoroutine(connecting);
    }

    private IEnumerator Connecting()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            count = (count + 1) % 4;
            text.text = strConnecting + GetTail(count);
        }
    }

    private string GetTail(int count)
    {
        switch (count)
        {
            default:
            case 0:
                return "";
            case 1:
                return ".";
            case 2:
                return "..";
            case 3:
                return "...";
        }
    }
}
