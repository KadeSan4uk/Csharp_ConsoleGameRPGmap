using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Game2Test.PlayerActions
{
    public class AttackAction : ICombatAction
    {
        public void ExecuteAction(Player player, Enemy enemy)
        {
            int damage = player.CalculateDamage();
            enemy.TakeDamage(damage);
        }
    }
}
