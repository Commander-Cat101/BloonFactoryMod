using BloonFactoryMod.API.Serializables;
using BTD_Mod_Helper.Api.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.UI
{
    public static class UIHelper
    {
        public static ModHelperSlider SetSelectable(this ModHelperSlider slider, CustomBloonSave save)
        {
            if (save.IsCustomDisplay)
            {
                slider.Slider.interactable = false;
                slider.Slider.handleRect.GetComponent<ModHelperImage>().Image.color = new UnityEngine.Color(0.6f, 0.6f, 0.6f);
            }
            return slider;
        }
    }
}
