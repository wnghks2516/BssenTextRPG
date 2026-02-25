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
        ShopItems.Add(Equipment.CreateArmor("가죽갑옷"));
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
                    BuyItem(player,inventory);
                    break;
                case "2":
                    // 판매
                    SellItem(player, inventory);
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

    #region 구매 매서드
    private void BuyItem(Player player, InventorySystem inventory)
    {
        Console.Clear();
        Console.WriteLine("\n구매할 아이템을 선택하세요 : ");

        for (int i = 0; i < ShopItems.Count; i++)
        {
            Item item = ShopItems[i];
            Console.WriteLine($"{i + 1}. [ {item.Type} ] {item.Name} - 가격: {item.Price} 골드");
        }

        Console.WriteLine("\n구매할 아이템 번호를 선택하세요. 0 : 취소");

        if (int.TryParse(Console.ReadLine(), out var index) && index > 0 && index <= ShopItems.Count)
        {
            Item selectedItem = ShopItems[index - 1];

            //골드가 충분한지 확인

            if (player.Gold >= selectedItem.Price)
            {
                //골드 차감
                Console.WriteLine($"{selectedItem.Name}을(를) {selectedItem.Price} 골드로 구매하시겠습니까? Y/N : ");
                if (Console.ReadLine()?.ToUpper() == "Y")
                {
                    player.SpendGold(selectedItem.Price);
                    
                    //구매한 아이템의 인스턴스 생성 ( 복제 )
                    Item? item = CreateItem(selectedItem);
                    if (item is Equipment equipment)
                    {
                        inventory.AddItem(equipment);
                        player.EquipItem(equipment);
                    }
                    else if (item is Consumable consumable)
                    {
                        inventory.AddItem(consumable);
                    }
                    Console.WriteLine($"{selectedItem.Name}을(를) 구매했습니다.");
                    ConsoleUI.PressAnyKey();
                }
                else
                {
                    Console.WriteLine("구매가 취소되었습니다.");
                    ConsoleUI.PressAnyKey();
                }
            }
            else
            {
                Console.WriteLine("골드가 부족합니다.");
            }
        }
    }
    #endregion

    #region 아이템 복제 매서드
    private Item? CreateItem ( Item item )
    {
        //장착 아이템 
        if ( item is Equipment equipment )
        {
            var newItem = new Equipment(
                equipment.Name,
                equipment.Description,
                equipment.Price,
                equipment.Slot,
                equipment.AttackBonus,
                equipment.DefenseBonus
                );
            return newItem;
        }

        //소모성 아이템
        else if ( item is Consumable consumable )
        {
            var newItem = new Consumable(
                consumable.Name,
                consumable.Description,
                consumable.Price,
                consumable.HpAmount,
                consumable.MpAmount
                );
            return newItem;
        }
        return null;
    }

    #endregion

    #region 아이템 판매 매서드
    private void SellItem ( Player player, InventorySystem inventory )
    {
        if ( inventory.Count == 0 )
        {
            Console.WriteLine("판매할 아이템이 없습니다.");
            ConsoleUI.PressAnyKey();
            return;
        }

        inventory.DisplayInventory();
        Console.WriteLine("\n판매할 아이템 번호를 선택하세요. 0 : 취소");
        Console.Write("선택 : ");

        if(int.TryParse(Console.ReadLine(), out var index) && index > 0 && index <= inventory.Count)
        {
            // 인벤토리에서 아이템 추출
            Item? item = inventory.GetItem(index - 1);

            if (item != null)
            {
                //판매 가격은 구매 가격의 절반으로 설정
                int sellPrice = item.Price / 2;

                Console.WriteLine($"{item.Name}을(를) {sellPrice} 골드에 판매하시겠습니까? Y/N : ");
                if (Console.ReadLine()?.ToUpper() == "Y")
                {
                    //아이템 인벤토리에서 제거 및 골드 증가
                    inventory.RemoveItem(item);
                    player.GainGold(sellPrice);

                    //장착 해제
                    if (item is Equipment equipment)
                    {
                        player.UnequipItem(equipment.Slot);
                    }
                    Console.WriteLine($"{item.Name}을(를) 판매했습니다.");
                }
            }

        }
    }
    #endregion
}
