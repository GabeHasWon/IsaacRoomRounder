using System.Text;

namespace IsaacRoomRounder;

/// <summary>
/// This is probably slow. Oh well!
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0 || args[0] == string.Empty)
        {
            Console.WriteLine("No file provided. Drag a file onto the exe to run.");
            Console.ReadLine();
            return;
        }

        const string Difficulty = "difficulty=\"";

        using var file = new StreamReader(File.OpenRead(args[0]));
        string text = file.ReadToEnd();
        int index = 0;
        StringBuilder builder = new(text);
        List<int> indexes = [];

        // Map indices
        while (true)
        {
            int newIndex = text.IndexOf(Difficulty, index) + Difficulty.Length;

            if (newIndex > index)
                index = newIndex;
            else
                break;

            indexes.Add(index);
        }

        // Move backwards through indexes so we don't modify forward indexes.
        for (int i = indexes.Count - 1; i >= 0; --i)
        {
            index = indexes[i];
            string number = text[index..text.IndexOf('"', index)];
            ModifyNumber(index, builder, number);
        }

        // Post adjusted file
        string adjName = args[0].Insert(args[0].Length - 4, "_adjusted");
        using var writer = File.CreateText(adjName);
        writer.Write(builder.ToString());

        Console.WriteLine("Modified file. You can close this now.");
        Console.ReadLine();
    }

    /// <summary>
    /// Modifies the number if it's valid.
    /// </summary>
    private static void ModifyNumber(int index, StringBuilder builder, string number)
    {
        if (int.TryParse(number, out int value))
        {
            // Can it be negative? No. Edge case anyway.
            if (value is <= 0 or 1 or 2)
                value = 1;
            else if (value is 3 or 4 or 5 or 6)
                value = 5;
            else if (value is 7 or 8 or 9 or 10 or 11 or 12)
                value = 10;
            else if (value is 13 or 14 or 15 or 16 or 17)
                value = 15;
            else
                value = 20;

            builder.Replace(number, value.ToString(), index, number.Length);
        }
    }
}