using System;
using TextRPG.Models;
using TextRPG.Utils;
using TextRPG.Data;

namespace TextRPG.Systems;

/// <summary>
/// 상점 시스템
/// </summary>
public class ShopSystem
{
    #region 프로퍼티
    private List<Item> ShopItems { get; set; }
    #endregion

    #region 생성자
    public ShopSystem()
    {
        ShopItems = new List<Item>();
        InitializeShop();
    }
    #endregion

    #region 초기화
    private void InitializeShop()
    {
        // 무기
        AddShopItem(Equipment.CreateSword("목검"));
        AddShopItem(Equipment.CreateSword("철검"));
        AddShopItem(Equipment.CreateSword("전설검"));

        // 방어구
        AddShopItem(Equipment.CreateArmor("가죽갑옷"));
        AddShopItem(Equipment.CreateArmor("철갑옷"));
        AddShopItem(Equipment.CreateArmor("전설갑옷"));

        // 포션
        AddShopItem(Consumable.CreatePotion("체력포션"));
        AddShopItem(Consumable.CreatePotion("대형체력포션"));
        AddShopItem(Consumable.CreatePotion("마나포션"));
        AddShopItem(Consumable.CreatePotion("대형마나포션"));
    }

    private void AddShopItem(Item? item)
    {
        if (item != null)
        {
            ShopItems.Add(item);
        }
    }
    #endregion

    #region 상점 메뉴
    public void ShowShopMenu(Player player, InventorySystem inventory)
    {
        while (true)
        {
            ConsoleUI.ShowShopMenu(player);

            int choice = InputHelper.GetIntInput("\n선택: ", 0, 2);

            switch (choice)
            {
                case 1:
                    BuyItem(player, inventory);
                    break;
                case 2:
                    SellItem(player, inventory);
                    break;
                case 0:
                    Console.WriteLine("\n상점을 나갑니다.");
                    InputHelper.PressAnyKey();
                    return;
            }
        }
    }
    #endregion

    #region 구매
    private void BuyItem(Player player, InventorySystem inventory)
    {
        ConsoleUI.ShowShopBuyMenu(player.Gold);

        DisplayShopItems();

        Console.Write("\n구매할 아이템 번호를 선택하세요 (0: 취소): ");

        if (!int.TryParse(Console.ReadLine(), out int index))
        {
            Console.WriteLine("잘못된 입력입니다.");
            InputHelper.PressAnyKey();
            return;
        }

        if (index == 0) return;

        if (index < 1 || index > ShopItems.Count)
        {
            Console.WriteLine("잘못된 번호입니다.");
            InputHelper.PressAnyKey();
            return;
        }

        Item selectedItem = ShopItems[index - 1];

        if (player.Gold < selectedItem.Price)
        {
            Console.WriteLine("\n골드가 부족합니다!");
            InputHelper.PressAnyKey();
            return;
        }

        if (!InputHelper.GetConfirmation($"\n{selectedItem.Name}을(를) {selectedItem.Price} 골드에 구매하시겠습니까?"))
        {
            Console.WriteLine("구매를 취소했습니다.");
            InputHelper.PressAnyKey();
            return;
        }

        if (player.SpendGold(selectedItem.Price))
        {
            Item? newItem = CreateItemCopy(selectedItem);
            
            if (newItem != null)
            {
                inventory.AddItem(newItem);
                
                // 장비는 자동 장착
                if (newItem is Equipment equipment)
                {
                    player.EquipItem(equipment);
                }
                
                Console.WriteLine($"\n{selectedItem.Name}을(를) 구매했습니다!");
            }
        }

        InputHelper.PressAnyKey();
    }

    private void DisplayShopItems()
    {
        Console.WriteLine("[ 판매 중인 아이템 ]");
        for (int i = 0; i < ShopItems.Count; i++)
        {
            Item item = ShopItems[i];
            string itemInfo = GetItemInfo(item);
            Console.WriteLine($"{i + 1}. [{item.Type}] {item.Name} - {itemInfo} | {item.Price} G");
        }
    }

    private string GetItemInfo(Item item)
    {
        return item switch
        {
            Equipment eq => $"공격+{eq.AttackBonus} 방어+{eq.DefenseBonus}",
            Consumable con => $"HP+{con.HpAmount} MP+{con.MpAmount}",
            _ => item.Description
        };
    }
    #endregion

    #region 판매
    private void SellItem(Player player, InventorySystem inventory)
    {
        if (inventory.Count == 0)
        {
            Console.WriteLine("\n판매할 아이템이 없습니다.");
            InputHelper.PressAnyKey();
            return;
        }

        ConsoleUI.ShowShopSellMenu();
        inventory.DisplayInventory();

        Console.Write("\n판매할 아이템 번호를 선택하세요 (0: 취소): ");

        if (!int.TryParse(Console.ReadLine(), out int index))
        {
            Console.WriteLine("잘못된 입력입니다.");
            InputHelper.PressAnyKey();
            return;
        }

        if (index == 0) return;

        if (index < 1 || index > inventory.Count)
        {
            Console.WriteLine("잘못된 번호입니다.");
            InputHelper.PressAnyKey();
            return;
        }

        Item? item = inventory.GetItem(index - 1);

        if (item == null)
        {
            Console.WriteLine("아이템을 찾을 수 없습니다.");
            InputHelper.PressAnyKey();
            return;
        }

        int sellPrice = (int)(item.Price * GameConfig.SellPriceRate);

        if (!InputHelper.GetConfirmation($"\n{item.Name}을(를) {sellPrice} 골드에 판매하시겠습니까?"))
        {
            Console.WriteLine("판매를 취소했습니다.");
            InputHelper.PressAnyKey();
            return;
        }

        // 장착 해제
        if (item is Equipment equipment)
        {
            if (player.EquippedWeapon == equipment || player.EquippedArmor == equipment)
            {
                player.UnequipItem(equipment.Slot);
            }
        }

        inventory.RemoveItem(item);
        player.GainGold(sellPrice);

        Console.WriteLine($"\n{item.Name}을(를) 판매했습니다!");
        InputHelper.PressAnyKey();
    }
    #endregion

    #region 아이템 복제
    private Item? CreateItemCopy(Item item)
    {
        return item switch
        {
            Equipment equipment => new Equipment(
                equipment.Name,
                equipment.Description,
                equipment.Price,
                equipment.Slot,
                equipment.AttackBonus,
                equipment.DefenseBonus
            ),
            Consumable consumable => new Consumable(
                consumable.Name,
                consumable.Description,
                consumable.Price,
                consumable.HpAmount,
                consumable.MpAmount
            ),
            _ => null
        };
    }
    #endregion
}
