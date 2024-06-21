using BloonFactoryMod.API.Serializables.Behaviors.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Serializables.Behaviors
{
    public abstract class CustomBloonBehaviorSerializable
    {
        public string GUID { get; set; }

        public abstract string Name { get; }
    }
}
