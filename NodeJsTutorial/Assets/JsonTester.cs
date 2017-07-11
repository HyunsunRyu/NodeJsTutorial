using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTester : MonoBehaviour
{
    private void Awake()
    {
        //Test1();
        Test2();
    }

    private void Test1()
    {
        Test1.Test testA = new Test1.Test();
        testA.name = "TestA";
        string str = JsonUtility.ToJson(testA);
        Debug.Log(str);
        Test1.Test testAA = JsonUtility.FromJson<Test1.Test>(str);
        Debug.Log(testAA.name);
    }

    private void Test2()
    {
        Test2.TestList testList = new Test2.TestList();
        Debug.Log(JsonUtility.ToJson(testList));
    }
}

namespace Test1
{
    public class Test
    {
        public string name;
    }

    public class TestList
    {
        public List<Test> list;

        public TestList(Test a)
        {
            if (list == null)
                list = new List<Test>();
            list.Add(a);
        }
    }
}

namespace Test2
{
    public class Test
    {
        public int intValue;
        public string strValue;

        public Test()
        {
            intValue = 10;
            strValue = "20";
        }
    }

    [System.Serializable]
    public class TestList
    {
        public string name;
        public Test[] testList = new Test[] { };

        public TestList()
        {
            testList = new Test[] { new Test() };
            name = "TestList";
        }
    }
}

