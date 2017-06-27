using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    public float throwForce = 2f;

    void Start() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Update() {
        device = SteamVR_Controller.Input((int)trackedObj.index);
    }

    void OnTriggerStay(Collider col) {
        if (col.gameObject.CompareTag("Throwable")) {
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {
                //Multi Throwing
                col.transform.SetParent(null);

				// marcelo: this GetComponent occurs only when the player is trying to release a ball
				// so there's no need to cache it
                Rigidbody rigidBody = col.GetComponent<Rigidbody>();
                rigidBody.isKinematic = false;

                rigidBody.velocity = device.velocity * throwForce;
                rigidBody.angularVelocity = device.angularVelocity;

			} else if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
                col.GetComponent<Rigidbody>().isKinematic = true;
                col.transform.SetParent(gameObject.transform);

                device.TriggerHapticPulse(2000);
            }
        }
    }

}
