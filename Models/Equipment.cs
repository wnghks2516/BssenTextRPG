using System;
namespace TextRPG.Models;


public class Equipment : Item
{
    #region 프로퍼티
    //장착 슬롯

    public EquipmentSlot Slot { get; private set; }

    // 공격력 증가
    public int AttackBonus { get; private set; }

    //방어력 증가
    public int DefenseBonus { get; private set; }


    #endregion


    #region 생성자 
    public Equipment(
        string name,
        string description, 
        int price, 
        EquipmentSlot slot, 
        int attackBonus = 0 , 
        int defenseBonus = 0) : 
        base(name, description, price, slot == EquipmentSlot.Weapon ? ItemType.Weapon : ItemType.Armor)
    {
        Slot = slot;
        AttackBonus = attackBonus;
        DefenseBonus = defenseBonus;
    }
    #endregion
    public override bool Use(Player player)
    {
        // 장비 착용 로직

        return true;
    }

    #region 장착 아이템 생성 매서드

    //무기
    public static Equipment CreateSword(string WeaponType)
    {
       switch(WeaponType)
        {
            case "목검":
                return new Equipment("목검", "나무로 만든 검입니다.", 100, EquipmentSlot.Weapon,5);
            case "철검":
                return new Equipment("철검", "철로 만든 강력한입니다.", 500, EquipmentSlot.Weapon,10);
            case "전설검": 
                return new Equipment("전설검", "전설적인 힘을 가진 검입니다.", 1000, EquipmentSlot.Weapon,20);
            default:
                return null;

        }
    }
    //방어구
    public static Equipment CreateArmor(string ArmorType)
    {
        switch (ArmorType)
        {
            case "가죽갑옷":
                return new Equipment("가죽갑옷", "가죽으로 만든 갑옷입니다.", 100, EquipmentSlot.Armor, 0, 5);
            case "철갑옷":
                return new Equipment("철갑옷", "철로 만든 강력한 갑옷입니다.", 500, EquipmentSlot.Armor, 0, 10);
            case "전설갑옷":
                return new Equipment("전설갑옷", "전설적인 힘을 가진 갑옷입니다.", 1000, EquipmentSlot.Armor, 0, 20);
            default:
                return null;

        }
    }
    #endregion
}
