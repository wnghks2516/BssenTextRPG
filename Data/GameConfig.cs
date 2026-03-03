namespace TextRPG.Data;

/// <summary>
/// 게임 전역 설정 및 상수
/// </summary>
public static class GameConfig
{
    // 파일 경로
    public const string SaveFilePath = "savegame.json";

    // 게임 비용
    public const int RestCost = 50;

    // 전투 설정
    public const int SkillMpCost = 20;
    public const float SkillDamageMultiplier = 1.5f;
    public const double EscapeSuccessRate = 0.5;

    // 상점 설정
    public const float SellPriceRate = 0.5f; // 판매가는 구매가의 50%

    // 초기 설정
    public const int InitialGold = 1000;
    public const string DefaultPlayerName = "무명용사";

    // 메시지
    public const string InvalidInputMessage = "잘못된 입력입니다. 다시 선택해주세요.";
    public const string PressAnyKeyMessage = "\n아무 키나 누르면 계속...";
}
