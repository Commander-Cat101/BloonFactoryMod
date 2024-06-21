using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Serializables.Behaviors.Actions
{
    internal class SetInvincibleActionSerializable : CustomBloonBehaviorSerializable
    {
        public bool IsInvincible = true;
        public override string Name => "Set Invincible Action";
    }
}
