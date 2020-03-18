using UnityEngine;

public class ty : MonoBehaviour
{
    public Vector3 origin;
    public float radius;
    public float theta; //in degree
    public float amp;

    [HideInInspector]
    public float ampx = 0f, ampz = 0f;
    [HideInInspector]
    public bool typhoon = false,ampchange = false, thetachange = false;

    Vector3 normal;
    float prevamp;
    float prevtheta;

    private void Start()
    {
        prevamp = amp;
        prevtheta = theta;

        typhoonChange();
    }
    private void Update()
    {
        ampchange = prevamp != amp;
        thetachange = prevtheta != theta;

        typhoon = ampx >= 0.005f || ampz >= 0.005f;

        typhoonChange();

        prevamp = amp;
        prevtheta = theta;
    }

    void typhoonChange()
    {
        float x = radius * Mathf.Cos(theta * Mathf.Deg2Rad);
        float z = radius * Mathf.Sin(theta * Mathf.Deg2Rad);

        transform.position = origin + new Vector3(x, 0f, z);
        normal = origin - transform.position;
        normal = normal.normalized;
        ampx = amp * normal.x;
        ampz = amp * normal.z;
    }
}
