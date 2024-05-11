using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game2Test.PlayerActions;
using System.Collections.Generic;

namespace Game2Test.InputPlayerStrategies
{
    public class InputPLayer
    {
        public (int,int) HandleInputArrow(ConsoleKeyInfo pressedKey,  int playerX,  int playerY, char[,] map)
        {
            int[] direction = GetDirection(pressedKey);

            int nextPlayerPositionX = playerX + direction[0];
            int nextPlayerPositionY = playerY + direction[1];
            char nextCell = map[nextPlayerPositionX, nextPlayerPositionY];

            if (nextCell == ' ' || nextCell == 'm')
            {
                if (nextCell == 'm')
                {
                    map[nextPlayerPositionX, nextPlayerPositionY] = ' ';
                }
                return (nextPlayerPositionX, nextPlayerPositionY);
            }

            return (playerX, playerY);
        }

        private static int[] GetDirection(ConsoleKeyInfo pressedKey)
        {
            int[] direction = { 0, 0 };

            if (pressedKey.Key == ConsoleKey.UpArrow)
                direction[1] -= 1;
            else if (pressedKey.Key == ConsoleKey.DownArrow)
                direction[1] += 1;
            else if (pressedKey.Key == ConsoleKey.LeftArrow)
                direction[0] -= 1;
            else if (pressedKey.Key == ConsoleKey.RightArrow)
                direction[0] += 1;
            return direction;
        }

        public ICombatAction HandleInputNumbers(ConsoleKeyInfo pressedKey)
        {
            switch (pressedKey.Key)
            {
                case ConsoleKey.NumPad1:
                    return new AttackAction();
                case ConsoleKey.NumPad2:
                    return new DefendAction();
                case ConsoleKey.NumPad3:
                    return new HealAction();
                default:
                    return null;
            }
        }
    }
    
}
