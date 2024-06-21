using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Serializables.Behaviors.Triggers
{
    internal class WaitForSecondsTriggerSerializable : CustomBloonBehaviorTriggerSerializable
    {
        public override string Name => "Wait for Seconds Trigger";

        public float TimeToWait = 2;
    }
}
