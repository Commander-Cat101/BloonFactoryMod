using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Serializables.Behaviors.Actions
{
    internal class MoveActionSerializable : CustomBloonBehaviorSerializable
    {
        public float Time = 0.5f;

        public int Distance = 50;
        public override string Name => "Move Action";
    }
}
