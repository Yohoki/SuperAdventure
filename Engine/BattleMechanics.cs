using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    partial class Player
    {
        private void BeginBattle(Location newLocation)
        {
            RaiseMessage("You see a " + newLocation.MonsterLivingHere.Name);

            // Make a new monster, using the values from the standard monster in the World.Monster list
            Monster standardMonster = World.MonsterByID(newLocation.MonsterLivingHere.ID);

            _currentMonster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage,
                standardMonster.ExperiencePoints, standardMonster.RewardGold, standardMonster.CurrentHitPoints, standardMonster.MaximumHitPoints);
            _currentMonster.LootTable.AddRange(standardMonster.LootTable);
        }
        public void UseWeapon(Weapon weapon)
        {
            // Determine the amount of damage to do to the monster
            int damageToMonster = RandomNumberGenerator.NumberBetween(weapon.MinimumDamage, weapon.MaximumDamage);

            // Apply the damage to the monster's CurrentHitPoints
            _currentMonster.CurrentHitPoints -= damageToMonster;

            // Display message
            RaiseMessage("You hit the " + _currentMonster.Name + " for " + damageToMonster + " points.");
            MonsterAttackTurn();
        }
        public void UsePotion(HealingPotion potion)
        {
            // Add healing amount to the player's current hit points
            CurrentHitPoints += potion.AmountToHeal;

            // CurrentHitPoints cannot exceed player's MaximumHitPoints
            if (CurrentHitPoints > MaximumHitPoints)
            {
                CurrentHitPoints = MaximumHitPoints;
            }

            // Remove the potion from the player's inventory
            RemoveItemFromInventory(potion, 1);

            // Display message
            RaiseMessage("You drink a " + potion.Name);

            // Monster gets their turn to attack
            MonsterAttackTurn();
        }

        private void MonsterAttackTurn()
        {
            if (_currentMonster.CurrentHitPoints <= 0)
            {
                // Monster is dead
                MonsterIsDead();
                return;
            }
            // Determine the amount of damage the monster does to the player
            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);

            // Display message
            RaiseMessage("The " + _currentMonster.Name + " did " + damageToPlayer + " points of damage.");

            // Subtract damage from player
            CurrentHitPoints -= damageToPlayer;

            if (CurrentHitPoints <= 0)
            {
                // Display message
                RaiseMessage("The " + _currentMonster.Name + " killed you.");

                // Move player to "Home"
                MoveHome();
            }
        }

        private void MonsterIsDead()
        {
            RaiseMessage("");
            RaiseMessage("You defeated the " + _currentMonster.Name);

            // Give player experience points for killing the monster
            AddExperiencePoints(_currentMonster.ExperiencePoints);
            RaiseMessage("You receive " + _currentMonster.ExperiencePoints + " experience points");

            // Give player gold for killing the monster 
            Gold += _currentMonster.RewardGold;
            RaiseMessage("You receive " + _currentMonster.RewardGold + " gold");

            // Get random loot items from the monster and
            // add items to the lootedItems list, comparing a random number to the drop percentage
            List<InventoryItem> lootedItems = (_currentMonster.LootTable
                                        .Where( lootItem => RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage)
                                        .Select(lootItem => new InventoryItem(lootItem.Details, 1))).ToList();

            // If no items were randomly selected, then add the default loot item(s).
            if (lootedItems.Count == 0)
            {
                lootedItems.AddRange(from LootItem lootItem in _currentMonster.LootTable
                                     where lootItem.IsDefaultItem
                                     select new InventoryItem(lootItem.Details, 1));
            }

            // Add the looted items to the player's inventory
            foreach (InventoryItem inventoryItem in lootedItems)
            {
                AddItemToInventory(inventoryItem.Details);

                if (inventoryItem.Quantity == 1)
                {
                    RaiseMessage("You loot " + inventoryItem.Quantity + " " + inventoryItem.Details.Name);
                }
                else
                {
                    RaiseMessage("You loot " + inventoryItem.Quantity + " " + inventoryItem.Details.NamePlural);
                }
            }

            // Add a blank line to the messages box, just for appearance.
            RaiseMessage("");

            // Move player to current location (to heal player and create a new monster to fight)
            MoveTo(CurrentLocation);
        }
    }
}
