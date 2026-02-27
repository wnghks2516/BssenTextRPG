using System;
using TextRPG.Models;
namespace TextRPG.Systems;
using TextRPG.Utils;
public class InventorySystem
{
    #region 인벤토리 관리 매서드
    private List<Item> Items { get; set; }

    //아이템 갯수 ( 읽기 전용 )
    public int Count => Items.Count; //인벤토리에 있는 아이템 갯수 반환
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

    //아이템 삭제
    public bool RemoveItem(Item item)
    {
        if (Items.Remove(item))
        {
            Console.WriteLine($"{item.Name}을(를) 인벤토리에서 제거했습니다.");
            return true;
        }

        return false;
       
    }

    //인덱스 값으로 아이템 반환
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
        ConsoleUI.DisplayInventory();

        if(Items.Count==0)
        {
            Console.WriteLine("인벤토리가 비어 있습니다.");
            return;
        }

        Console.WriteLine("\n[보유 아이템]");
        Console.WriteLine("장비 ");
        
        for(int i = 0; i < Items.Count; i++)
        {
            if(Items[i] is Equipment)
            {
                Console.WriteLine($"{i + 1}. {Items[i].Name} - {Items[i].Description}");
            }
        }

        Console.WriteLine("소비 아이템 ");
        for(int i = 0; i < Items.Count; i++)
        {
            if(Items[i] is Consumable)
            {
                Console.WriteLine($"{i + 1}. {Items[i].Name} - {Items[i].Description}");
            }
        }
    }

    public void ShowInventoryMenu(Player? player)
    {
       while(true)
        {
            DisplayInventory();
            Console.WriteLine("\n 선택하세요.");
            Console.WriteLine("1. 아이템 사용");
            Console.WriteLine("2. 아이템 버리기");
            Console.WriteLine("0. 나가기");
            Console.Write("선택 : ");

            string? input = Console.ReadLine();

            switch(input)
            {
                case "1":
                    //아이템 사용 매서드 호출
                    UseItem(player);
                    break;
                case "2":
                    //아이템 버리기 매서드 호출
                    DropItem(player);
                    break;
                case "0":
                    return; // 나가기
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
                    break;
            }
        }
    }
    #endregion


    #region 아이템 사용 매서드
    private void UseItem(Player player)
    {
        if (Items.Count == 0)
        {
            Console.WriteLine("인벤토리가 비어 있습니다.");
            return;
        }

        Console.WriteLine("사용할 아이템 번호를 입력하세요. 0을 누르면 취소합니다.");

        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <Items.Count)
        {
            Item item = Items[index - 1];
            if(item.Use(player))
            {
                //소모품일 경우   사용 후 인벤토리에서 제거
                if(item is Consumable)
                {
                    RemoveItem(item);
                }
            }
        }
        else if( index != 0)
        {
            Console.WriteLine("잘못된 입력입니다. 숫자를 입력해주세요.");
        }
    }
    #endregion


    #region 아이템 버리기 매서드

    private void DropItem(Player player)
    {
        if (Items.Count == 0) return;
        Console.WriteLine("\n 버릴 아이템 번호를 고르세요. 0 : 취소");
        //TryParse = 문자열을 숫자로 변환 시도, 성공하면 true 반환, 실패하면 false 반환
        // Out = 변환된 숫자를 저장할 변수, TryParse가 성공하면 해당 변수에 변환된 숫자가 저장됨
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= Items.Count)
        {
            Item item = Items[index - 1];
            Console.Write($"정말 {item.Name}을 버리겠습니까? Y/N : ");
            string? input = Console.ReadLine();
            if (input != null && input.ToUpper() == "Y")
            {
                //장착 장비였을 때 장착 해제
                if (item is Equipment equipment)
                {
                    if (player.EquippedWeapon == equipment || player.EquippedArmor == equipment)
                    {
                        player.UnequipItem(equipment.Slot);
                    }
                    
                }


                RemoveItem(item);

                Console.WriteLine($"{item.Name}을(를) 버렸습니다.");
                ConsoleUI.PressAnyKey();
            }
            else
            {
                Console.WriteLine("아이템 버리기를 취소했습니다.");
                ConsoleUI.PressAnyKey();
            }

        }
        else if (index != 0)
        {
            Console.WriteLine("잘못된 입력입니다. 숫자를 입력해주세요.");
            ConsoleUI.PressAnyKey();
        }
    }
    #endregion
}