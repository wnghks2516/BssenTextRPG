using System;
using TextRPG.Models;
using TextRPG.Utils;

namespace TextRPG.Systems;

public class ShopSystem
{
    // 상점 시스템
    // 메뉴 선택 ( 구매 , 판매 , 취소 )

    #region 프로퍼티
    //판매중인 아이템 목록
    //protected set을 사용 하지 않는 이유는 상점 시스템에서 아이템 목록을 변경할 수 있지만,
    //외부에서는 변경할 수 없도록 하기 위함.

    private List<Item>? ShopItems { get; set; }


    #endregion
    #region 생성자
    public ShopSystem()
    {
        // 상점 아이템 초기화
        ShopItems = new List<Item>();
        InitShop();
    }
    #endregion


    #region 초기화 매서드
    private void InitShop()
    {
        // 상점에 아이템 추가
        //무기
        ShopItems.Add(Equipment.CreateSword("목검"));
        ShopItems.Add(Equipment.CreateSword("철검"));
        ShopItems.Add(Equipment.CreateSword("전설검"));

        //방어구
        ShopItems.Add(Equipment.CreateArmor("천갑옷"));
        ShopItems.Add(Equipment.CreateArmor("철갑옷"));
        ShopItems.Add(Equipment.CreateArmor("전설갑옷"));

        //포션
        ShopItems.Add(Consumable.CreatePotion("체력포션"));
        ShopItems.Add(Consumable.CreatePotion("대형체력포션"));
        ShopItems.Add(Consumable.CreatePotion("마나포션"));
        ShopItems.Add(Consumable.CreatePotion("대형마나포션"));

    }
    #endregion

    #region 상점 메뉴
    public void ShowShopMenu(Player player, InventorySystem inventory)
    {
        while ( true )
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
            string? input = Console.ReadLine();

            switch(input)
            {
                case "1":
                    //구매 
                    break;
                case "2":
                    // 판매
                    break;
                case "3":
                    // 상점 나가기
                    Console.WriteLine("상점을 나갑니다...");
                    ConsoleUI.PressAnyKey();
                    return;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
                    ConsoleUI.PressAnyKey();
                    break;
            }
        }
    }

    #endregion
}
