using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public static class WordGridGenerator
{
    private static System.Random random = new System.Random();

    public static char[] GetLettersList(
        int sizeX,
        int sizeY,
        int subjects,
        int objects,
        int predicate,
        int level1,
        int level2,
        int level3,
        List<Word> words
    )
    {
        words = words.OrderBy(_ => random.Next()).ToList();

        List<Word> wordsToPrint = GetWordsList(
            Math.Max(sizeX, sizeY),
            subjects,
            objects,
            predicate,
            level1,
            level2,
            level3,
            words
        );

        wordsToPrint = wordsToPrint
            .OrderByDescending(w => w.value.Length)
            .ToList();

       foreach(Word w in wordsToPrint)
        {
            Debug.Log("parola stampata: " + w.value);
        }

        return PlaceWords(wordsToPrint, sizeX, sizeY);
    }

    private static List<Word> GetWordsList(
        int maxWordLength,
        int subjects,
        int objects,
        int predicate,
        int level1,
        int level2,
        int level3,
        List<Word> words
    )
    {
        List<Word> wordsToPrint = new();

        if (objects > 0)
        {
            wordsToPrint.AddRange(
                GetObjectWords(maxWordLength, objects, level1, level2, level3, words)
            );
        }

        if (subjects > 0)
        {
            wordsToPrint.AddRange(
                words
                    .Where(w => w.wordType == WordTypeEnum.SUBJECT &&
                                w.value.Length <= maxWordLength)
                    .Take(subjects)
            );
        }

        if (predicate > 0)
        {
            wordsToPrint.AddRange(
                words
                    .Where(w => w.wordType == WordTypeEnum.PREDICATE &&
                                w.value.Length <= maxWordLength)
                    .Take(predicate)
            );
        }

        return wordsToPrint;
    }

    private static List<Word> GetObjectWords(
        int maxWordLength,
        int objects,
        int level1,
        int level2,
        int level3,
        List<Word> words
    )
    {
        if ((level1 + level2 + level3) > objects)
            throw new Exception("Too many leveled object words requested.");

        List<Word> objectWords = new();

        void AddLevel(int level, int amount)
        {
            if (amount <= 0) return;

            objectWords.AddRange(
                words
                    .Where(w => w.wordType == WordTypeEnum.OBJECT &&
                                w.level == level &&
                                w.value.Length <= maxWordLength)
                    .Take(amount)
            );
        }

        AddLevel(1, level1);
        AddLevel(2, level2);
        AddLevel(3, level3);

        int remaining = objects - objectWords.Count;

        if (remaining > 0)
        {
            objectWords.AddRange(
                words
                    .Where(w => w.wordType == WordTypeEnum.OBJECT &&
                                w.level == 0)
                    .Take(remaining)
            );
        }

        return objectWords;
    }

    private static char[] PlaceWords(List<Word> wordsToPrint, int sizeX, int sizeY)
    {
        char[,] grid = new char[sizeY, sizeX];

        foreach (var word in wordsToPrint)
        {
            string value = word.value.ToUpper();
            Debug.Log("parola che viene ciclata: "+value);
            bool placed = false;

            for (int attempt = 0; attempt < 500 && !placed; attempt++)
            {
                Direction dir = DirectionExtensions.Random();

                int minX = (dir.dx < 0) ? value.Length - 1 : 0;
                int maxX = (dir.dx > 0) ? sizeX - value.Length : sizeX - 1;

                int minY = (dir.dy < 0) ? value.Length - 1 : 0;
                int maxY = (dir.dy > 0) ? sizeY - value.Length : sizeY - 1;

                if (minX > maxX || minY > maxY)
                    continue;

                int startX = random.Next(minX, maxX + 1);
                int startY = random.Next(minY, maxY + 1);

                if (CanPlace(grid, value, startX, startY, dir))
                {
                    PlaceWord(grid, value, startX, startY, dir);
                    placed = true;
                }
            }

            if (!placed)
                throw new Exception($"Cannot place word: {value}");
        }

        FillWithRandomChars(grid, sizeX, sizeY);

        return Flatten(grid, sizeX, sizeY);
    }

    private static bool CanPlace(char[,] grid, string word, int x, int y, Direction dir)
    {
        for (int i = 0; i < word.Length; i++)
        {
            int nx = x + i * dir.dx;
            int ny = y + i * dir.dy;

            char current = grid[ny, nx];

            if (current != '\0' && current != word[i])
                return false;
        }
        return true;
    }

    private static void PlaceWord(char[,] grid, string word, int x, int y, Direction dir)
    {
        for (int i = 0; i < word.Length; i++)
        {
            int nx = x + i * dir.dx;
            int ny = y + i * dir.dy;

            grid[ny, nx] = word[i];
        }
    }

    private static void FillWithRandomChars(char[,] grid, int sizeX, int sizeY)
    {
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                if (grid[y, x] == '\0')
                    grid[y, x] = RandomChar();
            }
        }
    }

    private static char RandomChar()
    {
        char[] vowels = { 'A', 'E', 'I', 'O', 'U', 'S' };
        char[] consonants = { 'B', 'C', 'D', 'F', 'G', 'H', 'L', 'M', 'N', 'P', 'R', 'T', 'V', 'Z' };

        return random.NextDouble() < 0.6
            ? vowels[random.Next(vowels.Length)]
            : consonants[random.Next(consonants.Length)];
    }

    private static char[] Flatten(char[,] grid, int sizeX, int sizeY)
    {
        char[] result = new char[sizeX * sizeY];
        int index = 0;

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                result[index++] = grid[y, x];
            }
        }

        return result;
    }
}




// ------------------------------ CLASSES --------------------------------------

public enum WordTypeEnum
{
    OBJECT,
    SUBJECT,
    PREDICATE
}

[Serializable]
public class Word
{
    public string id;
    public string value;
    public WordTypeEnum wordType;
    public int level;
}

public struct Direction
{
    public int dx;
    public int dy;

    public Direction(int dx, int dy)
    {
        this.dx = dx;
        this.dy = dy;
    }
}

public static class DirectionExtensions
{
    private static Direction[] dirs = new Direction[]
    {
        new Direction(1,0),
        new Direction(-1,0),
        new Direction(0,1),
        new Direction(0,-1),
        new Direction(1,1),
        new Direction(-1,1),
        new Direction(1,-1),
        new Direction(-1,-1),
    };

    private static System.Random random = new();

    public static Direction Random()
        => dirs[random.Next(dirs.Length)];
}