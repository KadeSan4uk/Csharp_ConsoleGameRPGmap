using System;
using System.IO;
using System.Collections.Generic;

namespace Game2Test
{
    public class Map
    {
        private char[,] _mapData;
        private List<Tuple<int, int>> _monsterPositions;

        public Map(string path)
        {
            _monsterPositions = new List<Tuple<int, int>>();
            _mapData = ReadMap(path);
        }

        public char[,] MapData => _mapData;
        public List<Tuple<int, int>> MonsterPositions => _monsterPositions;
        public void DrawMap()
        {
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < _mapData.GetLength(1); y++)
            {
                for (int x = 0; x < _mapData.GetLength(0); x++)
                {
                    Console.ForegroundColor = _mapData[x, y] switch
                    {
                        '#' => ConsoleColor.DarkBlue,
                        'm' => ConsoleColor.Red,
                        _ => ConsoleColor.Gray
                    };
                    Console.Write(_mapData[x, y]);
                }
                Console.WriteLine();
            }
        }
        public bool IsMonsterAt(int x, int y)
        {
            return _monsterPositions.Contains(new Tuple<int, int>(x, y));
        }

        private static bool HasNeighbor(char[,] map, int x, int y)
        {
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int neighborX = x + i;
                    int neighborY = y + j;

                    if (neighborX >= 0 && neighborX < rows && neighborY >= 0 && neighborY < cols)
                    {
                        if (map[neighborX, neighborY] == 'm' || map[neighborX, neighborY] == '@')
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private char[,] ReadMap(string path)
        {
            List<string> lines = new List<string>();

            using (StreamReader reader = new StreamReader(path))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            int maxWidth = GetMaxLengthOfLine(lines.ToArray());
            char[,] map = new char[maxWidth, lines.Count];
            Random random = new Random();

            for (int y = 0; y < lines.Count; y++)
            {
                string currentLine = lines[y];
                for (int x = 0; x < currentLine.Length; x++)
                {
                    char currentChar = currentLine[x];
                    if (currentChar == '#' || (!HasNeighbor(map, x, y) && random.Next(10) < 2))
                    {
                        map[x, y] = currentChar == '#' ? '#' : 'm';
                        if (map[x, y] == 'm')
                        {
                            _monsterPositions.Add(new Tuple<int, int>(x, y));
                        }
                    }
                    else
                    {
                        map[x, y] = ' ';
                    }
                }
                for (int x = currentLine.Length; x < maxWidth; x++)
                {
                    map[x, y] = ' ';
                }
            }

                return map;
        }
        public void RemoveMonsterAt(int x, int y)
        {
            var monsterPosition = new Tuple<int, int>(x, y);
            _monsterPositions.Remove(monsterPosition);
            _mapData[x, y] = ' ';
        }

        private static int GetMaxLengthOfLine(string[] lines)
        {
            int maxLength = 0;
            foreach (string line in lines)
            {
                maxLength = Math.Max(maxLength, line.Length);
            }
            return maxLength;
        }
    }
}
