using UnityEngine;
using System;

public class ObjectActivator1 : MonoBehaviour
{
    public ObjectActivator1(IntPtr ptr) : base(ptr) { }

    public GameObject objectToActivate;
    public KeyCode activationKey = KeyCode.Space;
    public bool startEnabled = false;

    private bool isActive = false;

    private void Start()
    {
        isActive = startEnabled;
        objectToActivate.SetActive(isActive);
    }

    private void Update()
    {
        if (Input.GetKeyDown(activationKey))
        {
            isActive = !isActive;
            objectToActivate.SetActive(isActive);
        }
    }

}