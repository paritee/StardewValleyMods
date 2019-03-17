using System;
using System.ComponentModel;
using AnimalHusbandryMod.common;

namespace AnimalHusbandryMod.meats
{
    public static class MeatExtension
    {
        public static string GetDescription(this Meat value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static string GetObjectString(MeatItem meatItem)
        {
            var i18n = DataLoader.i18n;
            return String.Format("{0}/{1}/{2}/Basic -14/{3}/{4}", meatItem.Name, meatItem.Price, meatItem.Edibility, i18n.Get($"Meat.{meatItem.Name}.Name"), i18n.Get($"Meat.{meatItem.Name}.Description"));
        }        
    }
}
