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
    //공통으로 사용할 메소드들
    //추상 메서드 : 자식 클래스에서 반드시 구현해야 하는 매서드

    public abstract int Attack(Character target); //공격 매서드 ( 반환값 : 공격력 )

    //대미지 처리 매서드
    //가상 매서드로 구현 : 자식 클래스에서 필요에 따라 오버라이드하여 구현할 수 있는 매서드
    public virtual int TakeDamage(int damage)
    {
        //방어력 적용
        int actualDamage = Math.Max(1, damage - Defense); // 실제 대미지는 방어력을 뺀 값 ( 최소 1 )
        CurrentHP = Math.Max(0, CurrentHP - actualDamage); // HP 감소 ( 최소 1 )
        return actualDamage; // 실제로 입은 대미지 반환
    }


    //캐릭터 스텟 출력
    public virtual void DisplayInfo()
    {
        Console.Clear();
        Console.WriteLine($"==== {Name} 정보====");
        Console.WriteLine($"레벨: {Level}");
        Console.WriteLine($"HP: {CurrentHP}/{MaxHP}");
        Console.WriteLine($"MP: {CurrentMP}/{MaxMP}");
        Console.WriteLine($"공격력: {AttackPower}");
        Console.WriteLine($"방어력: {Defense}");
        Console.WriteLine($"=================");
    }

    #endregion
}
