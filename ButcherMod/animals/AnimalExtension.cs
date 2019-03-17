using AnimalHusbandryMod.common;
using AnimalHusbandryMod.meats;
using StardewValley;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalHusbandryMod.animals
{
    public static class AnimalExtension
    {
        public static int GetMeat(string animal)
        {
            Dictionary<string, string> data = Game1.content.Load<Dictionary<string, string>>(Path.Combine("Data", "FarmAnimals"));

            if (!data.ContainsKey(animal))
            {
                return (int)Meat.Beef;
            }

            string[] values = data[animal].Split('/');

            if (!int.TryParse(values[23], out int index))
            {
                return (int)Meat.Beef;
            }

            return index;
        }

        public static string GetAnimalFromType(string type)
        {
            foreach (string key in DataLoader.AnimalData.Livestock.Keys)
            {
                if (type.Contains(key))
                {
                    return key;
                }
            }

            return null;
        }
    }
}
