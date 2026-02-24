using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
