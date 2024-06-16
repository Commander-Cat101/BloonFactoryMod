using BloonFactoryMod.API.Serializables.Behaviors;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Models.Towers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Behaviors
{
    public abstract class CustomBloonBehavior : ModContent
    {
        public static Dictionary<Type, CustomBloonBehavior> BehaviorByType = new Dictionary<Type, CustomBloonBehavior>();

        public abstract Type SerializableType { get; }

        public abstract string Name { get; }
        public abstract ModHelperPanel CreatePanel(CustomBloonBehaviorSerializable serializable);

        public abstract void AddToTower(TowerModel tower, CustomBloonBehaviorSerializable serializable);

        public override void Register()
        {
            BehaviorByType[SerializableType] = this;
        }
    }

    public abstract class CustomBloonBehavior<T> : CustomBloonBehavior where T : CustomBloonBehaviorSerializable
    {
        public sealed override Type SerializableType => typeof(T);
    }
}
