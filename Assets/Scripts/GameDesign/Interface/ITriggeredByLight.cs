using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public interface ITriggeredByLight {
    private void OnTriggerStay(Collider trigger) { }
}
