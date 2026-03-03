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
            int choice = InputHelper.GetIntInput("\n선택: ", 0, 2);

            switch (choice)
            {
                case 1:
                    // 새 게임 시작
                    GameManager.Instance.StartGame();
                    return;
                case 2:

                    //이어 하기
                    if (GameManager.Instance.LoadGame())
                    {
                        GameManager.Instance.StartGame(isLoadedGame: true);
                    }
                    return;
                case 0:
                    Console.WriteLine("\n게임을 종료합니다. 안녕히 가세요!");
                    return;
            }
        }
    }
}


