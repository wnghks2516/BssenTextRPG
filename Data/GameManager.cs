using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TextRPG.Utils;
using TextRPG.Models;

namespace TextRPG.Data;

public class GameManager
{
    // 싱글톤 패턴 구현
    // 싱글톤이란 어플리케이션 전체에서 단 하나의 인스턴스만 존재하도록 보장하는 디자인 패턴


    #region 싱글톤 패턴
    //리전이란 클래스, 메서드, 속성 등을 그룹화하여 코드의 가독성과 유지보수성을 높이는 기능
    //싱글톤 인스턴스 ( 내부접근용 변수 : 필드 )

    private static GameManager instance;


    //외부에서 인스턴스에 접근할 수 있는 정적 속성 ( 프로퍼티 )
    public static GameManager Instance
    {
        get
        {
            //인스턴스가 없으면 새로 생성
            if (instance == null)
                instance = new GameManager();
            return instance;
        }
    }

    private GameManager()
    {
        // 클래스가 생성될 때 초기화 작업 수행
    }
    #endregion

    #region 프로퍼티
    public Player? Player { get; private set; }

    //게임 실행여부
    public bool IsRunning { get; private set; } = true;

    #endregion



    #region 게임 시작/종료

    public void StartGame()
    {
        ConsoleUI.ShowTitle();
        Console.WriteLine("게임을 시작합니다.\n");

        //캐릭터 생성
        CreateCharacter();

        //메인 게임 루프
        IsRunning = true;
        while(IsRunning)
        {
            showMainMenu();
        }

        //Todo 인벤토리 초기화

        //Todo 초기 아이템 지급
    }

    #endregion


    #region 캐릭터 생성
    private void CreateCharacter()
    {
        //이름 입력
        Console.Write("캐릭터의 이름을 입력하세요 : ");
        string? name = Console.ReadLine(); // nullable 허용

        if ( string.IsNullOrWhiteSpace(name))
        {
            name = "무명용사"; // 기본 이름 설정
        }

        Console.WriteLine($"'{name}'님이 생성되었습니다. 모험을 시작하겠습니다!\n");

        //직업 선택
        Console.WriteLine("직업을 선택하세요  \n 1. 전사 \n 2. 궁수 \n 3. 마법사");

        JobType jobType = JobType.Warrior; // 기본값 설정

        while (true)
        {
            Console.Write("숫자를 입력하세요 : ");
            string? input = Console.ReadLine(); 

            switch(input)
            {
                case "1":
                    jobType = JobType.Warrior;
                    break;
                case "2":
                    jobType = JobType.Archer;
                    break;
                case "3":
                    jobType = JobType.Mage;
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    continue; // 잘못된 입력 시 다시 입력 받기
            }
            break; // 올바른 입력이 들어오면 루프 종료
        }

        //입력한 이름과 선택한 직업으로 플레이어 캐릭터 생성
        Player = new Player(name, jobType, gold: 1000);
        Console.WriteLine($"'{Player.Name}'님이 '{Player.Job}' 직업으로 생성되었습니다. 모험을 시작하겠습니다!\n");
        Player.DisplayInfo();
        //테스트 코드
        // Console.WriteLine($"[테스트] 플레이어 정보 : 이름={Player.Name}, 직업={Player.Job}, HP={Player.CurrentHP}/{Player.MaxHP}, MP={Player.CurrentMP}/{Player.MaxMP}, 공격력={Player.AttackPower}, 방어력={Player.Defense}, 레벨={Player.Level}, 골드={Player.Gold}");
        ConsoleUI.PressAnyKey();
    }
    #endregion

    #region 메인메뉴 출력
    public void showMainMenu()
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

        Console.Write("\n원하는 메뉴의 숫자를 입력하세요 : ");
        string? input = Console.ReadLine();
        switch(input)
        {
            case "1":
                Player?.DisplayInfo();
                ConsoleUI.PressAnyKey();
                break;
            case "2":
                //인벤토리 메뉴로 이동
                break;
            case "3":
                //상점 메뉴로 이동
                break;
            case "4":
                //던전 입장
                break;
            case "5":
                //휴식하여 HP/MP 회복
                break;
            case "6":
                //게임 저장
                break;
            case "0":
                IsRunning = false; // 게임 종료
                Console.WriteLine("게임을 종료합니다. 감사합니다!");
                break;
            default:
                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                ConsoleUI.PressAnyKey();
                break;
            }

        }
    #endregion
}

