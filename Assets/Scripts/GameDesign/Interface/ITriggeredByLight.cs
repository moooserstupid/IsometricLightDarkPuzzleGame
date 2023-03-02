
using UnityEngine;


public interface ITriggeredByLight {
    void OnTriggerStay(Collider trigger) { }
    void OnTriggerEnter(Collider trigger) { }
    void OnTriggerExit(Collider trigger) { }
    void LightUpdate() { }
}
