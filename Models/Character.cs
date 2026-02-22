namespace TextRPG.Models;


//캐릭터 기본 추상 클래스
//추상 클래스란 인스턴스를 생성할 수 없는 클래스이며,
//다른 클래스가 상속하여 구체적인 구현을 제공해야 하는 클래스입니다.

public abstract class Character
{
    #region 프로퍼티
    //protected set; : 외부에서는 읽기만 가능하고,
    //클래스 내부에서만 값을 설정할 수 있도록 하는 접근 제한자
    public string Name { get; protected set; }
    public int CurrentHP { get; protected set; }
    public int MaxHP { get; protected set; }
    public int CurrentMP { get; protected set; }
    public int MaxMP { get; protected set; }

    public int AttackPower { get; protected set; }
    public int Defense { get; protected set; }
    public int Level { get; protected set; }
    
    // 생존여부
    public bool IsAlive => CurrentHP > 0; // 현재 HP가 0보다 크면 생존, 그렇지 않으면 사망
    #endregion

    #region 생성자
    protected Character(string name, int maxHP, int maxMP, int attackPower, int defense, int level)
    {
        Name = name;
        MaxHP = maxHP;
        CurrentHP = maxHP; // 초기 HP는 최대 HP로 설정
        MaxMP = maxMP;
        CurrentMP = maxMP; // 초기 MP는 최대 MP로 설정
        AttackPower = attackPower;
        Defense = defense;
        Level = level; // 초기 레벨은 1로 설정
    }


    #endregion

    #region 매서드


    #endregion
}
