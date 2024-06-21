using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Serializables.Behaviors.Actions
{
    internal class SpawnBloonActionSerializable : CustomBloonBehaviorSerializable
    {
        public string BloonType = "Red";

        public int Amount = 10;

        public int DistanceAhead = 45;

        public float Time = 0.02f;
        public override string Name => "Spawn Bloon Action";
    }
}
