using UnityEngine;
using System;
using SLZ.Props.Weapons;
#if DEBUG
using MelonLoader;
#endif
public class PickupSystem1 : MonoBehaviour
{
    public PickupSystem1(IntPtr ptr) : base(ptr) { }

    public KeyCode holdKey = KeyCode.E;

    public KeyCode fireKey = KeyCode.Mouse0;

    public KeyCode throwKey = KeyCode.Mouse1;

    public float throwForce = 10f;

    public LayerMask pickupLayerMask;

    public float pickupDistance = 3f;

    public Animator playerAnimator;

    public GameObject rayCaster;

    public float smoothing = 0.05f;

    private Rigidbody rb;
    private bool isHolding = false;
    private Transform heldObject;
    private PickupSoundSystem soundSystem;
    private Gun heldGun;

    private void Start()
    {
        soundSystem = GetComponent<PickupSoundSystem>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(holdKey))
        {
            // Check if there is an object in front of the player that can be picked up
            RaycastHit hit;
            if (Physics.Raycast(rayCaster.transform.position, rayCaster.transform.forward, out hit, maxDistance:pickupDistance, pickupLayerMask) && !hit.rigidbody.isKinematic && !hit.collider.gameObject.name.Contains("Dude"))
            {
                // Pick up the object
                playerAnimator.SetInteger("Swipe", 1);
                //idk why but it no work
                //soundSystem.StartCoroutine("PlayRandomSound");
                heldObject = hit.transform;
                ObjectInteractable i = heldObject.gameObject.AddComponent<ObjectInteractable>();
                i.smoothTime = smoothing;
                GameObject obj = new GameObject("HoldTarget");
                obj.transform.parent = rayCaster.transform;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = Vector3.zero;
                obj.transform.position = rayCaster.transform.position + rayCaster.transform.forward * 1.5f;
                i.target = obj;
                isHolding = true;
                heldGun = heldObject.GetComponent<Gun>();
#if DEBUG
                MelonLogger.Msg($"Held gun object name: { heldGun.gameObject.name}");
                MelonLogger.Msg($"Held object name: {heldObject.gameObject.name}");
                MelonLogger.Msg($"Object to search for held gun name: {hit.rigidbody.gameObject.name}");
#endif
            }
        }

        if (Input.GetKeyUp(holdKey))
        {
            // Throw the object
            if (isHolding)
            {
                Destroy(heldObject.GetComponent<ObjectInteractable>());
                Destroy(rayCaster.transform.GetChild(0).gameObject);
                playerAnimator.SetInteger("Swipe", 0);
                heldObject = null;
                isHolding = false;
                heldGun = null;
            }
        }

        if (Input.GetKeyDown(throwKey) && isHolding)
        {
            Destroy(heldObject.GetComponent<ObjectInteractable>());
            Destroy(rayCaster.transform.GetChild(0).gameObject);
            heldObject.GetComponent<Rigidbody>().AddForce(rayCaster.transform.forward * throwForce, ForceMode.Impulse);
            playerAnimator.SetInteger("Swipe", 0);
            heldObject = null;
            isHolding = false;
            heldGun = null;
        }

        if (isHolding)
        {
            heldObject.transform.rotation = Quaternion.LookRotation(heldObject.transform.position - rayCaster.transform.position);
        }

        if (isHolding && heldGun != null)
        {
            if (Input.GetKey(fireKey))
            {
                heldGun.Fire();
            }
            if (Input.GetKeyUp(fireKey))
            {
                heldGun.CeaseFire();
            }
        }

    }

}