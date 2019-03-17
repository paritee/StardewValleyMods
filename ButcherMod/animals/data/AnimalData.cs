using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimalHusbandryMod.animals.data
{
    public class AnimalData
    {
        public const long PetId = -10;

        public List<AnimalItem> Livestock;
        public PetItem Pet;

        public AnimalData()
        {
            this.Livestock = new List<AnimalItem>()
            {
                new CowItem(),
                new PigItem(),
                new ChickenItem(),
                new DuckItem(),
                new RabbitItem(),
                new SheepItem(),
                new GoatItem(),
            };

            this.Pet = new PetItem();
        }

        public AnimalItem getAnimalItem(string animal)
        {
            AnimalItem item = Livestock.FirstOrDefault(o => o.Name == animal);

            if (item == null)
            {
                throw new ArgumentException("Invalid Animal");
            }

            return item;
        }
    }
}
