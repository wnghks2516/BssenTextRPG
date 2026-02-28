using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Models;

namespace TextRPG.Utils;


public class ConsoleUI
{
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
        Console.WriteLine(" 5. 휴식 ( HP / MP ) 회복");
        Console.WriteLine(" 6. 저장");
        Console.WriteLine(" 0. 게임 종료");
    }


    //아무키나 누르면 계속 메시지 출력
    public static void PressAnyKey()
    {
        Console.WriteLine("\n아무 키나 누르면 계속...");
        Console.ReadKey(true); // true는 입력된 키를 콘솔에 표시하지 않도록 함
    }

    public static void DisplayInventory()
    {
        Console.Clear();
        Console.WriteLine("\n╔══════════════════════════════════════════╗");
        Console.WriteLine("║                                          ║");
        Console.WriteLine("║            인벤토리                      ║");
        Console.WriteLine("║                                          ║");
        Console.WriteLine("╚══════════════════════════════════════════╝\n");
    }

    public static void ShowGameOver()
    {
        Console.Clear();
        Console.WriteLine("\n╔══════════════════════════════════════════╗");
        Console.WriteLine("║                                          ║");
        Console.WriteLine("║            Game Over                     ║");
        Console.WriteLine("║                                          ║");
        Console.WriteLine("╚══════════════════════════════════════════╝\n");
    }

    public static void ShowStartMenu()
    {
        Console.Clear();
        ConsoleUI.ShowTitle();
        Console.WriteLine("\n╔══════════════════════════════════════════╗");
        Console.WriteLine("║            게임 시작                     ║");
        Console.WriteLine("╚══════════════════════════════════════════╝\n");

        Console.WriteLine("1. 새 게임");
        Console.WriteLine("2. 이어하기");
        Console.WriteLine("3. 종료");
    }
    public static void ShowShopMenu(Player player)
    {
        Console.Clear();
        Console.WriteLine("\n╔════════════════════════════════╗");
        Console.WriteLine("║       상      점               ║");
        Console.WriteLine("╚════════════════════════════════╝\n");
        Console.WriteLine($"보유 골드 : {player.Gold} 골드");

        Console.WriteLine("\n1. 아이템 구매");
        Console.WriteLine("2. 아이템 판매");
        Console.WriteLine("3. 상점 나가기");
        Console.Write("\n선택 : ");
    }


    public static void PlayerTurn(Player player, Enemy enemy)
    {
        Console.WriteLine("\n╔════════════════════════════════╗");
        Console.WriteLine($"║         플레이어 턴!           ║");
        Console.WriteLine("╚════════════════════════════════╝\n");

        Console.WriteLine($"HP : {player.CurrentHP}/{player.MaxHP}  MP: {player.CurrentMP}/{player.MaxMP}");
        Console.WriteLine("\n 행동을 선택하세요.");
        Console.WriteLine("1. 공격");
        Console.WriteLine("2. 스킬");
        Console.WriteLine("3. 도망");
    }
}
