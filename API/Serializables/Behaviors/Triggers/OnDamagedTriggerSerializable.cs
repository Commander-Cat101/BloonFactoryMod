using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Serializables.Behaviors.Triggers
{
    internal class OnDamagedTriggerSerializable : CustomBloonBehaviorTriggerSerializable
    {
        public override string Name => "On Damaged Trigger";

        public float Cooldown = 2f;
    }
}
