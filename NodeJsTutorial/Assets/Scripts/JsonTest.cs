using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JsonTest : MonoBehaviour
{
    [System.Serializable]
    struct Test
    {
        public int testInt;

        public void SetTestInt(int value) { testInt = value; }
        public int GetTestInt() { return testInt; }
    }

    struct Data
    {
        public List<Test> test;

        public void AddData(Test t)
        {
            if (test == null)
                test = new List<Test>();
            test.Add(t);
        }
        
        public void Log()
        {
            if (test == null)
                Debug.Log("test is null");
            else
            {
                string log = "";
                foreach (Test t in test)
                {
                    log += t.GetTestInt().ToString() + ", ";
                }
                Debug.Log("test list is " + log);
            }
        }
    }

    private void Start()
    {
        Data data = new Data();
        Test t1 = new Test();
        t1.SetTestInt(1);
        Test t2 = new Test();
        t2.SetTestInt(2);
        data.AddData(t1);
        data.AddData(t2);
        string str = JsonUtility.ToJson(data);

        Debug.Log(str);

        Data jData = JsonUtility.FromJson<Data>(str);
        jData.Log();
    }
}
