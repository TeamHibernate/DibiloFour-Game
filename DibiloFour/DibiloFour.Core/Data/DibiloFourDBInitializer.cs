﻿namespace DibiloFour.Core
{
    using Models;
    using System.Data.Entity;
    using System.Linq;

    public class DibiloFourDBInitializer : CreateDatabaseIfNotExists<DibiloFourContext>
    {
        protected override void Seed(DibiloFourContext context)
        {
            // Initialize types
            LocationType city = new LocationType(1, "City");
            LocationType village = new LocationType(2, "Village");
            LocationType cave = new LocationType(3, "Cave");

            context.LocationTypes.Add(city);
            context.LocationTypes.Add(village);
            context.LocationTypes.Add(cave);

            LockType noLock = new LockType(1, "No Lock", 0);
            LockType simpleLock = new LockType(2, "Simple Lock", 5);
            LockType advancedLock = new LockType(3, "Advanced Lock", 10);

            context.LockTypes.Add(noLock);
            context.LockTypes.Add(simpleLock);
            context.LockTypes.Add(advancedLock);

            ItemType sword = new ItemType(1, "Sword");
            ItemType armour = new ItemType(2, "Armour");
            ItemType healthPotion = new ItemType(3, "Health Potion");
            ItemType lockpickSkillBook = new ItemType(4, "Lockpick Skill Book");

            context.ItemTypes.Add(sword);
            context.ItemTypes.Add(armour);
            context.ItemTypes.Add(healthPotion);
            context.ItemTypes.Add(lockpickSkillBook);

            // Initialize Locations
            Location windhelmCity = new Location(1, "Windhelm", "Windy city of Windhelm is located near crystal clear river and has good farming land", 1);
            Location helgenVillage = new Location(2, "Helgen", "Helgen is located in the skirts of Snowy Mountain. Home of the best blacksmiths.", 2);
            Location banditCave = new Location(3, "Bandit Cave", "The entrance of the cave is facing Windhelm city. Bandits usually hide here.", 3);

            context.Locations.Add(windhelmCity);
            context.Locations.Add(helgenVillage);
            context.Locations.Add(banditCave);

            // Initialize Inventories
            Inventory OwenShopKeeperInventory = new Inventory(1);
            Inventory OwenShopInventory = new Inventory(2);
            Inventory NaskoTheBanditInventory = new Inventory(3);
            Inventory KermitTheFarmerInventory = new Inventory(4);
            Inventory treasureChestOne = new Inventory(5);
            Inventory treasureChestTwo = new Inventory(6);

            context.Inventories.Add(OwenShopKeeperInventory);
            context.Inventories.Add(OwenShopInventory);
            context.Inventories.Add(NaskoTheBanditInventory);
            context.Inventories.Add(KermitTheFarmerInventory);
            context.Inventories.Add(treasureChestOne);
            context.Inventories.Add(treasureChestTwo);

            // Initialize Dibils
            Dibil OwenShopKeeper = new Dibil(1, "Owen", 100, 0, 5, 1000, 1, false)
            {
                CurrentLocationId = 1
            };
            Dibil NaskoTheBandit = new Dibil(2, "Nasko", 100, 5, 5, 500, 3, false)
            {
                CurrentLocationId = 3
            };
            Dibil KermitTheFarmer = new Dibil(3, "Kermit", 100, 1, 1, 5, 4, false)
            {
                CurrentLocationId = 2
            };

            context.Dibils.Add(OwenShopKeeper);
            context.Dibils.Add(NaskoTheBandit);
            context.Dibils.Add(KermitTheFarmer);

            // Initialize ItemShops
            ItemShop OwenShop = new ItemShop(1, "Owen Shop", 1, 1, 2);

            context.ItemShops.Add(OwenShop);

            // Initialize Chests
            Chest chestOneInBanditCave = new Chest(1, 1, 3, 5);
            Chest chestTwoInBanditCave = new Chest(2, 2, 3, 6);

            context.Chests.Add(chestOneInBanditCave);
            context.Chests.Add(chestTwoInBanditCave);

            // Initialize Items
            Item ironSwordInShop = new Item(1, "Iron Sword", "Sword made of iron.", 1, 10, 10, 2);
            Item ironArmourInShop = new Item(5, "Iron Armour", "Armour made of iron.", 2, 10, 10, 2);
            Item ironSwordInBandit = new Item(2, "Iron Sword", "Sword made of iron. Little used.", 1, 9, 10, 3);
            Item woodenSwordInKermit = new Item(3, "Wooden Sword", "Sword made of wood", 1, 2, 5, 4);
            Item bookOfMajorLockpickingInChestOne = new Item(4, "Book Of Lockpicking", "Book of major lockpicking", 4, 5, 10, 5);
            Item potionOfMajorHealthInChestOne = new Item(5, "Health Potion", "Potion of major health", 3, 100, 15, 5);
            Item steelSwordInChestTwo = new Item(4, "Steel Sword", "Sword made of steel", 1, 20, 30, 6);

            context.Items.Add(ironSwordInShop);
            context.Items.Add(ironArmourInShop);
            context.Items.Add(ironSwordInBandit);
            context.Items.Add(woodenSwordInKermit);
            context.Items.Add(bookOfMajorLockpickingInChestOne);
            context.Items.Add(potionOfMajorHealthInChestOne);
            context.Items.Add(steelSwordInChestTwo);

            context.SaveChanges();

            // Adding swords/armour to npc Dibils
            context.Dibils.Where(d => d.Id == 2).FirstOrDefault().CurrentWeaponItemId = 2;
            context.Dibils.Where(d => d.Id == 3).FirstOrDefault().CurrentWeaponItemId = 3;

            context.SaveChanges();

            base.Seed(context);
        }
    }
}