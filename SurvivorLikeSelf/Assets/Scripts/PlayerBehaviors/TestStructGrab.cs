using System;
using System.Diagnostics;
using UnityEngine;

public class TestStructGrab : MonoBehaviour
{
    private float _speedField = 1f;
    [SerializeField]
    private TestGrab _testGrab;
    [SerializeField]
    private TestGrabClass _testGrabClass;

    float placeholderSpeed = 0f;

    [Serializable]
    private struct TestGrab
    {
        public float speed;
    }

    [Serializable]
    private class TestGrabClass
    {
        public float speed;
    }

    void OnEnable()
    {
        Stopwatch st = new Stopwatch();
        st.Start();
        for (int i = 0; i < 1000000; i++)
        {
            placeholderSpeed = _speedField;
        }
        st.Stop();
        UnityEngine.Debug.Log((float)st.Elapsed.TotalMilliseconds);
    }

    void OnDisable()
    {
        Stopwatch st = new Stopwatch();
        st.Start();
        for (int i = 0; i < 1000000; i++)
        {
            placeholderSpeed = _testGrab.speed;
        }
        st.Stop();
        UnityEngine.Debug.Log((float)st.Elapsed.TotalMilliseconds);
    }


}
