using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class PlayerQuest
    {
        public Quest Details;
        public bool IsCompleted;

        public PlayerQuest(Quest details, bool isCompleted = false)
        {
            Details = details;
            IsCompleted = isCompleted;
        }
    }
}
