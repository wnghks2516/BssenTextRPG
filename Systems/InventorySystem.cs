using System;
using TextRPG.Models;
using TextRPG.Utils;

namespace TextRPG.Systems;

/// <summary>
/// 인벤토리 관리 시스템
/// </summary>
public class InventorySystem
{
    #region 프로퍼티
    private List<Item> Items { get; set; }
    public int Count => Items.Count;
    #endregion

    #region 생성자
    public InventorySystem()
    {
        Items = new List<Item>();
    }
    #endregion

    #region 아이템 관리
    public void AddItem(Item item)
    {
        Items.Add(item);
        Console.WriteLine($"{item.Name}을(를) 인벤토리에 추가했습니다.");
    }

    public bool RemoveItem(Item item)
    {
        if (Items.Remove(item))
        {
            Console.WriteLine($"{item.Name}을(를) 인벤토리에서 제거했습니다.");
            return true;
        }
        return false;
    }

    public Item? GetItem(int index)
    {
        if (index >= 0 && index < Items.Count)
        {
            return Items[index];
        }
        return null;
    }
    #endregion

    #region 인벤토리 표시
    public void DisplayInventory()
    {
        ConsoleUI.ShowHeader("인벤토리");

        if (Items.Count == 0)
        {
            Console.WriteLine("인벤토리가 비어 있습니다.");
            return;
        }

        DisplayItemsByType<Equipment>("[ 장비 ]");
        DisplayItemsByType<Consumable>("[ 소비 아이템 ]");
    }

    private void DisplayItemsByType<T>(string categoryName) where T : Item
    {
        var itemsOfType = Items.OfType<T>().ToList();
        
        if (itemsOfType.Count == 0) return;

        Console.WriteLine($"\n{categoryName}");
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i] is T)
            {
                Console.WriteLine($"{i + 1}. {Items[i].Name} - {Items[i].Description}");
            }
        }
    }

    public void ShowInventoryMenu(Player? player)
    {
        if (player == null)
        {
            Console.WriteLine("플레이어 정보가 없습니다.");
            return;
        }

        while (true)
        {
            DisplayInventory();
            ConsoleUI.ShowInventoryMenu();

            int choice = InputHelper.GetIntInput("\n선택: ", 0, 2);

            switch (choice)
            {
                case 1:
                    UseItem(player);
                    break;
                case 2:
                    DropItem(player);
                    break;
                case 0:
                    return;
            }
        }
    }
    #endregion

    #region 아이템 사용
    private void UseItem(Player player)
    {
        if (Items.Count == 0)
        {
            Console.WriteLine("\n인벤토리가 비어 있습니다.");
            InputHelper.PressAnyKey();
            return;
        }

        Console.Write("\n사용할 아이템 번호를 입력하세요 (0: 취소): ");
        
        if (!int.TryParse(Console.ReadLine(), out int index))
        {
            Console.WriteLine("잘못된 입력입니다.");
            InputHelper.PressAnyKey();
            return;
        }

        if (index == 0) return;

        if (index < 1 || index > Items.Count)
        {
            Console.WriteLine("잘못된 번호입니다.");
            InputHelper.PressAnyKey();
            return;
        }

        Item item = Items[index - 1];
        
        if (item.Use(player))
        {
            // 소모품일 경우 사용 후 인벤토리에서 제거
            if (item is Consumable)
            {
                RemoveItem(item);
            }
        }
        
        InputHelper.PressAnyKey();
    }
    #endregion

    #region 아이템 버리기
    private void DropItem(Player player)
    {
        if (Items.Count == 0)
        {
            Console.WriteLine("\n인벤토리가 비어 있습니다.");
            InputHelper.PressAnyKey();
            return;
        }

        Console.Write("\n버릴 아이템 번호를 입력하세요 (0: 취소): ");
        
        if (!int.TryParse(Console.ReadLine(), out int index))
        {
            Console.WriteLine("잘못된 입력입니다.");
            InputHelper.PressAnyKey();
            return;
        }

        if (index == 0) return;

        if (index < 1 || index > Items.Count)
        {
            Console.WriteLine("잘못된 번호입니다.");
            InputHelper.PressAnyKey();
            return;
        }

        Item item = Items[index - 1];

        if (!InputHelper.GetConfirmation($"\n정말 {item.Name}을(를) 버리시겠습니까?"))
        {
            Console.WriteLine("취소했습니다.");
            InputHelper.PressAnyKey();
            return;
        }

        // 장착 장비일 경우 장착 해제
        if (item is Equipment equipment)
        {
            if (player.EquippedWeapon == equipment || player.EquippedArmor == equipment)
            {
                player.UnequipItem(equipment.Slot);
            }
        }

        RemoveItem(item);
        InputHelper.PressAnyKey();
    }
    #endregion
}