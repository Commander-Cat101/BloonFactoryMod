using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Serializables.Behaviors.Actions
{
    internal class DamageReductionSerializable : CustomBloonBehaviorSerializable
    {
        public int DamageReduction = 1;
        public override string Name => "Buff Bloon Speed";
    }
}
