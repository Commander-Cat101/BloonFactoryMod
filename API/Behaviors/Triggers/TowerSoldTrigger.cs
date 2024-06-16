using BloonFactoryMod.API.Serializables.Behaviors;
using BloonFactoryMod.API.Serializables.Behaviors.Triggers;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Behaviors.Triggers
{
    internal class TowerSoldTrigger : CustomBloonBehavior<TowerSoldTriggerSerializable>
    {
        public override string Name => "Tower Sold Trigger";
        public override void AddToTower(TowerModel tower, CustomBloonBehaviorSerializable serializable)
        {

        }

        public override ModHelperPanel CreatePanel(CustomBloonBehaviorSerializable serializable)
        {
            return ModHelperPanel.Create(new Info("Panel", 0, 0, 900, 600), VanillaSprites.MainBGPanelBlue);
        }
    }
}
