using Game2Test.PlayerActions;
using Game2Test.InputPlayerStrategies;

namespace Game2Test
{
    public class Game
    {
        private Map _map;
        private Player _player;
        private Logger? _logger;
        private Enemy? _enemy;
        private InputPLayer _inputHandler;
        private int nextBarPositionY = 14;
        public Game(string path)
        {
            _map = new Map(path);

            _logger = new Logger((message, line) =>
            {
                Console.SetCursorPosition(_map.MapData.GetLength(0) + 2, line);
                Console.WriteLine(message.PadRight(30, ' '));
            });

            _inputHandler = new InputPLayer();
            _player = new Player(1, 9,_logger);
            Console.CursorVisible = false;            
        }

        public void Run()
        {
            while (true)
            {
                nextBarPositionY = 14;
                _map.DrawMap();
                _player.DrawPlayer();
                DrawPlayerHealthBar(); 

                int newX = _player.PlayerPositionX;
                int newY = _player.PlayerPositionY;

                Console.SetCursorPosition(0, _map.MapData.GetLength(1));
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write($"Игрок на ({newX}, {newY}) координатах");

                _logger?.ShowLog();                

                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                (newX, newY) = _inputHandler.HandleInputArrow(pressedKey, newX, newY, _map.MapData);
                _player.UpdatePlayerPosition(newX, newY);

                if (_map.IsMonsterAt(newX, newY))
                {
                    _enemy = new Enemy(_player.Level);
                    _map.DrawMap();
                    _player.DrawPlayer();
                    DrawEnemyHealthBar(_enemy); 
                    StartFight(_player, _enemy);
                    if (!_enemy.IsAlive())
                    {
                        _map.RemoveMonsterAt(newX, newY);
                    }
                }
               
            }

        }

        public void StartFight(Player player, Enemy enemy)
        {
            _logger?.AddLog("Битва началась!");
            while (enemy.IsAlive())
            {
                DrawPlayerHealthBar();
                DrawEnemyHealthBar(enemy);
                DrawActionChoices();

                bool validInput = false;
                while (!validInput)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                    ICombatAction action = _inputHandler.HandleInputNumbers(pressedKey);                   

                    if (action != null)
                    {
                        action.ExecuteAction(player, enemy);

                        if (!enemy.IsAlive())
                        {
                            _logger?.AddLog("Враг повержен!");
                            return;
                        }

                        int enemyDamage = enemy.CalculateDamage();
                        player.TakeDamage(enemyDamage);

                        if (!player.IsAlive())
                        {
                            _logger?.AddLog("Игрок погиб!");
                            return;
                        }

                        validInput = true; 
                    }
                    else
                    {
                        int menuPositionY = nextBarPositionY;
                        Console.SetCursorPosition(0, menuPositionY + 4);
                        Console.WriteLine("Неверный ввод, попробуйте снова.");
                    }
                }               
            }
        }


        public void DrawActionChoices()
        {
            int menuPositionY = nextBarPositionY;
            Console.SetCursorPosition(0, menuPositionY);
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1: Атака");
            Console.WriteLine("2: Защита");
            Console.WriteLine("3: Лечение");
        }
        public void DrawPlayerHealthBar()
        {
            DrawBars(_player, ConsoleColor.Green);
        }

        public void DrawEnemyHealthBar(Enemy enemy)
        {
            DrawBars(enemy, ConsoleColor.Red);
        }

        public void DrawBars(IBarDraw barDraw, ConsoleColor barColor)
        {
            int MaxHealth = 0;
            int health = 0;
            barDraw.GiveHealthForBars(ref health, ref MaxHealth);

            int PartSize = 10;
            int BarSize = MaxHealth / PartSize;
            //if (BarSize == 0) BarSize = 1;

            int HealthSize = health / BarSize;

            string HealthStatus = $"{health}/{MaxHealth}";

            int barStartX = 0;
            int barStartY = nextBarPositionY;

            if (nextBarPositionY >= 20)
            {
                return;
            }

            Console.SetCursorPosition(barStartX, barStartY - 1);
            Console.Write(HealthStatus);

            Console.SetCursorPosition(barStartX, barStartY);
            Console.Write('[');
            for (int i = 0; i < PartSize; i++)
            {
                Console.BackgroundColor = i < HealthSize ? barColor : ConsoleColor.DarkGray;
                Console.Write(" ");
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(']');
            Console.WriteLine();

            nextBarPositionY += 3;
        }


    }
}
