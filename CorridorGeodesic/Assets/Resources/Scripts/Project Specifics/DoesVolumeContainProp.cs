using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoesVolumeContainProp : LogicComponent
{
    public Volume volume;

    void Update()
    {
        isPowered = volume.propsInTrigger.Count > 0;
    }
}
