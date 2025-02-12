﻿namespace Game2Test
{
    public class Player:IBarDraw
    {        
        private Logger _logger;
        public int X { get; private set; }
        public int Y { get; private set; }
        private int _health = 120;
        private int _maxHealth = 120;
        private int _damage = 20;
        private int _level = 1;
        private int _experience = 0;
        private int _experienceToNextLevel = 100;
        private bool _isDefending = false;
        public int Level => _level;

        public Player(int startPositionX, int startPositionY, Logger logger)
        {
            X = startPositionX;
            Y = startPositionY;
            _logger = logger;
        }
        public bool IsAlive()
        {
            return _health > 0;
        }
        public int CalculateDamage()
        {
            Random rand = new Random();
            bool isHit = rand.Next(100) > 10;
            if (!isHit) return 0;

            int critChance = rand.Next(100);
            if (critChance < 10)
            {
                int criticalDamage = _damage * 2;
                _logger.AddLog($"Критический удар! Нанесено урона: {criticalDamage}");
                return criticalDamage;
            }

            _logger.AddLog($"Нанесено урона: {_damage}");
            return _damage;
        }

        public void Defend()
        {
            _logger.AddLog("Игрок защищается.");
            _isDefending = true;
        }

        public void Heal()
        {
            int heal = 20;
            RestoreHealth(heal);
            _logger.AddLog($"Игрок исцелен на {heal}. Текущее здоровье: {_health}");
        }
        public void TakeDamage(int damage)
        {
            if (_isDefending)
            {
                 damage /= 2;
                _health -= damage;
                _logger.AddLog($"Игрок защищен. Полученный урон сокращен на 50%");
                _logger.AddLog($"Игрок получил урон: {damage}. Текущее здоровье: {_health}");
                _isDefending = false;
            }
            else
            {
                _health -= damage;
                _logger.AddLog($"Игрок получил урон: {damage}. Текущее здоровье: {_health}");
            }           

            if (_health <= 0)
            {
                _health = 0;
                Die();
            }
        }

        private void Die()
        {
            _logger.AddLog("Игрок погиб!");
        }



        public void SetDamage(int damage)
        {
            _damage = damage;
        }

        public void RestoreHealth(int health)
        {
            _health += health;
            if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }
        }

        public void IncreaseMaxHealth(int health)
        {
            _maxHealth += health;
            _health = _maxHealth;
            _logger.AddLog($"Максимальное здоровье увеличено на: {health}.");
        }
        public void AddExperience(int exp)
        {
            _experience += exp;
            _logger.AddLog($"Получено {exp} опыта. Текущий опыт: {_experience}/{_experienceToNextLevel}");
            if (_experience >= _experienceToNextLevel)
            {
                LevelUp();
            }
        }

        public void LevelUp()
        {
            _level++;
            _experience -= _experienceToNextLevel;
            _experienceToNextLevel += 50;
            IncreaseMaxHealth(50);
            SetDamage(_damage + 10);
            _logger.AddLog("Поздравляем! Уровень повышен!");
        }

        public void GiveHealthForBars(ref int health, ref int MaxHealt)
        {
            health = _health;
            MaxHealt = _maxHealth;
        }
        public void UpdatePlayerPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void DrawPlayer()
        {
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write('@');
        }
    }
}
