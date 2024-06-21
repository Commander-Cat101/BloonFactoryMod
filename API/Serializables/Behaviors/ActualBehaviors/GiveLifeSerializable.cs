using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Serializables.Behaviors.Actions
{
    internal class GiveLifeSerializable : CustomBloonBehaviorSerializable
    {
        public int Life = 1;
        public override string Name => "Give Life";
    }
}
