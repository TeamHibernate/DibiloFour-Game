namespace DibiloFour.Core.Data
{
    using System.Data.Entity;
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
            Dibil OwenShopKeeper = new Villain(1, "Owen", 100, 1);
            Dibil NaskoTheBandit = new Villain(2, "Nasko", 100, 3);
            Dibil KermitTheFarmer = new Villain(3, "Kermit", 100, 4);

            context.Dibils.Add(OwenShopKeeper);
            context.Dibils.Add(NaskoTheBandit);
            context.Dibils.Add(KermitTheFarmer);

            // Initialize ItemShops
            ItemShop OwenShop = new ItemShop(1, "Owen Shop", 1000M, 1, 1, 2);

            context.ItemShops.Add(OwenShop);

            // Initialize Chests
            Chest chestOneInBanditCave = new Chest(1, 1, 3, 5);
            Chest chestTwoInBanditCave = new Chest(2, 2, 3, 6);

            context.Chests.Add(chestOneInBanditCave);
            context.Chests.Add(chestTwoInBanditCave);

            // Initialize Items
            Item ironSwordInShop = new Weapon(1, "Iron Sword", "Sword made of iron.", Material.Iron, 10, 10, 2);
            Item ironArmourInShop = new Apprael(5, "Iron Armour", "Armour made of iron.", Material.Iron, 10, 10, 2);
            Item ironSwordInBandit = new Weapon(2, "Iron Sword", "Sword made of iron. Little used.", Material.Iron, 9, 10, 3);
            Item woodenSwordInKermit = new Weapon(3, "Wooden Sword", "Sword made of wood", Material.Iron, 2, 5, 4);

            Item bookOfMajorLockpickingInChestOne = new Book(4, "Book Of Lockpicking", "Book of major lockpicking", 4, 5)
            {
                BonusLockpickingSkills = 10,
                BonusSpeechSkills = 0
            };
            Item potionOfMajorHealthInChestOne = new Potion(5, "Health Potion", "Potion of major health", 3, 100, 10);
            Item steelSwordInChestTwo = new Weapon(4, "Steel Sword", "Sword made of steel", Material.Steel, 20, 30, 6);

            context.Items.Add(ironSwordInShop);
            context.Items.Add(ironArmourInShop);
            context.Items.Add(ironSwordInBandit);
            context.Items.Add(woodenSwordInKermit);
            context.Items.Add(bookOfMajorLockpickingInChestOne);
            context.Items.Add(potionOfMajorHealthInChestOne);
            context.Items.Add(steelSwordInChestTwo);

            context.SaveChanges();

            // Adding swords/armour to npc Dibils
            context.Dibils.FirstOrDefault(d => d.Id == 2).CurrentWeaponItemId = 2;
            context.Dibils.FirstOrDefault(d => d.Id == 3).CurrentWeaponItemId = 3;

            context.SaveChanges();

            base.Seed(context);
        }
    }
}