using UnityEngine;

public class earthquake : MonoBehaviour
{
    public float m_xAmplitude = 1f;
    public float m_zAmplitude = 1f;

    public float m_xFrequency = 1f;
    public float m_zFrequency = 1f;

    public float m_timeScale = 1f;

    float swaytime;
    Vector3 pos;

    [HideInInspector]
    public float ampx, ampz, count = 0f;
    [HideInInspector]
    public bool quake = false, ampchange = false;

    float prev_xamp;
    float prev_zamp;

    void Awake()
    {
        pos = transform.position;
        prev_xamp = m_xAmplitude;
        prev_zamp = m_zAmplitude;
    }
    void Update()
    {
        ampchange = prev_xamp != m_xAmplitude || prev_zamp != m_zAmplitude;

        //if either amplitude is greater than threshold amplitude, quake returns true
        quake = (m_xAmplitude > 0.005f || m_zAmplitude > 0.005);

        //if quake is true, then earthquake happens
        if (quake)
        {
            swaytime += (Time.deltaTime * m_timeScale);
            float omega_x = 2 * Mathf.PI * m_xFrequency;
            float omega_z = 2 * Mathf.PI * m_zFrequency;

            Earthquake(omega_x * swaytime, omega_z * swaytime);
        }

        prev_xamp = m_xAmplitude;
        prev_zamp = m_zAmplitude;
    }
    
    void Earthquake(float phasex, float phasez)
    {
        //periodic oscillating vibration is given
        ampx = m_xAmplitude * Mathf.Sin(phasex);
        ampz = m_zAmplitude * Mathf.Sin(phasez);

        transform.position = pos + new Vector3(ampx, 0f, ampz);

        //only works if oscillation is periodic, like here sin wave with period 2*pi
        if (phasex > 2 * Mathf.PI || phasez > 2 * Mathf.PI)
        {
            swaytime = 0f;
            phasex = phasex - 2 * Mathf.PI;
            phasez = phasez - 2 * Mathf.PI;
        }
    }
}
