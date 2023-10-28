using UnityEngine;
using System;
public class ObjectInteractable : MonoBehaviour
{
    public ObjectInteractable(IntPtr ptr) : base(ptr) { }

    public GameObject target;
    public float smoothTime = 0.05f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void OnDestroy()
    {
        rb.useGravity = true;
    }

    private void FixedUpdate()
    {
        Vector3 newPos = Vector3.MoveTowards(transform.position, target.transform.position, smoothTime * Time.deltaTime);
        newPos = Vector3.Lerp(transform.position, newPos, 0.75f);
        transform.position = newPos;
    }

}
