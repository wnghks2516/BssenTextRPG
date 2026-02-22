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

    #endregion



    #region 게임 시작/종료

    public void StartGame()
    {
        ConsoleUI.ShowTitle();
        Console.WriteLine("게임을 시작합니다.\n");

        
        CreateCharacter();

        //Todo 인벤토리 초기화

        //Todo 초기 아이템 지급
    }

    #endregion


    #region 캐릭터 생성
    private void CreateCharacter()
    {
        //이름 입력
        Console.WriteLine("캐릭터의 이름을 입력하세요 : ");
        string? name = Console.ReadLine(); // nullable 허용

        if ( string.IsNullOrWhiteSpace(name))
        {
            name = "무명용사"; // 기본 이름 설정
        }

        Console.WriteLine($"'{name}'님이 생성되었습니다. 모험을 시작하겠습니다!\n");

        //직업 선택
        Console.WriteLine("직업을 선택하세요 : \n 1. 전사 \n 2. 궁수 \n 3. 마법사");

        JobType jobType = JobType.Warrior; // 기본값 설정

        while (true)
        {
            Console.WriteLine("숫자를 입력하세요 : ");
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
    }
    #endregion
}

