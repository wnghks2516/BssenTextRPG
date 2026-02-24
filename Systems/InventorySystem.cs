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
    }

    public void ShowInventoryMenu()
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
                    break;
                case "2":
                    //아이템 버리기 매서드 호출
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
}