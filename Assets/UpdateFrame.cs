using UnityEngine;
using System.Collections;

public class UpdateFrame : MonoBehaviour {
    public int targetFrameRate = 50;
    void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}
