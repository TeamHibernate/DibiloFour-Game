namespace DibiloFour.Core.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using Models;
    using Models.Dibils;
    using Models.Enums;
    using Models.Items;

    public class DibiloFourDBInitializer : CreateDatabaseIfNotExists<DibiloFourContext>
    {
        protected override void Seed(DibiloFourContext context)
        {
            // Initialize types
            var city = new LocationType(1, "City");
            var village = new LocationType(2, "Village");
            var cave = new LocationType(3, "Cave");

            context.LocationTypes.Add(city);
            context.LocationTypes.Add(village);
            context.LocationTypes.Add(cave);

            var noLock = new LockType(1, "No Lock", 0);
            var simpleLock = new LockType(2, "Simple Lock", 5);
            var advancedLock = new LockType(3, "Advanced Lock", 10);

            context.LockTypes.Add(noLock);
            context.LockTypes.Add(simpleLock);
            context.LockTypes.Add(advancedLock);

            var sword = new ItemType(1, "Sword");
            var armour = new ItemType(2, "Armour");
            var healthPotion = new ItemType(3, "Health Potion");
            var lockpickSkillBook = new ItemType(4, "Lockpick Skill Book");

            context.ItemTypes.Add(sword);
            context.ItemTypes.Add(armour);
            context.ItemTypes.Add(healthPotion);
            context.ItemTypes.Add(lockpickSkillBook);

            // Initialize Locations
            var windhelmCity = new Location(1, "Windhelm", "Windy city of Windhelm is located near crystal clear river and has good farming land", 1);
            var helgenVillage = new Location(2, "Helgen", "Helgen is located in the skirts of Snowy Mountain. Home of the best blacksmiths.", 2);
            var banditCave = new Location(3, "Bandit Cave", "The entrance of the cave is facing Windhelm city. Bandits usually hide here.", 3);

            context.Locations.Add(windhelmCity);
            context.Locations.Add(helgenVillage);
            context.Locations.Add(banditCave);

            // Initialize Inventories
            var OwenShopKeeperInventory = new Inventory(1);
            var OwenShopInventory = new Inventory(2);
            var NaskoTheBanditInventory = new Inventory(3);
            var KermitTheFarmerInventory = new Inventory(4);
            var treasureChestOne = new Inventory(5);
            var treasureChestTwo = new Inventory(6);

            context.Inventories.Add(OwenShopKeeperInventory);
            context.Inventories.Add(OwenShopInventory);
            context.Inventories.Add(NaskoTheBanditInventory);
            context.Inventories.Add(KermitTheFarmerInventory);
            context.Inventories.Add(treasureChestOne);
            context.Inventories.Add(treasureChestTwo);

            context.SaveChanges();

            // проблем
            // Initialize Dibils
            var OwenShopKeeper = new Villain(1, "Owen", 100, 1, OwenShopKeeperInventory);
            var NaskoTheBandit = new Villain(2, "Nasko", 100, 3, NaskoTheBanditInventory);
            var KermitTheFarmer = new Villain(3, "Kermit", 100, 4, KermitTheFarmerInventory);

            context.Villains.Add(OwenShopKeeper);
            context.Villains.Add(NaskoTheBandit);
            context.Villains.Add(KermitTheFarmer);

            context.SaveChanges();

            // Initialize ItemShops
            var OwenShop = new ItemShop(1, "Owen Shop", 1000M, 1, 1, 2);

            context.ItemShops.Add(OwenShop);

            context.SaveChanges();

            // Initialize Chests
            var chestOneInBanditCave = new Chest(1, 1, 3, 5);
            var chestTwoInBanditCave = new Chest(2, 2, 3, 6);

            context.Chests.Add(chestOneInBanditCave);
            context.Chests.Add(chestTwoInBanditCave);

            context.SaveChanges();

            // Initialize Items
            var ironSwordInShop = new Weapon(1, "Iron Sword", "Sword made of iron.", Material.Iron, 10, 10, 2);
            var ironSwordInBandit = new Weapon(2, "Iron Sword", "Sword made of iron. Little used.", Material.Iron, 9, 10, 3);
            var woodenSwordInKermit = new Weapon(3, "Wooden Sword", "Sword made of wood", Material.Iron, 2, 5, 4);
            var steelSwordInChestTwo = new Weapon(4, "Steel Sword", "Sword made of steel", Material.Steel, 20, 30, 6);
            
            context.Weapons.Add(ironSwordInShop);
            context.Weapons.Add(ironSwordInBandit);
            context.Weapons.Add(woodenSwordInKermit);
            context.Weapons.Add(steelSwordInChestTwo);

            context.SaveChanges();

            var ironArmourInShop = new Apprael(1, "Iron Armour", "Armour made of iron.", Material.Iron, 10, 10, 2);
            
            context.Appraels.Add(ironArmourInShop);

            context.SaveChanges();

            var bookOfMajorLockpickingInChestOne = new Book(1, "Book Of Lockpicking", "Book of major lockpicking", 4, 5)
            {
                BonusLockpickingSkills = 10,
                BonusSpeechSkills = 0
            };

            context.Books.Add(bookOfMajorLockpickingInChestOne);

            context.SaveChanges();

            var potionOfMajorHealthInChestOne = new Potion(1, "Health Potion", "Potion of major health", 3, 100, 10);
            
            context.Potions.Add(potionOfMajorHealthInChestOne);

            context.SaveChanges();

            // Adding swords/armour to npc Dibils
            context.Villains.FirstOrDefault(d => d.Id == 2).CurrentWeaponItemId = 2;
            context.Villains.FirstOrDefault(d => d.Id == 3).CurrentWeaponItemId = 3;

            context.SaveChanges();

            base.Seed(context);
        }
    }
}