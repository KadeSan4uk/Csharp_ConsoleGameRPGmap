using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace Game2Test
{
    public class Enemy:IBarDraw
    {
        private int _health;
        private int _maxHealth;
        private int _damage;
        private int _level;
        public bool IsAlive()
        {
            return _health > 0;
        }

        public Enemy( int playerLevel)
        {
            ScaleEnemy(playerLevel);
        }

        public int CalculateDamage()
        {
            Random rand = new Random();
            bool isHit = rand.Next(100) > 20;
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
            if (_health <= 0)
            {
                Console.WriteLine("Enemy is defeated!");
            }
        }      

        private void ScaleEnemy(int playerLevel)
        {
            _level = playerLevel;
            _health = 100 + (20 * playerLevel);
            _maxHealth = _health;
            _damage = 10 + (5 * playerLevel);
        }
              
        public void GiveHealthForBars(ref int health, ref int MaxHealt)
        {
            health = _health;
            MaxHealt = _maxHealth;
        }
    }

}
