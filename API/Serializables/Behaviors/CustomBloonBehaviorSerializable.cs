using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Serializables.Behaviors
{
    []
    public abstract class CustomBloonBehaviorSerializable
    {
        public string GUID { get; set; }

    }

    public static class BehaviorHelper
    {
        public static T CreateBehavior<T>()
            where T : CustomBloonBehaviorSerializable
        {
            T behavior = (T)Activator.CreateInstance(typeof(T));
            string guid = new Guid().ToString();
            behavior.GUID = guid;
            return behavior;
        }
    }
}
