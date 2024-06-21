using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Serializables.Behaviors.Actions
{
    internal class DrainLivesActionSerializable : CustomBloonBehaviorSerializable
    {
        public int Lives = 10;
        public override string Name => "Drain Lives Action";
    }
}
