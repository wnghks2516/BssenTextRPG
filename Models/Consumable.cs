using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.Models;
public class Consumable : Item
{
    #region 프로퍼티
    //HP 회복량

    public int HpAmount { get; private set; }

    //MP 회복량
    public int MpAmount { get; private set; }


    #endregion
    public Consumable(
        string name,
        string description,
        int price,
        int hpAmount = 0,
        int mpAmount = 0) : base(name, description, price, ItemType.Potion)
    {
        HpAmount = hpAmount;
        MpAmount = mpAmount;
    }


    #region

    public override bool Use(Player player)
    {
        //플레이어의 HP/MP를 회복시키는 로직
        bool isUsed = false;

        if(HpAmount > 0)
        {
            int healedHP = player.HealHP(HpAmount);
            if(healedHP > 0)
            {
                Console.WriteLine($"{player.Name}의 HP가 {healedHP}만큼 회복되었습니다.");
                isUsed = true;
            }
            else
            {
                Console.WriteLine($"{player.Name}의 HP가 이미 최대입니다.");
            }
        }

        if(MpAmount > 0)
        {
            int healedMP = player.HealMP(MpAmount);
            if(healedMP > 0)
            {
                Console.WriteLine($"{player.Name}의 MP가 {healedMP}만큼 회복되었습니다.");
                isUsed = true;
            }
            else
            {
                Console.WriteLine($"{player.Name}의 MP가 이미 최대입니다.");
            }
        }

        return isUsed;
    }

    #endregion

    #region 포션 생성 매서드

    public static Consumable CreatePotion(string potionType) => potionType switch
    {
        "체력포션" => new Consumable("체력포션", "HP를 50회복시키는 포션입니다.", 50, hpAmount: 50),
        "대형체력포션" => new Consumable("대형체력포션", "HP를 100회복시키는 포션입니다.", 100, hpAmount: 100),
        "마나포션" => new Consumable("마나포션", "MP를 50회복시키는 포션입니다.", 50, mpAmount: 50),
        "대형마나포션" => new Consumable("대형마나포션", "MP를 100회복시키는 포션입니다.", 100, mpAmount: 100),
        _=>null!
    };
    #endregion
}