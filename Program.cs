using TextRPG.Utils;
using TextRPG.Data;

namespace BssenTextRPG;

internal class Program
{
    static void Main(string[] args)
    {
        //Todo 타이틀 표시 
        
        // 저장된 게임 존재 여부 확인
        if(SaveLoadSystem.IsSaveFileExists())
        {
            // 메뉴 오픈 ( 새 게임, 이어서하기 , 종료 )
            ShowStartMenu();
        }
        else
        {
            GameManager.Instance.StartGame();
        }
    }
    static void ShowStartMenu()
    {
        ConsoleUI.ShowStartMenu();
        

        while (true)
        {
            Console.Write("선택: ");
            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    // 새 게임 시작
                    GameManager.Instance.StartGame();
                    return;
                case "2":

                    //이어 하기
                    if (GameManager.Instance.LoadGame())
                    {
                        GameManager.Instance.StartGame(true);
                    }
                    return;
                case "3":
                    Console.WriteLine("게임을 종료합니다.");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
                    break;
            }
        }
    }
}


