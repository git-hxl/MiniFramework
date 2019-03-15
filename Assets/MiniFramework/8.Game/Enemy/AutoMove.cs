using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AutoMove : MonoBehaviour
{
    public float Speed;
    public float RandomRange;
    [SerializeField]
    private Vector3 randomTargetPos;
    private Rigidbody mRigidbody;
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        GetRandomPos();
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, randomTargetPos) < 1)
        {
            GetRandomPos();
        }
    }
    void FixedUpdate()
    {
        Vector3 dir = (randomTargetPos - transform.position).normalized;
        dir.y = 0;
        SetLookDir(dir);
        mRigidbody.MovePosition(transform.position + transform.forward * Speed * Time.deltaTime);
    }
    void GetRandomPos()
    {
        Vector3 pos = new Vector3(Random.Range(-RandomRange, RandomRange), 0, Random.Range(-RandomRange, RandomRange));
        randomTargetPos = pos;
    }
    public void SetLookDir(Vector3 dir)
    {
        mRigidbody.MoveRotation(Quaternion.LookRotation(dir));
    }
    void OnCollisionEnter(Collision other)
    {
        GetRandomPos();
    }
}
