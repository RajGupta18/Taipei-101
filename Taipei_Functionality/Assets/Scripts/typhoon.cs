using UnityEngine;

public class typhoon : MonoBehaviour
{
    public float force;
    public Rigidbody rb;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(transform.forward * force * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.AddForce(-transform.forward * force * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(transform.right * force * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(-transform.right * force * Time.deltaTime);
        }
    }
}
