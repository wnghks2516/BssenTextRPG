namespace TextRPG.Models;

/// <summary>
/// 캐릭터 기본 추상 클래스
/// 플레이어와 적이 공통으로 사용하는 속성과 메서드를 정의
/// </summary>
public abstract class Character
{
    #region 프로퍼티
    public string Name { get; set; }
    public int CurrentHP { get; set; }
    public int MaxHP { get; set; }
    public int CurrentMP { get; set; }
    public int MaxMP { get; set; }
    public int AttackPower { get; set; }
    public int Defense { get; set; }
    public int Level { get; set; }

    /// <summary>
    /// 생존 여부 (CurrentHP > 0)
    /// </summary>
    public bool IsAlive => CurrentHP > 0;
    #endregion

    #region 생성자
    protected Character(string name, int maxHP, int maxMP, int attackPower, int defense, int level)
    {
        Name = name;
        MaxHP = maxHP;
        CurrentHP = maxHP;
        MaxMP = maxMP;
        CurrentMP = maxMP;
        AttackPower = attackPower;
        Defense = defense;
        Level = level;
    }
    #endregion

    #region 추상 메서드
    /// <summary>
    /// 공격 메서드 (자식 클래스에서 구현 필요)
    /// </summary>
    public abstract int Attack(Character target);
    #endregion

    #region 전투 메서드
    /// <summary>
    /// 데미지를 받습니다
    /// </summary>
    /// <param name="damage">받을 데미지</param>
    /// <returns>실제로 입은 데미지</returns>
    public virtual int TakeDamage(int damage)
    {
        int actualDamage = Math.Max(1, damage - Defense);
        CurrentHP = Math.Max(0, CurrentHP - actualDamage);
        return actualDamage;
    }
    #endregion

    #region 회복 메서드
    /// <summary>
    /// HP를 회복합니다
    /// </summary>
    /// <param name="amount">회복량</param>
    /// <returns>실제로 회복된 HP</returns>
    public int HealHP(int amount)
    {
        int beforeHP = CurrentHP;
        CurrentHP = Math.Min(MaxHP, CurrentHP + amount);
        return CurrentHP - beforeHP;
    }

    /// <summary>
    /// MP를 회복합니다
    /// </summary>
    /// <param name="amount">회복량</param>
    /// <returns>실제로 회복된 MP</returns>
    public int HealMP(int amount)
    {
        int beforeMP = CurrentMP;
        CurrentMP = Math.Min(MaxMP, CurrentMP + amount);
        return CurrentMP - beforeMP;
    }
    #endregion

    #region 정보 표시
    /// <summary>
    /// 캐릭터 정보를 출력합니다
    /// </summary>
    public virtual void DisplayInfo()
    {
        Console.Clear();
        Console.WriteLine($"==== {Name} 정보 ====");
        Console.WriteLine($"레벨: {Level}");
        Console.WriteLine($"HP: {CurrentHP}/{MaxHP}");
        Console.WriteLine($"MP: {CurrentMP}/{MaxMP}");
        Console.WriteLine($"공격력: {AttackPower}");
        Console.WriteLine($"방어력: {Defense}");
        Console.WriteLine("===================");
    }
    #endregion
}
