using UnityEngine;

public class Manager : MonoBehaviour
{
    public TaipeiSway ts;

    Vector3 prevrefCord;

    void Start()
    {
        prevrefCord = ts.m_refCord;
    }

    void Update()
    {
        ts.refChange = prevrefCord != ts.m_refCord;

        prevrefCord = ts.m_refCord;
    }
}
