using UnityEngine;

public class RigbodyMove : MonoBehaviour
{
    public float speed = 5;
    public float angleSpeed = 10;
    private Rigidbody rig;
    private Vector3 dir;
    // Use this for initialization
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        dir = new Vector3(x, 0, z);
        rig.MovePosition(rig.position + dir * speed * Time.deltaTime);
        rig.transform.forward = Vector3.Slerp(transform.forward, dir.normalized, angleSpeed * Time.deltaTime);
    }
}
