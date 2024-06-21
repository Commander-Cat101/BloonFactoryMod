using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Serializables.Behaviors.Triggers
{
    internal class TimeTriggerSerializable : CustomBloonBehaviorTriggerSerializable
    {
        public override string Name => "Time Trigger";

        public float Interval = 5;
    }
}
