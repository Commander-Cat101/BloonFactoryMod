using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Serializables.Behaviors
{
    public abstract class CustomBloonBehaviorTriggerSerializable : CustomBloonBehaviorSerializable 
    {
        public List<string> ActionIDs = new List<string>();
    }
}
