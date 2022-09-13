using System.Text.RegularExpressions;

namespace DevastedSystematics.ConsoleLib;

/// <summary>
/// Provides static methods getting valid console input.
/// </summary>
public static class Input
{
    /// <summary>
    /// Validates input.
    /// </summary>
    /// <param name="validationRegex">Pattern.</param>
    /// <returns>Validated input.</returns>
    /// <exception cref="FormatException">Throws when input doesn't match pattern.</exception>
    public static string Read(string validationRegex)
    {
        string? input = ReadLine();

        bool isValid = IsValidInput(input, validationRegex);
        if (isValid)
            return input!;

        throw new FormatException($"{input} does not match the patthern [{validationRegex}].");
    }

    /// <summary>
    /// Validates input.
    /// </summary>
    /// <param name="validationRegex">Pattern.</param>
    /// <param name="inputPrecedingMessage">Message to show before first input and after each next input.</param>
    /// <returns>Validated input.</returns>
    /// <exception cref="FormatException">Throws when input doesn't match pattern.</exception>
    public static string Read(string validationRegex, string inputPrecedingMessage)
    {
        Write(inputPrecedingMessage);
        return Read(validationRegex);
    }

    /// <summary>
    /// Validates input.
    /// </summary>
    /// <param name="validationRegex">Pattern.</param>
    /// <param name="inputPrecedingMessage">Message to show before first input and after each next input.</param>
    /// <returns>Validated input.</returns>
    /// <exception cref="FormatException">Throws when input doesn't match pattern.</exception>
    public static string Read(string validationRegex, Message inputPrecedingMessage)
    {
        inputPrecedingMessage.Show();
        return Read(validationRegex);
    }

    public static bool TryRead(string validationRegex, out string? input) => TryRead(validationRegex, "", out input);

    public static bool TryRead(string validationRegex, string inputPrecedingMessage, out string? input)
    {
        try
        {
            input = Read(validationRegex, inputPrecedingMessage);
            return true;
        }
        catch (FormatException)
        {
            input = null;
            return false;
        }
    }

    /// <summary>
    /// Repeatedly validates input until valid or out of attempts.
    /// </summary>
    /// <param name="validationRegex">Pattern.</param>
    /// <param name="attemptsCount">Attempts count.</param>
    /// <returns>Validated input.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if attempts count <= 0.</exception>
    /// <exception cref="FormatException">Throws when input doesn't match pattern and no attempts remaining.</exception>
    public static string ReadUntilValidOrOutOfAttempts(string validationRegex, int attemptsCount)
        => ReadUntilValidOrOutOfAttempts(validationRegex, "", attemptsCount);

    /// <summary>
    /// Repeatedly validates input until valid or out of attempts.
    /// </summary>
    /// <param name="validationRegex">Pattern.</param>
    /// <param name="inputPrecedingMessage">Message to show before first input and after each next input.</param>
    /// <param name="attemptsCount">Attempts count.</param>
    /// <returns>Validated input.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if attempts count <= 0.</exception>
    /// <exception cref="FormatException">Throws when input doesn't match pattern and no attempts remaining.</exception>
    public static string ReadUntilValidOrOutOfAttempts(string validationRegex, string inputPrecedingMessage, int attemptsCount)
        => ReadUntilValidOrOutOfAttempts(validationRegex, inputPrecedingMessage, attemptsCount, "");

    /// <summary>
    /// Repeatedly validates input until valid or out of attempts.
    /// </summary>
    /// <param name="validationRegex">Pattern.</param>
    /// <param name="inputPrecedingMessage">Message to show before first input and after each next input.</param>
    /// <param name="attemptsCount">Attempts count.</param>
    /// <param name="invalidInputMessage">Message to show after validation failed message.</param>
    /// <returns>Validated input.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if attempts count <= 0.</exception>
    /// <exception cref="FormatException">Throws when input doesn't match pattern and no attempts remaining.</exception>
    public static string ReadUntilValidOrOutOfAttempts(string validationRegex, string inputPrecedingMessage,
        int attemptsCount, string invalidInputMessage)
    {
        if (attemptsCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(attemptsCount));

        int count = 0;
        while (count < attemptsCount)
        {
            try
            {
                return Read(validationRegex, inputPrecedingMessage);
            }
            catch (FormatException)
            {
                Write(invalidInputMessage);
                count += 1;
            }
        }
        throw new FormatException($"input remained invalid after {count} attempts.");
    }

