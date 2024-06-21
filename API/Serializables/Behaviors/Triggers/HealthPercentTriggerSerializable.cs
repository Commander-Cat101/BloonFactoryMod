using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Serializables.Behaviors.Triggers
{
    internal class HealthPercentTriggerSerializable : CustomBloonBehaviorTriggerSerializable
    {
        public override string Name => "Health Percent Trigger";

        public float Percent = 50;
    }
}
