using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TextRPG.Utils;
using TextRPG.Models;
using TextRPG.Systems;

namespace TextRPG.Data;

/// <summary>
/// 게임의 전반적인 흐름을 관리하는 싱글톤 클래스
/// </summary>
public class GameManager
{
    #region 싱글톤 패턴
    private static GameManager? instance;
    public static GameManager Instance => instance ??= new GameManager();

    private GameManager()
    {
        BattleSystem = new BattleSystem();
        Shop = new ShopSystem();
    }
    #endregion

    #region 프로퍼티
    public Player? Player { get; private set; }
    public bool IsRunning { get; private set; } = true;
    public BattleSystem BattleSystem { get; private set; }
    public InventorySystem? Inventory { get; private set; }
    public ShopSystem Shop { get; private set; }
    #endregion

    #region 게임 시작/종료
    public void StartGame(bool isLoadedGame = false)
    {
        ConsoleUI.ShowTitle();
        Console.WriteLine("게임을 시작합니다.\n");

        if (!isLoadedGame)
        {
            CreateCharacter();
            InitializeInventory();
            SetupInitialItems();
        }

        RunGameLoop();

        if (!IsRunning)
        {
            ConsoleUI.ShowGameOver();
        }
    }

    private void RunGameLoop()
    {
        IsRunning = true;
        while (IsRunning)
        {
            ShowMainMenu();
        }
    }

    public void QuitGame()
    {
        IsRunning = false;
        Console.WriteLine("\n게임을 종료합니다. 안녕히 가세요!");
    }
    #endregion

    #region 캐릭터 생성
    private void CreateCharacter()
    {
        string name = InputHelper.GetStringInput(
            "캐릭터의 이름을 입력하세요: ", 
            GameConfig.DefaultPlayerName
        );

        if (string.IsNullOrWhiteSpace(name))
        {
            name = GameConfig.DefaultPlayerName;
        }

        JobType jobType = InputHelper.GetEnumInput<JobType>("직업을 선택하세요");

        Player = new Player(name, jobType);
        Console.WriteLine($"\n'{Player.Name}'님이 '{Player.Job}' 직업으로 생성되었습니다!");
        Console.WriteLine("모험을 시작하겠습니다!\n");
        
        Player.DisplayInfo();
        InputHelper.PressAnyKey();
    }
    #endregion

    #region 초기화
    private void InitializeInventory()
    {
        Inventory = new InventorySystem();
    }

    private void SetupInitialItems()
    {
        if (Player == null || Inventory == null) return;

        var weapon = Equipment.CreateSword("목검");
        var armor = Equipment.CreateArmor("가죽갑옷");
        
        if (weapon != null && armor != null)
        {
            Inventory.AddItem(weapon);
            Inventory.AddItem(armor);
            Player.EquipItem(weapon);
            Player.EquipItem(armor);
        }

        var healthPotion = Consumable.CreatePotion("체력포션");
        var manaPotion = Consumable.CreatePotion("마나포션");
        
        if (healthPotion != null)
        {
            Inventory.AddItem(healthPotion);
            Inventory.AddItem(healthPotion);
        }
        
        if (manaPotion != null)
        {
            Inventory.AddItem(manaPotion);
        }

        Console.WriteLine("\n초기 아이템이 지급되었습니다.");
        InputHelper.PressAnyKey();
    }
    #endregion

    #region 메인 메뉴
    private void ShowMainMenu()
    {
        ConsoleUI.ShowMainMenu();
        int choice = InputHelper.GetIntInput("\n원하는 메뉴의 숫자를 입력하세요: ", 0, 6);

        HandleMenuChoice(choice);
    }

    private void HandleMenuChoice(int choice)
    {
        switch (choice)
        {
            case 1:
                ShowPlayerInfo();
                break;
            case 2:
                ShowInventory();
                break;
            case 3:
                ShowShop();
                break;
            case 4:
                EnterDungeon();
                break;
            case 5:
                RestPlayer();
                break;
            case 6:
                SaveGame();
                break;
            case 0:
                QuitGame();
                break;
        }
    }
    #endregion

    #region 메뉴 기능
    private void ShowPlayerInfo()
    {
        Player?.DisplayInfo();
        InputHelper.PressAnyKey();
    }

    private void ShowInventory()
    {
        if (Inventory == null || Player == null)
        {
            Console.WriteLine("인벤토리를 사용할 수 없습니다.");
            InputHelper.PressAnyKey();
            return;
        }

        Inventory.ShowInventoryMenu(Player);
    }

    private void ShowShop()
    {
        if (Player == null || Inventory == null)
        {
            Console.WriteLine("상점을 사용할 수 없습니다.");
            InputHelper.PressAnyKey();
            return;
        }

        Shop.ShowShopMenu(Player, Inventory);
    }

    private void EnterDungeon()
    {
        if (Player == null) return;

        ConsoleUI.ShowDungeonEntrance();

        Enemy enemy = Enemy.CreateEnemy(Player.Level);
        
        BattleSystem.StartBattle(Player, enemy);

        Console.WriteLine("\n던전 탐험을 끝내고 마을로 돌아갑니다.");
        InputHelper.PressAnyKey();
    }

    private void RestPlayer()
    {
        if (Player == null)
        {
            Console.WriteLine("플레이어가 없습니다.");
            InputHelper.PressAnyKey();
            return;
        }

        ConsoleUI.ShowRestMenu(Player, GameConfig.RestCost);

        if (Player.Gold < GameConfig.RestCost)
        {
            Console.WriteLine("골드가 부족하여 휴식을 취할 수 없습니다.");
            InputHelper.PressAnyKey();
            return;
        }

        if (InputHelper.GetConfirmation("휴식을 취하시겠습니까?"))
        {
            if (Player.SpendGold(GameConfig.RestCost))
            {
                Player.Rest();
            }
        }
        else
        {
            Console.WriteLine("휴식을 취하지 않았습니다.");
        }

        InputHelper.PressAnyKey();
    }

    private void SaveGame()
    {
        if (Player == null || Inventory == null)
        {
            Console.WriteLine("저장할 데이터가 없습니다.");
            InputHelper.PressAnyKey();
            return;
        }

        ConsoleUI.ShowSaveGame();

        if (SaveLoadSystem.SaveGame(Player, Inventory))
        {
            Console.WriteLine("게임이 성공적으로 저장되었습니다.");
        }
        else
        {
            Console.WriteLine("게임 저장에 실패했습니다.");
        }

        InputHelper.PressAnyKey();
    }
    #endregion

    #region 게임 로드
    public bool LoadGame()
    {
        var saveData = SaveLoadSystem.LoadGame();
        if (saveData == null)
        {
            Console.WriteLine("저장된 게임 데이터를 불러올 수 없습니다.");
            InputHelper.PressAnyKey();
            return false;
        }

        Player = SaveLoadSystem.LoadPlayer(saveData.Player);
        Inventory = SaveLoadSystem.LoadInventory(saveData.InventoryData, Player);
        SaveLoadSystem.LoadEquippedItems(Player, saveData.Player, Inventory);

        Console.WriteLine("게임이 성공적으로 로드되었습니다.");
        InputHelper.PressAnyKey();
        return true;
    }
    #endregion
}
