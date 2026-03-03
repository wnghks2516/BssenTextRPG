namespace TextRPG.Utils;

/// <summary>
/// 사용자 입력 처리를 위한 헬퍼 클래스
/// </summary>
public static class InputHelper
{
    /// <summary>
    /// 사용자로부터 유효한 정수 입력을 받습니다
    /// </summary>
    /// <param name="prompt">입력 안내 메시지</param>
    /// <param name="min">최소값</param>
    /// <param name="max">최대값</param>
    /// <returns>유효한 정수 입력</returns>
    public static int GetIntInput(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int result) && result >= min && result <= max)
            {
                return result;
            }

            Console.WriteLine($"{Data.GameConfig.InvalidInputMessage} ({min}~{max} 사이의 숫자를 입력하세요)");
        }
    }

    /// <summary>
    /// 사용자로부터 Enum 선택을 받습니다
    /// </summary>
    /// <typeparam name="T">Enum 타입</typeparam>
    /// <param name="prompt">선택 안내 메시지</param>
    /// <returns>선택된 Enum 값</returns>
    public static T GetEnumInput<T>(string prompt) where T : struct, Enum
    {
        T[] values = Enum.GetValues<T>();
        
        Console.WriteLine(prompt);
        for (int i = 0; i < values.Length; i++)
        {
            Console.WriteLine($" {i + 1}. {values[i]}");
        }

        int choice = GetIntInput("숫자를 입력하세요: ", 1, values.Length);
        return values[choice - 1];
    }

    /// <summary>
    /// Yes/No 확인을 받습니다
    /// </summary>
    /// <param name="message">확인 메시지</param>
    /// <returns>Yes이면 true, 아니면 false</returns>
    public static bool GetConfirmation(string message)
    {
        Console.Write($"{message} (Y/N): ");
        string? input = Console.ReadLine();
        return input?.ToUpper() == "Y";
    }

    /// <summary>
    /// 비어있지 않은 문자열 입력을 받습니다
    /// </summary>
    /// <param name="prompt">입력 안내 메시지</param>
    /// <param name="defaultValue">기본값 (입력이 없을 경우)</param>
    /// <returns>입력된 문자열 또는 기본값</returns>
    public static string GetStringInput(string prompt, string? defaultValue = null)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            return defaultValue ?? string.Empty;
        }

        return input;
    }

    /// <summary>
    /// 아무 키나 누르면 계속
    /// </summary>
    /// <param name="message">표시할 메시지 (null이면 기본 메시지)</param>
    public static void PressAnyKey(string? message = null)
    {
        Console.WriteLine(message ?? Data.GameConfig.PressAnyKeyMessage);
        Console.ReadKey(true);
    }
}
