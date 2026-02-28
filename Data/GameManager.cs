using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TextRPG.Utils;
using TextRPG.Models;
using TextRPG.Systems;
using TextRPG.Data;
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

        //전투 시스템 초기화
        BattleSystem = new BattleSystem();

        //상점 시스템 초기화
        Shop = new ShopSystem();
    }
    #endregion

    #region 프로퍼티
    public Player? Player { get; private set; }

    //게임 실행여부
    public bool IsRunning { get; private set; } = true;

    #endregion

    //전투 시스템
    public BattleSystem BattleSystem { get; private set; }

    //인벤토리 시스템
    public InventorySystem Inventory { get; private set; }

    //상점 시스템
    public ShopSystem Shop { get; private set; }

    // 저장/불러오기 시스템
    public SaveLoadSystem SaveLoadManager { get; private set; }

    #region 게임 시작/종료

    //게임 시작 매서드

    

    public void StartGame(bool loadedGame = false)
    {
        ConsoleUI.ShowTitle();
        Console.WriteLine("게임을 시작합니다.\n");

        //새로 시작하는 게임에서만 새 캐릭터 및 설정을 처리
        //캐릭터 생성

        if (!loadedGame)
        {
            CreateCharacter();

            //인벤토리 초기화
            Inventory = new InventorySystem();

            SetupInitItems();

        }
        //메인 게임 루프
        IsRunning = true;
        while(IsRunning)
        {
            showMainMenu();
        }
        if(!IsRunning)
        {
            ConsoleUI.ShowGameOver();
        }
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
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= 3)
            {
                jobType = (JobType)(choice - 1);
                break;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
            }
             
            break; // 올바른 입력이 들어오면 루프 종료
        }

        //입력한 이름과 선택한 직업으로 플레이어 캐릭터 생성
        Player = new Player(name, jobType);
        Console.WriteLine($"'{Player.Name}'님이 '{Player.Job}' 직업으로 생성되었습니다. 모험을 시작하겠습니다!\n");
        Player.DisplayInfo();



        ConsoleUI.PressAnyKey();
    }
    #endregion
    
    #region 초기 아이템 지급
    private void SetupInitItems()
    {
        //기본 장비
        // var = 지역변수, 지역변수는 메서드 내부에서만 사용 가능
        // 필드와 프로퍼티는 클래스 전체에서 사용 가능
        // sum이라는 지역변수는 이 메서드 내부에서만 사용 가능, 다른 메서드에서는 접근 불가능
        var weapon = Equipment.CreateSword("목검");
        var armor  = Equipment.CreateArmor("가죽갑옷");
        Inventory.AddItem(weapon);
        Inventory.AddItem(armor);

        //기본 장비 착용
        Player.EquipItem(weapon);
        Player.EquipItem(armor);


        Inventory.AddItem(Consumable.CreatePotion("체력포션"));
        Inventory.AddItem(Consumable.CreatePotion("체력포션"));
        Inventory.AddItem(Consumable.CreatePotion("마나포션"));

        Console.WriteLine("\n초기 아이템이 지급되었습니다.");
        ConsoleUI.PressAnyKey();
    }
    #endregion

    #region 메인메뉴 출력
    public void showMainMenu()
    {
        ConsoleUI.ShowMainMenu();
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
                Inventory.ShowInventoryMenu(Player);
                break;
            case "3":
                //상점 메뉴로 이동
                Shop.ShowShopMenu(Player, Inventory);
                break;
            case "4":
                //던전 입장
                EnterDungeon();
                break;
            case "5":
                //휴식하여 HP/MP 회복
                RestPlayer(Player);
                break;
            case "6":
                //게임 저장
                SaveGame(Player, Inventory);
                break;
            case "0":
                IsRunning = false; // 게임 종료
                ConsoleUI.ShowGameOver(); 
                ConsoleUI.PressAnyKey();
                break;
            default:
                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                ConsoleUI.PressAnyKey();
                break;
            }

        }
    #endregion

    #region 메뉴 기능
    // 던전 입장

    public void EnterDungeon()
    {
        Console.Clear();   
        Console.WriteLine("던전에 입장합니다...");

        //적 생성
        Enemy enemy = Enemy.CreateEnemy(Player!.Level);
        ConsoleUI.PressAnyKey();

        //전투 시작
        BattleSystem.StartBattle(Player!, enemy);

        Console.WriteLine("\n 던전 탐험을 끝내고 마을로 돌아갑니다..");
        ConsoleUI.PressAnyKey();
    }
    #endregion

    #region 휴식 기능
    private void RestPlayer(Player player)
    {
        //상수 const : 상수는 한 번 값이 할당되면 변경할 수 없는 변수, 보통 대문자로 작성하여 구분
        const int restCost = 50;
        Console.Clear();
        Console.WriteLine("휴식을 취합니다... HP와 MP가 회복됩니다.");
        if ( player.Gold < restCost)
        {
            Console.WriteLine("골드가 부족하여 휴식을 취할 수 없습니다.");
            ConsoleUI.PressAnyKey();
            return;
        }

        Console.WriteLine($"휴식 비용 : {restCost} 골드을 지불하여 휴식을 취하겠습니까? Y / N");
        if(Console.ReadLine()?.ToUpper() == "Y")
        {
            player.SpendGold(restCost);
            player.Rest(restCost);
            Console.WriteLine("휴식을 취했습니다. HP와 MP가 회복되었습니다.");
        }
        else
        {
            Console.WriteLine("휴식을 취하지 않았습니다.");
        }
        ConsoleUI.PressAnyKey();
    }
    #endregion


    #region 게임 저장 및 로드 기능

    //게임 저장

    private void SaveGame(Player player, InventorySystem inventory)
    {
        Console.Clear();
        Console.WriteLine("게임을 저장합니다...");
        //저장 매서드 호출
        SaveLoadSystem.SaveGame(player, inventory);
        if ( Player == null || Inventory == null)
        {
            Console.WriteLine("저장할 데이터가 없습니다.");
            ConsoleUI.PressAnyKey();
            return;
        }
        if (SaveLoadSystem.SaveGame(player, inventory))
        {
            Console.WriteLine("게임이 성공적으로 저장되었습니다.");
        }
        else
        {
            Console.WriteLine("게임 저장에 실패했습니다.");
        }
        ConsoleUI.PressAnyKey();
    }

    //게임 로드

    public bool LoadGame()
    {
        var saveData = SaveLoadSystem.LoadGame();
        if (saveData == null) return false;

        // 1. Player 복원
        Player = SaveLoadSystem.LoadPlayer(saveData.Player);


        // 2. Inventory 복원
        Inventory = SaveLoadSystem.LoadInventory(saveData.InventoryData, Player);
        // 3. 장착 아이템 복원
        SaveLoadSystem.LoadEquippedItems(Player,saveData.Player, Inventory);


        Console.WriteLine("게임이 성공적으로 로드되었습니다.");
        ConsoleUI.PressAnyKey();
        return true;
    }


    #endregion
}
