using System.Collections.Generic;
using System.Linq;

namespace Engine
{
    public class Monster : LivingCreature
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int MaximumDamage { get; set; }
        public List<LootItem> LootTable { get; set; }

        public Monster(int id, string name, int maximumDamage, int experiencePoints,
                       int gold, int currentHitPoints, int maximumHitPoints)
                : base(currentHitPoints, maximumHitPoints, experiencePoints, gold)
        {
            ID = id;
            Name = name;
            MaximumDamage = maximumDamage;
            Gold = gold;

            LootTable = new List<LootItem>();
        }
        internal Monster NewInstanceOfMonster()
        {
            Monster newMonster =
                new Monster(ID, Name, MaximumDamage, ExperiencePoints, Gold, CurrentHitPoints, MaximumHitPoints);

            // Add items to the lootedItems list, comparing a random number to the drop percentage
            foreach (LootItem lootItem in LootTable.Where(lootItem => RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage))
            {
                newMonster.LootTable.Add(new LootItem(lootItem.Details, 1, false));
            }

            // If no items were randomly selected, add the default loot item(s).
            if (newMonster.LootTable.Count == 0)
            {
                foreach (LootItem lootItem in LootTable.Where(x => x.IsDefaultItem))
                {
                    newMonster.LootTable.Add(new LootItem(lootItem.Details, 1, true));
                }
            }

            return newMonster;
        }
    }
}
