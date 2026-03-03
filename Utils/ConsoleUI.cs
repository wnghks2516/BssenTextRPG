using System;
using TextRPG.Models;

namespace TextRPG.Utils;

/// <summary>
/// 콘솔 UI 표시를 담당하는 정적 클래스
/// </summary>
public static class ConsoleUI
{
    #region 타이틀 및 헤더
    public static void ShowTitle()
    {
        Console.Clear();
        Console.WriteLine(@"
            ╔═══════════════════════════════════════════════════════════════════════╗
            ║                                                                       ║
            ║  ████████╗███████╗██╗  ██╗████████╗    ██████╗ ██████╗  ██████╗       ║
            ║  ╚══██╔══╝██╔════╝╚██╗██╔╝╚══██╔══╝    ██╔══██╗██╔══██╗██╔════╝       ║
            ║     ██║   █████╗   ╚███╔╝    ██║       ██████╔╝██████╔╝██║  ███╗      ║
            ║     ██║   ██╔══╝   ██╔██╗    ██║       ██╔══██╗██╔═══╝ ██║   ██║      ║
            ║     ██║   ███████╗██╔╝ ██╗   ██║       ██║  ██║██║     ╚██████╔╝      ║
            ║     ╚═╝   ╚══════╝╚═╝  ╚═╝   ╚═╝       ╚═╝  ╚═╝╚═╝      ╚═════╝       ║
            ║                                                                       ║
            ║                    턴제 전투 텍스트 RPG 게임                          ║
            ║                                                                       ║
            ╚═══════════════════════════════════════════════════════════════════════╝
        ");
    }

    public static void ShowHeader(string title)
    {
        Console.Clear();
        Console.WriteLine($"\n╔════════════════════════════════╗");
        Console.WriteLine($"║  {title,-28}  ║");
        Console.WriteLine($"╚════════════════════════════════╝\n");
    }

    public static void ShowSectionHeader(string title)
    {
        Console.WriteLine($"\n╔════════════════════════════════╗");
        Console.WriteLine($"║  {title,-28}  ║");
        Console.WriteLine($"╚════════════════════════════════╝\n");
    }
    #endregion

    #region 메뉴 표시
    public static void ShowStartMenu()
    {
        Console.Clear();
        ShowTitle();
        Console.WriteLine("\n╔══════════════════════════════════════════╗");
        Console.WriteLine("║            게임 시작                     ║");
        Console.WriteLine("╚══════════════════════════════════════════╝\n");

        Console.WriteLine("1. 새 게임");
        Console.WriteLine("2. 이어하기");
        Console.WriteLine("0. 종료");
    }

    public static void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("╔═════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                         메인 메뉴                       ║");
        Console.WriteLine("╚═════════════════════════════════════════════════════════╝");

        Console.WriteLine("\n 1. 상태 보기");
        Console.WriteLine(" 2. 인벤토리");
        Console.WriteLine(" 3. 상점");
        Console.WriteLine(" 4. 던전 입장");
        Console.WriteLine(" 5. 휴식 (HP/MP 회복)");
        Console.WriteLine(" 6. 저장");
        Console.WriteLine(" 0. 게임 종료");
    }

    public static void ShowShopMenu(Player player)
    {
        ShowHeader("상점");
        Console.WriteLine($"보유 골드: {player.Gold} G\n");

        Console.WriteLine("1. 아이템 구매");
        Console.WriteLine("2. 아이템 판매");
        Console.WriteLine("0. 나가기");
    }

    public static void ShowInventoryMenu()
    {
        ShowHeader("인벤토리");
        Console.WriteLine("\n1. 아이템 사용");
        Console.WriteLine("2. 아이템 버리기");
        Console.WriteLine("0. 나가기");
    }

    public static void ShowRestMenu(Player player, int restCost)
    {
        ShowHeader("휴식처");
        Console.WriteLine($"휴식 비용: {restCost} 골드");
        Console.WriteLine($"현재 골드: {player.Gold} 골드");
        Console.WriteLine($"현재 HP: {player.CurrentHP}/{player.MaxHP}");
        Console.WriteLine($"현재 MP: {player.CurrentMP}/{player.MaxMP}\n");
    }
    #endregion

    #region 전투 관련 UI
    public static void ShowBattleStart()
    {
        Console.Clear();
        Console.WriteLine("\n╔════════════════════════════════╗");
        Console.WriteLine("║       전투 시작!               ║");
        Console.WriteLine("╚════════════════════════════════╝\n");
    }

    public static void ShowBattleTurn(int turn)
    {
        Console.WriteLine($"\n═══ 턴 {turn} ═══");
    }

    public static void ShowPlayerTurn()
    {
        Console.WriteLine("\n╔════════════════════════════════╗");
        Console.WriteLine("║       플레이어 턴!             ║");
        Console.WriteLine("╚════════════════════════════════╝");
    }

    public static void ShowEnemyTurn(string enemyName)
    {
        Console.WriteLine($"\n═══ {enemyName}의 턴 ═══");
    }

    public static void ShowBattleMenu(Player player)
    {
        ShowPlayerTurn();
        Console.WriteLine($"HP: {player.CurrentHP}/{player.MaxHP} | MP: {player.CurrentMP}/{player.MaxMP}");
        
        int weaponBonus = player.EquippedWeapon?.AttackBonus ?? 0;
        int armorBonus = player.EquippedArmor?.DefenseBonus ?? 0;
        
        Console.WriteLine($"ATK: {player.AttackPower}(+{weaponBonus}) | DEF: {player.Defense}(+{armorBonus})");
        Console.WriteLine("\n1. 공격");
        Console.WriteLine("2. 스킬");
        Console.WriteLine("3. 도망");
    }

    public static void ShowBattleVictory()
    {
        Console.WriteLine("\n╔════════════════════════════════╗");
        Console.WriteLine("║       전투 승리!               ║");
        Console.WriteLine("╚════════════════════════════════╝\n");
    }

    public static void ShowBattleDefeat()
    {
        Console.WriteLine("\n╔════════════════════════════════╗");
        Console.WriteLine("║       전투 패배...             ║");
        Console.WriteLine("╚════════════════════════════════╝");
    }
    #endregion

    #region 상점 관련 UI
    public static void ShowShopBuyMenu(int gold)
    {
        ShowHeader("아이템 구매");
        Console.WriteLine($"보유 골드: {gold} G\n");
    }

    public static void ShowShopSellMenu()
    {
        ShowHeader("아이템 판매");
    }
    #endregion

    #region 특수 화면
    public static void ShowGameOver()
    {
        Console.Clear();
        Console.WriteLine("\n╔══════════════════════════════════════════╗");
        Console.WriteLine("║                                          ║");
        Console.WriteLine("║            Game Over                     ║");
        Console.WriteLine("║                                          ║");
        Console.WriteLine("╚══════════════════════════════════════════╝\n");
    }

    public static void ShowDungeonEntrance()
    {
        Console.Clear();
        ShowSectionHeader("던전 입장");
        Console.WriteLine("던전에 입장합니다...\n");
    }

    public static void ShowSaveGame()
    {
        ShowHeader("게임 저장");
        Console.WriteLine("게임을 저장합니다...\n");
    }
    #endregion

    #region 유틸리티 (Deprecated - InputHelper 사용 권장)
    [Obsolete("Use InputHelper.PressAnyKey() instead")]
    public static void PressAnyKey()
    {
        InputHelper.PressAnyKey();
    }
    #endregion
}
