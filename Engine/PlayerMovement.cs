using System.Linq;

namespace Engine
{
    partial class Player
    {
        public void MoveTo(Location newLocation)
        {
            //Does the location have any required items
            if (!HasRequiredItemToEnterThisLocation(newLocation))
            {
                RaiseMessage("You must have a " + newLocation.ItemRequiredToEnter.Name + " to enter this location.");
                return;
            }

            // Update the player's current location
            CurrentLocation = newLocation;

            // Heal the player
            HealPlayerOnMovement();

            // Does the location have a quest?
            if (newLocation.QuestAvailableHere != null)
            {
                // See if the player already has the quest
                CheckQuestAvailableHere(newLocation);
            }

            // Does the location have a monster?
            if (newLocation.HasAMonster)
            {
                BeginBattle(newLocation);
                return;
            }
            CurrentMonster = null;
        }

        private void HealPlayerOnMovement()
        {
            CurrentHitPoints = MaximumHitPoints;
        }

        private void MoveHome()
        {
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
        }

        public void MoveNorth()
        {
            if (CurrentLocation.LocationToNorth != null)
            {
                MoveTo(CurrentLocation.LocationToNorth);
            }
        }

        public void MoveEast()
        {
            if (CurrentLocation.LocationToEast != null)
            {
                MoveTo(CurrentLocation.LocationToEast);
            }
        }

        public void MoveSouth()
        {
            if (CurrentLocation.LocationToSouth != null)
            {
                MoveTo(CurrentLocation.LocationToSouth);
            }
        }

        public void MoveWest()
        {
            if (CurrentLocation.LocationToWest != null)
            {
                MoveTo(CurrentLocation.LocationToWest);
            }
        }
        private void CheckQuestAvailableHere(Location newLocation)
        {
            // See if the player already has the quest, and if they've completed it
            bool playerAlreadyHasQuest = HasThisQuest(newLocation.QuestAvailableHere);
            bool playerAlreadyCompletedQuest = CompletedThisQuest(newLocation.QuestAvailableHere);

            if (playerAlreadyHasQuest)
            {
                CheckQuestCompleted(newLocation, playerAlreadyCompletedQuest);
                return;
            }
            // The player does not already have the quest
            GiveQuestToPlayer(newLocation);
        }

        private void CheckQuestCompleted(Location newLocation, bool playerAlreadyCompletedQuest)
        {
            // If the player has not completed the quest yet
            if (!playerAlreadyCompletedQuest)
            {
                // See if the player has all the items needed to complete the quest
                bool playerHasAllItemsToCompleteQuest = HasAllQuestCompletionItems(newLocation.QuestAvailableHere);

                // The player has all items required to complete the quest
                if (playerHasAllItemsToCompleteQuest)
                {
                    CompletePlayerQuest(newLocation);
                }
            }
        }

        private void CompletePlayerQuest(Location newLocation)
        {
            // Display message
            RaiseMessage("");
            RaiseMessage("You complete the '" + newLocation.QuestAvailableHere.Name + "' quest.");

            // Remove quest items from inventory
            RemoveQuestCompletionItems(newLocation.QuestAvailableHere);

            // Give quest rewards
            RaiseMessage("You receive: ");
            RaiseMessage(newLocation.QuestAvailableHere.RewardExperiencePoints + " experience points");
            RaiseMessage(newLocation.QuestAvailableHere.RewardGold + " gold");
            RaiseMessage(newLocation.QuestAvailableHere.RewardItem.Name, true);

            AddExperiencePoints(newLocation.QuestAvailableHere.RewardExperiencePoints);
            Gold += newLocation.QuestAvailableHere.RewardGold;

            // Add the reward item to the player's inventory
            AddItemToInventory(newLocation.QuestAvailableHere.RewardItem);

            // Mark the quest as completed
            MarkQuestCompleted(newLocation.QuestAvailableHere);
        }

        private void GiveQuestToPlayer(Location newLocation)
        {

            // Display the messages
            RaiseMessage("You receive the " + newLocation.QuestAvailableHere.Name + " quest.");
            RaiseMessage(newLocation.QuestAvailableHere.Description);
            RaiseMessage("To complete it, return with:");
            foreach (QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
            {
                if (qci.Quantity == 1)
                {
                    RaiseMessage(qci.Quantity + " " + qci.Details.Name);
                }
                else
                {
                    RaiseMessage(qci.Quantity + " " + qci.Details.NamePlural);
                }
            }
            RaiseMessage("");

            // Add the quest to the player's quest list
            Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
        }
        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if (location.ItemRequiredToEnter == null)
            {
                // There is no required item for this location, so return "true"
                return true;
            }

            // See if the player has the required item in their inventory
            return Inventory.Any(ii => ii.Details.ID == location.ItemRequiredToEnter.ID);
        }
        public bool HasThisQuest(Quest quest)
        {
            return Quests.Any(pq => pq.Details.ID == quest.ID);
        }

        public bool CompletedThisQuest(Quest quest)
        {
            foreach (PlayerQuest playerQuest in Quests)
            {
                if (playerQuest.Details.ID == quest.ID)
                {
                    return playerQuest.IsCompleted;
                }
            }

            return false;
        }

        public bool HasAllQuestCompletionItems(Quest quest)
        {
            // See if the player has all the items needed to complete the quest here
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                // Check each item in the player's inventory, to see if they have it, and enough of it
                if (!Inventory.Any(ii => ii.Details.ID == qci.Details.ID && ii.Quantity >= qci.Quantity))
                {
                    return false;
                }
            }
            // If we got here, then the player must have all the required items, and enough of them, to complete the quest.
            return true;
        }

        public void RemoveQuestCompletionItems(Quest quest)
        {
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                // Subtract the quantity from the player's inventory that was needed to complete the quest
                InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == qci.Details.ID);

                if (item != null)
                {
                    RemoveItemFromInventory(item.Details, qci.Quantity);
                }
            }
        }

        public void MarkQuestCompleted(Quest quest)
        {
            // Find the quest in the player's quest list
            PlayerQuest playerQuest = Quests.SingleOrDefault(pq => pq.Details.ID == quest.ID);

            if (playerQuest != null)
            {
                playerQuest.IsCompleted = true;
            }
        }
    }
}
