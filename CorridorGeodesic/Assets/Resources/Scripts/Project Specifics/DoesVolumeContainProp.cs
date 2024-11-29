using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neverway.Framework.LogicSystem
{
    public class DoesVolumeContainProp : LogicComponent
    {
        public Volume volume;

        void Update()
        {
            isPowered = volume.propsInTrigger.Count > 0;
        }
    }
}
