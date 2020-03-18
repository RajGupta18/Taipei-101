using UnityEngine;

public class SwayRadial : MonoBehaviour
{
    public float m_swayFrequency_x = 0f;
    public float m_swayFrequency_z = 0f;

    public float m_swayAmplitude_x = 0f;
    public float m_swayAmplitude_z = 0f;

    public float m_thresholdAmplitude;

    //public GameObject m_referenceObj;

    Mesh mesh;
    float swaytime;
    float r_min;
    float r_max;
    Vector3[] originalVert;

    void Start() {
        swaytime = 0f;

        mesh = GetComponent<MeshFilter>().mesh;
        originalVert = mesh.vertices;

        r_min = transform.TransformPoint(originalVert[0]).y;
        r_max = transform.TransformPoint(originalVert[0]).y;
        for(int i = 0; i < originalVert.Length; i++) {
            float radius = transform.TransformPoint(originalVert[i]).y;
            if(radius <= r_min) {
                r_min = radius;
            }
            if(radius >= r_max) {
                r_max = radius;
            }
        }
    }

    void Update() {
        swaytime += Time.deltaTime;

        float omega_x = 2*m_swayFrequency_x*Mathf.PI;
        float omega_z = 2*m_swayFrequency_z*Mathf.PI;

        SwayMovement(omega_x*swaytime, omega_z*swaytime);
    }

    void SwayMovement(float swayPhase_x, float swayPhase_y) {
        float theta_x = (m_swayAmplitude_x < m_thresholdAmplitude)? 0f : Mathf.Deg2Rad*(m_swayAmplitude_x - m_thresholdAmplitude)* Mathf.Sin(swayPhase_x);     //angular sway equation in radian
        float theta_z = (m_swayAmplitude_z < m_thresholdAmplitude)? 0f : Mathf.Deg2Rad*(m_swayAmplitude_z - m_thresholdAmplitude)* Mathf.Sin(swayPhase_y);

        Vector3[] vert = mesh.vertices;
        for(int i =0; i < originalVert.Length; i++) {
            Vector3 originalWorldPos = transform.TransformPoint(originalVert[i]);
            float radius = transform.TransformPoint(originalVert[i]).y;
            float swayHeightFactor = (radius - r_min) / (r_max - r_min);    //scale eefect of angular sway w.r.t height from reference plane

            float netAmplitude_y = Mathf.Cos(swayHeightFactor*Mathf.Sqrt(theta_x*theta_x + theta_z*theta_z));

            vert[i] = transform.InverseTransformPoint(new Vector3(radius*Mathf.Sin(swayHeightFactor*theta_x) + originalWorldPos.x,
                                                                    radius*netAmplitude_y,
                                                                    radius*Mathf.Sin(swayHeightFactor*theta_z)+originalWorldPos.z));
        }

        mesh.vertices = vert;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
