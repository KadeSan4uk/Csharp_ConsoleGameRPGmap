using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Game2Test.PlayerActions
{
    public class DefendAction : ICombatAction
    {
        public void ExecutePlayerAction(Player player, Enemy enemy)
        {
            player.Defend();
        }
    }
}
