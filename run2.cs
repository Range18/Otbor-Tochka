using System;
using System.Collections.Generic;
using System.Linq;


class Program
{
    // Константы для символов ключей и дверей
    static readonly char[] keys_char = Enumerable.Range('a', 26).Select(i => (char)i).ToArray();
    static readonly char[] doors_char = keys_char.Select(char.ToUpper).ToArray();

    private static (int dx, int dy)[] directions = { (0, 1), (1, 0), (0, -1), (-1, 0) };

    // Метод для чтения входных данных
    static List<List<char>> GetInput()
    {
        var data = new List<List<char>>();
        string line;
        while ((line = Console.ReadLine()) != null && line != "")
        {
            data.Add(line.ToCharArray().ToList());
        }

        return data;
    }


    static string EncodeState(List<(int, int)> positions, string keys)
    {
        var positionStr = string.Join(",", positions.Select(p => $"{p.Item1}:{p.Item2}"));
        var sortedKeys = string.Concat(keys.OrderBy(c => c));
        return $"{positionStr}|{sortedKeys}";
    }
    
    private static bool IsKey(char cell) => keys_char.Contains(cell);
    private static bool IsDoor(char cell) => doors_char.Contains(cell);

    static int Solve(List<List<char>> data)
    {
        var rows = data.Count;
        var cols = data[0].Count;

        var startPositions = new List<(int x, int y)>();
        var keysPositions = new Dictionary<char, (int x, int y)>();

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                var cell = data[i][j];
                if (cell == '@')
                    startPositions.Add((i, j));
                else if (IsKey(cell))
                    keysPositions.Add(cell, (i, j));
            }
        }

        var visited = new HashSet<string>();
        var queue = new Queue<(List<(int, int)> positions, string keys, int steps)>();

        queue.Enqueue((startPositions, "", 0));
        visited.Add(EncodeState(startPositions, ""));


        while (queue.Count > 0)
        {
            var (positions, keys, steps) = queue.Dequeue();

            if (keys.Length == keysPositions.Count)
                return steps;

            for (var i = 0; i < 4; i++)
            {
                var (x, y) = positions[i];

                foreach (var (dx, dy) in directions)
                {
                    var nx = x + dx;
                    var ny = y + dy;

                    if (nx < 0 || ny < 0 || nx >= rows || ny >= cols)
                        continue;

                    var cell = data[nx][ny];
                    if (cell == '#') continue;

                    if (IsDoor(cell) && !keys.Contains(char.ToLower(cell)))
                        continue;

                    var newKeys = keys;
                    if (IsKey(cell) && !keys.Contains(cell))
                    {
                        newKeys += cell;
                        newKeys = string.Concat(newKeys.OrderBy(c => c));
                    }

                    var newPositions = new List<(int, int)>(positions)
                    {
                        [i] = (nx, ny)
                    };

                    var state = EncodeState(newPositions, newKeys);
                    if (!visited.Add(state)) continue;
                    queue.Enqueue((newPositions, newKeys, steps + 1));
                }
            }
        }

        return -1;
    }



    static void Main()
    {
        var data = GetInput();
        var result = Solve(data);

        if (result == -1)
        {
            Console.WriteLine("No solution found");
        }
        else
        {
            Console.WriteLine(result);
        }
    }
}