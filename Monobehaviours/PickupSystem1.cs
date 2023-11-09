using MelonLoader;
using SLZ.Props.Weapons;
using System;
using System.Threading.Tasks;
using UnityEngine;

// Picks things up via telekinesis.
public class PickupSystem1 : MonoBehaviour
{
    public PickupSystem1(IntPtr ptr) : base(ptr) { }

    public KeyCode holdKey = KeyCode.E;
    public KeyCode fireKey = KeyCode.Mouse0;
    public KeyCode throwKey = KeyCode.Mouse1;

    public LayerMask pickupLayerMask;

    public float throwForce = 40f;
    public float pickupDistance = 1f; // TODO: needs to be changed within dude spawnable

    public Animator playerAnimator;

    public GameObject rayCaster;

    public Gun HeldGun { 
        get
        {
            if (!IsHolding) return null;
            return HeldObject.GetComponent<Gun>();
        }
    }

    public Rigidbody HeldRigidbody
    {
        get
        {
            if (!IsHolding) return null;
            return HeldObject.GetComponent<Rigidbody>();
        }
    }

    private PickupSoundSystem soundSystem;

    private bool IsHolding => HeldObject != null;

    public Transform HeldObject { get; private set; }

    // TO BE CONVERTED INTO FIELDS
    private const float autoDropDistance = 5f; // once the distance between the player and the object is this distance or higher, it will auto-drop
    private const float smoothTime = 10f;
    private const float reloadTime = 1f; // time in seconds it takes to reload
    //

    public async void ReloadGunWithoutHands(Gun gun) // untested
    {
        gun.EjectCartridge();
        await Task.Delay(Mathf.RoundToInt(reloadTime * 60)); // TODO: add sounds or animations to indicate reloading.
        gun.MagazineState.AddCartridge(gun.defaultMagazine.rounds, gun.MagazineState.cartridgeData);
    }

    private void PickupObject(Transform toPickup)
    {
        playerAnimator.SetInteger("Swipe", 1);
        HeldObject = toPickup;
        HeldRigidbody.useGravity = false;
        MelonCoroutines.Start(soundSystem.PlayRandomSound());
    }

    private void ThrowHeldObject()
    {
        HeldRigidbody.AddForce(rayCaster.transform.forward.normalized * throwForce, ForceMode.Impulse);
        DropHeldObject();
    }

    private void DropHeldObject()
    {
        playerAnimator.SetInteger("Swipe", 0);
        HeldRigidbody.useGravity = true;
        HeldObject = null;
    }

    private void Start()
    {
        soundSystem = GetComponent<PickupSoundSystem>();
    }

    private void Update()
    {
        if (IsHolding && Vector3.Distance(HeldObject.position, rayCaster.transform.position) >= autoDropDistance) DropHeldObject();

        if (Input.GetKeyDown(holdKey))
        {
            if (!IsHolding && Physics.Raycast(rayCaster.transform.position, rayCaster.transform.forward, out RaycastHit hit, maxDistance: pickupDistance, pickupLayerMask) && hit.rigidbody != null && !hit.collider.gameObject.name.Contains("Dude"))
            {
                PickupObject(hit.transform);
            }
            else if (IsHolding)
            {
                DropHeldObject();
            }
        }

        if (Input.GetKeyDown(throwKey) && IsHolding)
        {
            ThrowHeldObject();
        }

        if (IsHolding && HeldGun != null)
        {
            if (Input.GetKey(fireKey))
            {
                HeldGun.Fire();
            }
            if (Input.GetKeyUp(fireKey))
            {
                HeldGun.CeaseFire();
                if (HeldGun.AmmoCount() <= 0)
                {
                    ReloadGunWithoutHands(HeldGun);
                }
            }
        }

    }

    private void FixedUpdate()
    {
        if (IsHolding)
        {
            Vector3 newPos = Vector3.MoveTowards(HeldObject.position, rayCaster.transform.position + (rayCaster.transform.forward * pickupDistance), smoothTime * Time.deltaTime);
            newPos = Vector3.Lerp(HeldObject.position, newPos, 0.75f);

            Rigidbody rb = HeldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.MovePosition(newPos);
                rb.MoveRotation(Quaternion.LookRotation(HeldObject.position - rayCaster.transform.position));
            }
        }
        
    }

}