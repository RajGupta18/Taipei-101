using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vibration : MonoBehaviour
{
    public int Flag=0;
    public float distance;
    public Vector3 temp;
    public GameObject c1;
    public GameObject c2;
    Mesh ms;
    Vector3[] vertices;
    public float angle;
    Vector3 pos_z_axis = new Vector3(0, 0, 1);
    Vector3 neg_z_axis = new Vector3(0, 0, -1);

    float hmin, hmax;

    private void Start()
    {
        ms = c2.GetComponent<MeshFilter>().mesh;
        vertices = ms.vertices;

        for(int i =0; i<vertices.Length; i++)
        {
            vertices[i] = c2.transform.TransformPoint(vertices[i]);
        }

        hmin = vertices[0].y;
        hmax = vertices[0].y;
        for(int i =0; i<vertices.Length; i++)
        {
            float h = vertices[i].y;
            if (h <= hmin)
                hmin = h;
            if (h >= hmax)
                hmax = h;
        }
    }
    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(c1.transform.position, c2.transform.position);
        
        if(Vector3.Dot(c1.transform.position,pos_z_axis) < 1/Mathf.Pow(2,1/2))
        {
            vibration_front();
        }
        if (Vector3.Dot(c1.transform.position,pos_z_axis) >= 1 / Mathf.Pow(2, 1 / 2))
        {
            vibration_right();
        }
        /*if (Vector3.Dot(c1.transform.position,neg_z_axis) < 1 / Mathf.Pow(2, 1 / 2))
        {
            vibration_front();
        }
        if (Vector3.Dot(c1.transform.position,neg_z_axis) >= 1 / Mathf.Pow(2, 1 / 2))
        {
            vibration_right();
        }*/

    }

    void vibration_front()
    {
        if (distance <= 20f)
        {
            if (Flag == 0)
            {
                temp = c2.transform.position;
                temp += new Vector3(0, 0, 5 / distance);
                c2.transform.position = temp;
                Flag = 1;
            }

            else if (Flag == 1)
            {
                temp -= new Vector3(0, 0, 5 / distance);
                c2.transform.position = temp;
                Flag = 0;
            }
        }
    }

    void vibration_right()
    {
        if (distance <= 20f)
        {
            if (Flag == 0)
            {
                temp = c2.transform.position;
                temp += new Vector3(5 / distance, 0, 0);
                c2.transform.position = temp;
                Flag = 1;
            }

            else if (Flag == 1)
            {
                temp -= new Vector3(5 / distance, 0, 0);
                c2.transform.position = temp;
                Flag = 0;
            }
        }
    }
}
