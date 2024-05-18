using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace Game2Test
{
    public class Enemy:IBarDraw
    {
        Logger? _logger;
        private int _health=100;
        private int _maxHealth=100;
        private int _damage=10;
        private int _level;
        public bool IsAlive()
        {
            return _health > 0;
        }

        public Enemy( int playerLevel,Logger logger)
        {
            _logger = logger;
            ScaleEnemy(playerLevel);
        }
        
        public int CalculateDamage()
        {
            Random rand = new Random();
            bool isHit = rand.Next(100) > 10;
            if (!isHit) return 0; 

            int critChance = rand.Next(100);
            if (critChance < 10) 
            {
                return _damage * 2; 
            }

            return _damage; 
        }
        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health > 0)
            {
                _logger.AddLog($"Монстр получил {damage} урона. У монстра осталось {_health} здоровья.");
            }
            else if (_health <= 0)
                return;
        }

        private void ScaleEnemy(int playerLevel)
        {
            _level = playerLevel;
            _health += (20 * playerLevel);
            _maxHealth +=(20 * playerLevel);
            _damage += (5 * playerLevel);
        }       

        public void GiveHealthForBars(ref int health, ref int MaxHealt)
        {
            health = _health;
            MaxHealt = _maxHealth;
        }
    }

}
