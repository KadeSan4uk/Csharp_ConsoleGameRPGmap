using Game2Test.PlayerActions;
using Game2Test.InputPlayerStrategies;

namespace Game2Test
{
    public class Game
    {
        private Map _map;
        private Player _player;
        private Logger _logger;
        private Enemy? _enemy;
        private InputPLayer _inputHandler;
        private int nextBarPositionY = 14;
        public Game(string path)
        {
            _map = new Map(path);

            _logger = new Logger((message, line) =>
            {
                Console.SetCursorPosition(_map.MapData.GetLength(0) + 2, line);
                Console.WriteLine(message.PadRight(60, ' '));
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
                DrawPlayerHealthBar(_player);                              

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
                    _map.DrawMap();
                    _player.DrawPlayer();
                    _enemy = new Enemy(_player.Level, _logger);

                    if (_enemy is not null)
                    {
                        StartFight(_player, _enemy);
                        if (_enemy is null)
                            _map.RemoveMonsterAt(newX, newY);
                    }                    
                }               
            }
        }

        public void StartFight(Player player, Enemy enemy)
        {
            _logger?.AddLog("Битва началась!");
            while (enemy.IsAlive() && enemy is not null)
            {
                DrawPlayerHealthBar(_player);
                if (_enemy is not null)                
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
                            _enemy=null;
                            ClearEnemyHealthBar();
                            _logger?.AddLog("Враг повержен!");
                            _player.AddExperience(30);
                            DrawPlayerHealthBar(_player);
                            if (_enemy is not null)                            
                                DrawEnemyHealthBar(_enemy);
                            
                            return;
                        }

                        int enemyDamage = enemy.CalculateDamage();
                        player.TakeDamage(enemyDamage);
                        if (!player.IsAlive())
                        {
                            _logger?.AddLog("Игрок погиб!");
                            DrawPlayerHealthBar(_player);
                            if (_enemy is not null)                            
                                DrawEnemyHealthBar(enemy);
                            
                            return;
                        }

                        validInput = true; 
                    }
                    else
                    {
                        int menuPositionY = nextBarPositionY;
                        Console.SetCursorPosition(0, menuPositionY + 5);
                        Console.WriteLine("Неверный ввод, попробуйте снова.");
                    }
                }               
            }
            
        }
        public void ClearEnemyHealthBar()
        {            
            int barStartY = nextBarPositionY + 3;
            Console.SetCursorPosition(0, barStartY - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, barStartY);
            Console.Write(new string(' ', Console.WindowWidth));
        }

        public void DrawActionChoices()
        {
                int menuPositionY = nextBarPositionY + 6;
                Console.SetCursorPosition(0, menuPositionY);
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1: Атака");
                Console.WriteLine("2: Защита");
                Console.WriteLine("3: Лечение");                
        }
        public void DrawPlayerHealthBar(Player player)
        {
            DrawBarsPlayer(player, ConsoleColor.Green);
        }

        public void DrawEnemyHealthBar(Enemy enemy)
        {
            DrawBarsEnemy(enemy, ConsoleColor.Red);
        }

        public void DrawBarsPlayer(IBarDraw barDraw, ConsoleColor barColor)
        {
            int MaxHealth = 0;
            int health = 0;
            barDraw.GiveHealthForBars(ref  health, ref MaxHealth);
            if (health <= 0)
                return;

            int PartSize = 12;
            int BarSize = MaxHealth / PartSize;
            int HealthSize = health / BarSize;

            string HealthStatus = $"{health}/{MaxHealth}";

            int barStartX = 0;
            int barStartY = nextBarPositionY;
            Console.SetCursorPosition(barStartX, barStartY - 1);
            Console.Write(new string(' ', Console.WindowWidth));

            Console.SetCursorPosition(barStartX+3, barStartY - 1);
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

        }
        public void DrawBarsEnemy(IBarDraw barDraw, ConsoleColor barColor)
        {
            int MaxHealth = 0;
            int health = 0;
            barDraw.GiveHealthForBars(ref health, ref MaxHealth);
            if (health <= 0)
                return;

            int PartSize = 12;
            int BarSize = MaxHealth / PartSize;
            int HealthSize = health / BarSize;

            string HealthStatus = $"{health}/{MaxHealth}";

            int barStartX = 0;
            int barStartY = nextBarPositionY+3;
            
            Console.SetCursorPosition(barStartX, barStartY - 1);
            Console.Write(new string(' ', Console.WindowWidth));

            Console.SetCursorPosition(barStartX + 3, barStartY - 1);
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

        }
    }
}