    public static bool TryReadUntilValidOrOutOfAttempts(string validationRegex, int attemptsCount, out string? input)
        => TryReadUntilValidOrOutOfAttempts(validationRegex, "", attemptsCount, out input);

    public static bool TryReadUntilValidOrOutOfAttempts(string validationRegex, string inputPrecedingMessage,
        int attemptsCount, out string? input)
        => TryReadUntilValidOrOutOfAttempts(validationRegex, inputPrecedingMessage, attemptsCount, "", out input);

    public static bool TryReadUntilValidOrOutOfAttempts(string validationRegex, string inputPrecedingMessage,
        int attemptsCount, string invalidInputMessage, out string? input)
    {
        try
        {
            input = ReadUntilValidOrOutOfAttempts(validationRegex, inputPrecedingMessage, attemptsCount, invalidInputMessage);
            return true;
        }
        catch (FormatException)
        {
            input = null;
            return false;
        }
    }

    /// <summary>
    /// Repeatedly validates input until valid.
    /// </summary>
    /// <param name="validationRegex">Pattern.</param>
    /// <returns>Validated input.</returns>
    public static string ReadUntilValid(string validationRegex) => ReadUntilValid(validationRegex, "");

    /// <summary>
    /// Repeatedly validates input until valid.
    /// </summary>
    /// <param name="validationRegex">Pattern.</param>
    /// <param name="inputPrecedingMessage">Message to show before first input and after each next input.</param>
    /// <returns>Validated input.</returns>
    public static string ReadUntilValid(string validationRegex, string inputPrecedingMessage)
        => ReadUntilValid(validationRegex, inputPrecedingMessage, "");

    /// <summary>
    /// Repeatedly validates input until valid.
    /// </summary>
    /// <param name="validationRegex">Pattern.</param>
    /// <param name="inputPrecedingMessage">Message to show before first input and after each next input.</param>
    /// <returns>Validated input.</returns>
    public static string ReadUntilValid(string validationRegex, Message inputPrecedingMessage)
        => ReadUntilValid(validationRegex, inputPrecedingMessage, Message.Empty);

    /// <summary>
    /// Repeatedly validates input until valid.
    /// </summary>
    /// <param name="validationRegex">Pattern.</param>
    /// <param name="inputPrecedingMessage">Message to show before first input and after each next input.</param>
    /// <param name="invalidInputMessage">Message to show after validation failed message.</param>
    /// <returns>Validated input.</returns>
    public static string ReadUntilValid(string validationRegex, string inputPrecedingMessage, string invalidInputMessage)
    {
        while (true)
        {
            try
            {
                return Read(validationRegex, inputPrecedingMessage);
            }
            catch (FormatException)
            {
                Write(invalidInputMessage);
            }
        }
    }

    public static string ReadUntilValid(string validationRegex, string inputPrecedingMessage, Message invalidInputMessage)
        => ReadUntilValid(validationRegex, new Message(inputPrecedingMessage), invalidInputMessage);

    /// <summary>
    /// Repeatedly validates input until valid.
    /// </summary>
    /// <param name="validationRegex">Pattern.</param>
    /// <param name="inputPrecedingMessage">Message to show before first input and after each next input.</param>
    /// <param name="invalidInputMessage">Message to show after validation failed message.</param>
    /// <returns>Validated input.</returns>
    public static string ReadUntilValid(string validationRegex, Message inputPrecedingMessage, Message invalidInputMessage)
    {
        while (true)
        {
            try
            {
                return Read(validationRegex, inputPrecedingMessage);
            }
            catch (FormatException)
            {
                invalidInputMessage.Show();
            }
        }
    }

    private static bool IsValidInput(string? input, string validationRegex)
    {
        if (input is null)
            return false;

        return Regex.IsMatch(input, validationRegex);
    }
}
