using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Serializables.Behaviors.Actions
{
    internal class StunTowersActionSerializable : CustomBloonBehaviorSerializable
    {
        public float StunDuration = 3;

        public float Radius = 60;
        public override string Name => "Stun Towers Action";
    }
}
