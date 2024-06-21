using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Serializables.Behaviors.Actions
{
    internal class BuffBloonSpeedSerializable : CustomBloonBehaviorSerializable
    {
        public float DebuffRadius = 45;

        public float Multiplier = 2.5f;
        public override string Name => "Buff Bloon Speed";
    }
}
