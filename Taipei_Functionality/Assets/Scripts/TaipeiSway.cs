using UnityEngine;

public class TaipeiSway : MonoBehaviour
{
    public Vector3 m_refCord;
    public GameObject m_building;
    public earthquake eq;
    //public ty typ;
    public float ampscale;

    public bool damped = true;
    public float m_dampingRatio = 0.5f;

    float m_swayFrequency_x = 0f;
    float m_swayFrequency_z = 0f;

    float m_swayAmplitude_x = 0f;
    float m_swayAmplitude_z = 0f;

    float m_timescale = 0.01f;

    Mesh[] meshes;

    [HideInInspector]
    public MeshFilter[] mf;
    public Vector3[][] meshesVertices;

    [HideInInspector]
    public bool refChange = false;

    Vector3 prevRefCord;
    float r_min;
    float r_max;
    float swaytime;
    float phase_x = 0f;
    float phase_z = 0f;

    void Awake()
    {
        prevRefCord = new Vector3(0f,0f,0f);

        //get mesh and vertices of gameobject
        mf = m_building.GetComponentsInChildren<MeshFilter>();
        meshes = new Mesh[mf.Length];
        meshesVertices = new Vector3[mf.Length][];

        //get meshes and vertices of all children object
        for(int i =0; i < mf.Length; i++)
        {
            meshes[i] = mf[i].mesh;
            meshesVertices[i] = meshes[i].vertices;
        }

        //Change vertices from local to world
        for (int i =0; i<mf.Length; i++)
        {
            for (int j =0; j < meshesVertices[i].Length; j++)
            {
                meshesVertices[i][j] = mf[i].gameObject.transform.TransformPoint(meshesVertices[i][j]) - prevRefCord + m_refCord;
            }
        }

        //get r_max and r_min
        r_min = meshesVertices[0][0].y;
        r_max = meshesVertices[0][0].y;
        for (int i = 0; i < mf.Length; i++)
        {
            for(int j =0; j < meshesVertices[i].Length; j++)
            {
                float radius = meshesVertices[i][j].y;
                if (radius <= r_min)
                    r_min = radius;
                if (radius >= r_max)
                    r_max = radius;
            }
        }

        prevRefCord = m_refCord;
    }
    
    void Update()
    {
        //executes if reference coordinate changes
        if (refChange)
        {
            //Change vertices from local to world and get rmin and rmax
            r_min = meshesVertices[0][0].y;
            r_max = meshesVertices[0][0].y;
            for (int i = 0; i < mf.Length; i++)
            {
                meshesVertices[i] = meshes[i].vertices;
                for (int j = 0; j < meshesVertices[i].Length; j++)
                {
                    meshesVertices[i][j] = mf[i].gameObject.transform.TransformPoint(meshesVertices[i][j]) - prevRefCord + m_refCord;

                    float radius = meshesVertices[i][j].y;
                    if (radius <= r_min)
                        r_min = radius;
                    if (radius >= r_max)
                        r_max = radius;
                }
            }
            prevRefCord = m_refCord;
        }

        //executes only if earthquake is true
        if (eq.quake)
        {
            m_dampingRatio = Mathf.Clamp(m_dampingRatio, 0f, 1f);   //define range of damping ratio(0-1)

            m_swayFrequency_x = eq.m_xFrequency;
            m_swayFrequency_z = eq.m_zFrequency;

            swaytime = Time.deltaTime*m_timescale;
        
            float omega_x, omega_z;
            if (!damped)    //undamped
            {
                omega_x = 2 * m_swayFrequency_x * Mathf.PI;
                omega_z = 2 * m_swayFrequency_z * Mathf.PI;
            }
            else    //damped (underDamped)
            {
                omega_x = 2 * m_swayFrequency_x * Mathf.PI * Mathf.Sqrt(1f - (m_dampingRatio * m_dampingRatio));
                omega_z = 2 * m_swayFrequency_z * Mathf.PI * Mathf.Sqrt(1f - (m_dampingRatio * m_dampingRatio));
            }

            if (eq.ampchange || eq.count == 0)
            {
                m_swayAmplitude_x = -eq.m_xAmplitude/ampscale;
                m_swayAmplitude_z = -eq.m_zAmplitude/ampscale;
                eq.count = 1f;
            }

            m_timescale = eq.m_timeScale;

            SwayMovement(omega_x * swaytime, omega_z * swaytime);

            phase_x = (phase_x > 2 * Mathf.PI) ? phase_x - (2 * Mathf.PI) + (omega_x * swaytime) : phase_x + (omega_x * swaytime);
            phase_z = (phase_z > 2 * Mathf.PI) ? phase_z - (2 * Mathf.PI) + (omega_x * swaytime) : phase_z + (omega_z * swaytime);
        }
    }

    void SwayMovement(float swayPhase_x, float swayPhase_z)
    {
        Vector3[][] newVertices = new Vector3[mf.Length][];

        //get vertices of all children object
        for (int i = 0; i < mf.Length; i++)
        {
            newVertices[i] = meshes[i].vertices;
        }

        //angular sway equation in radian
        float theta_x = Mathf.Deg2Rad * m_swayAmplitude_x;
        float theta_z = Mathf.Deg2Rad * m_swayAmplitude_z;

        if (Mathf.Sin(phase_x) != 0)
            m_swayAmplitude_x = m_swayAmplitude_x * Mathf.Sin(swayPhase_x + phase_x) / Mathf.Sin(phase_x);
        else
            m_swayAmplitude_x = m_swayAmplitude_x * Mathf.Sin(swayPhase_x + phase_x);

        if (Mathf.Sin(phase_z) != 0)
            m_swayAmplitude_z = m_swayAmplitude_z * Mathf.Sin(swayPhase_z + phase_z) / Mathf.Sin(phase_z);
        else
            m_swayAmplitude_z = m_swayAmplitude_z * Mathf.Sin(swayPhase_z + phase_z);

        if (damped)
        {
            m_swayAmplitude_x = m_swayAmplitude_x * Mathf.Exp(-m_dampingRatio * swayPhase_x);
            m_swayAmplitude_z = m_swayAmplitude_z * Mathf.Exp(-m_dampingRatio * swayPhase_z);
        }
        
        for (int i =0; i<mf.Length; i++)
        {
            for(int j =0; j<meshesVertices[i].Length; j++)
            {
                //scale effect of angular sway w.r.t height from reference plane
                float radius = meshesVertices[i][j].y;
                float swayHeightFactor = (radius - r_min) / (r_max - r_min);

                float netAmplitude_y = Mathf.Cos(swayHeightFactor * Mathf.Sqrt(theta_x * theta_x + theta_z * theta_z));
                newVertices[i][j] = mf[i].gameObject.transform.InverseTransformPoint(new Vector3(radius * Mathf.Sin(swayHeightFactor * theta_x) + meshesVertices[i][j].x + eq.ampx,
                                                                                                radius * netAmplitude_y,
                                                                                                radius * Mathf.Sin(swayHeightFactor * theta_z) + meshesVertices[i][j].z + eq.ampz));
            }
        }

        //update vertices of meshes
        for(int i =0; i<mf.Length; i++)
        {
            meshes[i].vertices = newVertices[i];
            meshes[i].RecalculateNormals();
            meshes[i].RecalculateBounds();
        }
    }
}