﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimalHusbandryMod.common;
using AnimalHusbandryMod.animals.data;
using AnimalHusbandryMod.meats;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Menus;
using Object = StardewValley.Object;

namespace AnimalHusbandryMod.animals
{
    public class MeatController : AnimalStatusController
    {
        public static bool CanGetMeatFrom(FarmAnimal farmAnimal)
        {
            if (farmAnimal.type.Value == "Dinosaur" && !DataLoader.ModConfig.DisableMeatFromDinosaur)
            {
                return true;
            }
            try
            {
                return GetAnimalItem(farmAnimal) != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static AnimalItem GetAnimalItem(FarmAnimal farmAnimal)
        {
            List<Item> itemsToReturn = new List<Item>();

            string foundAnimal = AnimalExtension.GetAnimalFromType(farmAnimal.type.Value);
            return DataLoader.AnimalData.getAnimalItem(foundAnimal);
        }

        public static List<Item> CreateMeat(FarmAnimal farmAnimal)
        {
            List<Item> itemsToReturn = new List<Item>();

            string animal;
            string foundAnimal = AnimalExtension.GetAnimalFromType(farmAnimal.type.Value);
            if (foundAnimal == null) 
            {
                return itemsToReturn;
            }
            else
            {
                animal = foundAnimal;
            }

            AnimalItem animalItem;
            int meatIndex;
            int minimumNumberOfMeat;
            int maxNumberOfMeat;
            int meatPrice;
            if (animal == "Dinosaur")
            {
                if (DataLoader.ModConfig.DisableMeatFromDinosaur)
                {
                    return itemsToReturn;
                }
                animalItem = null;
                var meats = Enum.GetValues(typeof(Meat));
                meatIndex = (int)meats.GetValue(new Random((int)farmAnimal.myID.Value).Next(meats.Length));
                minimumNumberOfMeat = 1;
                meatPrice = DataLoader.MeatData.getMeatItem(meatIndex).Price;
                maxNumberOfMeat = 1 + (1300 / meatPrice);
            }
            else
            {
                animalItem = DataLoader.AnimalData.getAnimalItem(animal);
                meatIndex = AnimalExtension.GetMeat(animal);
                minimumNumberOfMeat = animalItem.MinimalNumberOfMeat;
                meatPrice = DataLoader.MeatData.getMeatItem(meatIndex).Price;
                maxNumberOfMeat = animalItem.MaximumNumberOfMeat;
                
            }
            var debrisType = (int)meatIndex;
            var numberOfMeat = minimumNumberOfMeat;

            numberOfMeat += (int)((farmAnimal.getSellPrice() / ((double)farmAnimal.price.Value) - 0.3) * (maxNumberOfMeat - minimumNumberOfMeat));

            Random random = new Random((int)farmAnimal.myID.Value * 10000 + (int)Game1.stats.DaysPlayed);
            int[] quality = { 0, 0, 0, 0, 0 };
            for (int i = 0; i < numberOfMeat; i++)
            {
                var produceQuality = ProduceQuality(random, farmAnimal);
                quality[produceQuality]++;
            }

            var tempTotal = meatPrice * quality[0] + meatPrice * quality[1] * 1.25 + meatPrice * quality[2] * 1.5 + meatPrice * quality[4] * 2;
            while (tempTotal < farmAnimal.getSellPrice() && quality[4] != numberOfMeat)
            {
                if (numberOfMeat < maxNumberOfMeat)
                {
                    numberOfMeat++;
                    quality[0]++;
                    tempTotal += meatPrice;
                }
                else if (quality[0] > 0)
                {
                    quality[0]--;
                    quality[1]++;
                    tempTotal += meatPrice * 0.25;
                }
                else if ((quality[1] > 0))
                {
                    quality[1]--;
                    quality[2]++;
                    tempTotal += meatPrice * 0.25;
                }
                else if ((quality[2] > 0))
                {
                    quality[2]--;
                    quality[4]++;
                    tempTotal += meatPrice * 0.50;
                }
            }

            for (; numberOfMeat > 0; --numberOfMeat)
            {
                Object newItem = new Object(Vector2.Zero, debrisType, 1);
                newItem.Quality = quality[4] > 0 ? 4 : quality[2] > 0 ? 2 : quality[1] > 0 ? 1 : 0;
                quality[newItem.Quality]--;

                itemsToReturn.Add(newItem);
            }

            if ((animal == "Sheep" || animal == "Rabbit"))
            {
                WoolAnimalItem woolAnimalItem = (WoolAnimalItem)animalItem;
                int numberOfWools = farmAnimal.currentProduce.Value > 0 ? 1 : 0;
                numberOfWools += (int)(woolAnimalItem.MinimumNumberOfExtraWool + (farmAnimal.getSellPrice() / ((double)farmAnimal.price.Value) - 0.3) * (woolAnimalItem.MaximumNumberOfExtraWool - woolAnimalItem.MinimumNumberOfExtraWool));

                for (; numberOfWools > 0; --numberOfWools)
                {
                    Object newItem = new Object(Vector2.Zero, farmAnimal.defaultProduceIndex.Value, 1);
                    newItem.Quality = ProduceQuality(random, farmAnimal);
                    itemsToReturn.Add(newItem);
                }
            }

            if (animal == "Duck")
            {
                FeatherAnimalItem featherAnimalItem = (FeatherAnimalItem)animalItem;
                int numberOfFeather = (int)(featherAnimalItem.MinimumNumberOfFeatherChances + (farmAnimal.getSellPrice() / ((double)farmAnimal.price.Value) - 0.3) * (featherAnimalItem.MaximumNumberOfFeatherChances - featherAnimalItem.MinimumNumberOfFeatherChances));
                float num1 = (int)farmAnimal.happiness.Value > 200 ? (float)farmAnimal.happiness.Value * 1.5f : ((int)farmAnimal.happiness.Value <= 100 ? (float)((int)farmAnimal.happiness.Value - 100) : 0.0f);
                for (; numberOfFeather > 0; --numberOfFeather)
                {
                    if (random.NextDouble() < (double)farmAnimal.happiness.Value / 150.0)
                    {
                        if (random.NextDouble() < ((double)farmAnimal.friendshipTowardFarmer.Value + (double)num1) / 5000.0 + Game1.dailyLuck + (double)Game1.player.LuckLevel * 0.01)
                        {
                            Object newItem = new Object(Vector2.Zero, farmAnimal.deluxeProduceIndex.Value, 1);
                            newItem.Quality = ProduceQuality(random, farmAnimal);
                            itemsToReturn.Add(newItem);
                        }
                    }
                }
            }

            if (animal == "Rabbit")
            {
                FeetAnimalItem feetAnimalItem = (FeetAnimalItem)animalItem;
                int numberOfFeet = (int)(feetAnimalItem.MinimumNumberOfFeetChances + (farmAnimal.getSellPrice() / ((double)farmAnimal.price.Value) - 0.3) * (feetAnimalItem.MaximumNumberOfFeetChances - feetAnimalItem.MinimumNumberOfFeetChances));
                float num1 = (int)farmAnimal.happiness.Value > 200 ? (float)farmAnimal.happiness.Value * 1.5f : ((int)farmAnimal.happiness.Value <= 100 ? (float)((int)farmAnimal.happiness.Value - 100) : 0.0f);
                for (; numberOfFeet > 0; --numberOfFeet)
                {
                    if (random.NextDouble() < (double)farmAnimal.happiness.Value / 150.0)
                    {
                        if (random.NextDouble() < ((double)farmAnimal.friendshipTowardFarmer.Value + (double)num1) / 5000.0 + Game1.dailyLuck + (double)Game1.player.LuckLevel * 0.01)
                        {
                            Object newItem = new Object(Vector2.Zero, farmAnimal.deluxeProduceIndex.Value, 1);
                            newItem.Quality = ProduceQuality(random, farmAnimal);
                            itemsToReturn.Add(newItem);
                        }
                    }
                }
            }

            return itemsToReturn;
        }

        public static void ThrowItem(List<Item> newItems, FarmAnimal farmAnimal)
        {
            GameLocation location = Game1.currentLocation;
            int xTile = farmAnimal.getTileX() - 1;
            int yTile = farmAnimal.getTileY() - 1;
            Vector2 debrisOrigin = new Vector2((float)(xTile * Game1.tileSize + Game1.tileSize),(float)(yTile * Game1.tileSize + Game1.tileSize));

            foreach (Item newItem in newItems)
            {
                switch (Game1.random.Next(4))
                {
                    case 0:
                        location.debris.Add(new Debris(newItem, debrisOrigin,
                            debrisOrigin + new Vector2(-Game1.tileSize, 0.0f)));
                        break;
                    case 1:
                        location.debris.Add(new Debris(newItem, debrisOrigin,
                            debrisOrigin + new Vector2(Game1.tileSize, 0.0f)));
                        break;
                    case 2:
                        location.debris.Add(new Debris(newItem, debrisOrigin,
                            debrisOrigin + new Vector2(0.0f, Game1.tileSize)));
                        break;
                    case 3:
                        location.debris.Add(new Debris(newItem, debrisOrigin,
                            debrisOrigin + new Vector2(0.0f, -Game1.tileSize)));
                        break;
                }
            }
        }

        public static void AddItemsToInventoryByMenuIfNecessary(List<Item> items, ItemGrabMenu.behaviorOnItemSelect itemSelectedCallback = null)
        {
            Game1.player.addItemsByMenuIfNecessary(
                new List<Item>(
                    items
                    .GroupBy(i => new {id = i.ParentSheetIndex, (i as Object).Quality })
                    .Select(g => new Object(Vector2.Zero, g.Key.id, g.Count()) {Quality = g.Key.Quality })
                    .ToList()
                ),
                itemSelectedCallback
            );
        }

        private static int ProduceQuality(Random random, FarmAnimal farmAnimal)
        {
            AnimalStatus animalStatus = GetAnimalStatus(farmAnimal.myID.Value);
            if (animalStatus.HasWon??false)
            {
                return 4;
            }

            double chance = (double)farmAnimal.friendshipTowardFarmer.Value / 1000.0 - (1.0 - (double)farmAnimal.happiness.Value / 225.0);
            if (!farmAnimal.isCoopDweller() && Game1.getFarmer(farmAnimal.ownerID.Value).professions.Contains(3) ||
                farmAnimal.isCoopDweller() && Game1.getFarmer(farmAnimal.ownerID.Value).professions.Contains(2))
                chance += 0.33;
            var produceQuality = chance < 0.95 || random.NextDouble() >= chance / 2.0
                ? (random.NextDouble() >= chance / 2.0 ? (random.NextDouble() >= chance ? 0 : 1) : 2)
                : 4;
            return produceQuality;
        }
    }
}
