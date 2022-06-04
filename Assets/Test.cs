using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AillieoUtils.EasyLogger;
using Logger = AillieoUtils.EasyLogger.Logger;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Logger l = LoggerFactory.GetLogger<Test>();
        l.Log("logger log");
        l.Warning("logger warning");
        l.Error("logger error");

        UnityEngine.Debug.Log("unity log");
        UnityEngine.Debug.LogWarning("unity warning");
        UnityEngine.Debug.LogError("unity error");

    }
}
