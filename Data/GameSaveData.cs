using System;
namespace TextRPG.Data;


public class GameSaveData
{
    //Player 데이터 
    
    public PlayerData Player { get; set; }

    //인벤토리 데이터
    public List<ItemData> InventoryData { get; set; } = new List<ItemData>();
}
public class PlayerData
{
    //기본정보
    public string Name { get; set; }
    public string Job { get; set; }

    //스텟 정보
    public int Level { get; set; }
    public int MaxHP { get; set; }
    public int CurrentHP { get; set; }
    public int MaxMP { get; set; }
    public int CurrentMP { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Gold { get; set; }

    //장착 아이템 정보

    public string? EquippedWeaponName { get; set; }
    public string? EquippedArmorName { get; set; }

    //소모 아이템
 
}

public class ItemData
{
    public string ItemType { get; set; }
    public string Name { get; set; }
    public string? Slot { get; set; } // 장착 아이템인 경우 슬롯 정보 (Weapon, Armor)
    public string Disciption { get; set; }
}