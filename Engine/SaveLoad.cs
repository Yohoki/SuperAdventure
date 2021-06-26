using System;
using System.Linq;
using System.Xml;


namespace Engine
{
    public partial class Player
    {

        public static Player CreateDefaultPlayer()
        {
            Player player = new(10, 10, 20, 0);
            player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));
            player.CurrentLocation = World.LocationByID(World.LOCATION_ID_HOME);

            return player;
        }

        public static Player CreatePlayerFromXmlString(string xmlPlayerData)
        {
            try
            {
                XmlDocument playerData = new();

                playerData.LoadXml(xmlPlayerData);
                Player player = LoadStats(playerData);

                LoadWeapons(playerData, player);
                LoadInventory(playerData, player);
                LoadQuests(playerData, player);

                return player;
            }
            catch
            {
                // If there was an error with the XML data, return a default player object
                return Player.CreateDefaultPlayer();
            }
        }

        public string ToXmlString()
        {
            XmlDocument playerData = new();

            // Create the top-level XML node
            XmlNode player = playerData.CreateElement("Player");
            playerData.AppendChild(player);

            // Create the "Stats" child node to hold the other player statistics nodes
            XmlNode stats = playerData.CreateElement("Stats");
            player.AppendChild(stats);

            // Create the child nodes for the "Stats" node
            SaveStats(playerData, stats);

            SaveWeapon(playerData, stats);

            // Create the "InventoryItems" child node to hold each InventoryItem node
            XmlNode inventoryItems = playerData.CreateElement("InventoryItems");
            player.AppendChild(inventoryItems);

            // Create an "InventoryItem" node for each item in the player's inventory
            SaveInventory(playerData, inventoryItems);

            // Create the "PlayerQuests" child node to hold each PlayerQuest node
            XmlNode playerQuests = playerData.CreateElement("PlayerQuests");
            player.AppendChild(playerQuests);

            // Create a "PlayerQuest" node for each quest the player has acquired
            SaveQuests(playerData, playerQuests);

            return playerData.InnerXml; // The XML document, as a string, so we can save the data to disk
        }

        private void SaveStats(XmlDocument playerData, XmlNode stats)
        {
            XmlNode currentHitPoints = playerData.CreateElement("CurrentHitPoints");
            currentHitPoints.AppendChild(playerData.CreateTextNode(this.CurrentHitPoints.ToString()));
            stats.AppendChild(currentHitPoints);

            XmlNode maximumHitPoints = playerData.CreateElement("MaximumHitPoints");
            maximumHitPoints.AppendChild(playerData.CreateTextNode(this.MaximumHitPoints.ToString()));
            stats.AppendChild(maximumHitPoints);

            XmlNode gold = playerData.CreateElement("Gold");
            gold.AppendChild(playerData.CreateTextNode(this.Gold.ToString()));
            stats.AppendChild(gold);

            XmlNode experiencePoints = playerData.CreateElement("ExperiencePoints");
            experiencePoints.AppendChild(playerData.CreateTextNode(this.ExperiencePoints.ToString()));
            stats.AppendChild(experiencePoints);

            XmlNode currentLocation = playerData.CreateElement("CurrentLocation");
            currentLocation.AppendChild(playerData.CreateTextNode(this.CurrentLocation.ID.ToString()));
            stats.AppendChild(currentLocation);
        }

        private void SaveWeapon(XmlDocument playerData, XmlNode stats)
        {
            if (CurrentWeapon != null)
            {
                XmlNode currentWeapon = playerData.CreateElement("CurrentWeapon");
                currentWeapon.AppendChild(playerData.CreateTextNode(this.CurrentWeapon.ID.ToString()));
                stats.AppendChild(currentWeapon);
            }
        }

        private void SaveInventory(XmlDocument playerData, XmlNode inventoryItems)
        {
            foreach (var (item, inventoryItem, idAttribute) in from InventoryItem item in this.Inventory
                                                               let inventoryItem = playerData.CreateElement("InventoryItem")
                                                               let idAttribute = playerData.CreateAttribute("ID")
                                                               select (item, inventoryItem, idAttribute))
            {
                idAttribute.Value = item.Details.ID.ToString();
                inventoryItem.Attributes.Append(idAttribute);
                XmlAttribute quantityAttribute = playerData.CreateAttribute("Quantity");
                quantityAttribute.Value = item.Quantity.ToString();
                inventoryItem.Attributes.Append(quantityAttribute);
                inventoryItems.AppendChild(inventoryItem);
            }
        }

        private void SaveQuests(XmlDocument playerData, XmlNode playerQuests)
        {
            foreach (var (quest, playerQuest, idAttribute) in from PlayerQuest quest in this.Quests
                                                              let playerQuest = playerData.CreateElement("PlayerQuest")
                                                              let idAttribute = playerData.CreateAttribute("ID")
                                                              select (quest, playerQuest, idAttribute))
            {
                idAttribute.Value = quest.Details.ID.ToString();
                playerQuest.Attributes.Append(idAttribute);
                XmlAttribute isCompletedAttribute = playerData.CreateAttribute("IsCompleted");
                isCompletedAttribute.Value = quest.IsCompleted.ToString();
                playerQuest.Attributes.Append(isCompletedAttribute);
                playerQuests.AppendChild(playerQuest);
            }
        }

        private static void LoadWeapons(XmlDocument playerData, Player player)
        {
            if (playerData.SelectSingleNode("/Player/Stats/CurrentWeapon") != null)
            {
                int currentWeaponID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentWeapon").InnerText);
                player.CurrentWeapon = (Weapon)World.ItemByID(currentWeaponID);
            }
        }
        private static void LoadQuests(XmlDocument playerData, Player player)
        {
            foreach (var (isCompleted, playerQuest) in from XmlNode node in playerData.SelectNodes("/Player/PlayerQuests/PlayerQuest")
                                                       let id = Convert.ToInt32(node.Attributes["ID"].Value)
                                                       let isCompleted = Convert.ToBoolean(node.Attributes["IsCompleted"].Value)
                                                       let playerQuest = new PlayerQuest(World.QuestByID(id))
                                                       select (isCompleted, playerQuest))
            {
                playerQuest.IsCompleted = isCompleted;
                player.Quests.Add(playerQuest);
            }
        }
        private static void LoadInventory(XmlDocument playerData, Player player)
        {
            foreach (var (id, quantity) in from XmlNode node in playerData.SelectNodes("/Player/InventoryItems/InventoryItem")
                                           let id = Convert.ToInt32(node.Attributes["ID"].Value)
                                           let quantity = Convert.ToInt32(node.Attributes["Quantity"].Value)
                                           select (id, quantity))
            {
                for (int i = 0; i < quantity; i++)
                {
                    player.AddItemToInventory(World.ItemByID(id));
                }
            }
        }
        private static Player LoadStats(XmlDocument playerData)
        {
            int currentHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentHitPoints").InnerText);
            int maximumHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/MaximumHitPoints").InnerText);
            int gold = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Gold").InnerText);
            int ExperiencePoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/ExperiencePoints").InnerText);

            Player player = new(currentHitPoints, maximumHitPoints, gold, ExperiencePoints);

            int currentLocationID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentLocation").InnerText);
            player.CurrentLocation = World.LocationByID(currentLocationID);
            return player;
        }
    }
}
