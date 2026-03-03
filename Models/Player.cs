namespace TextRPG.Models;

public class Player : Character
{
    #region 프로퍼티
    public JobType Job { get; private set; }
    public int Gold { get; private set; }
    public Equipment? EquippedWeapon { get; private set; }
    public Equipment? EquippedArmor { get; private set; }

    // 계산된 프로퍼티 - 장비 보너스 포함
    public int TotalAttack => AttackPower + (EquippedWeapon?.AttackBonus ?? 0);
    public int TotalDefense => Defense + (EquippedArmor?.DefenseBonus ?? 0);
    #endregion

    #region 생성자
    public Player(string name, JobType job) : base(
        name: name,
        maxHP: GetInitHP(job),
        maxMP: GetInitMP(job),
        attackPower: GetInitAttack(job),
        defense: GetInitDefense(job),
        level: 1
    )
    {
        Job = job;
        Gold = Data.GameConfig.InitialGold;
    }
    #endregion

    #region 직업별 초기 스텟
    private static int GetInitHP(JobType job) => job switch
    {
        JobType.Warrior => 150,
        JobType.Archer => 100,
        JobType.Mage => 80,
        _ => 100
    };

    private static int GetInitMP(JobType job) => job switch
    {
        JobType.Warrior => 30,
        JobType.Archer => 70,
        JobType.Mage => 110,
        _ => 100
    };

    private static int GetInitAttack(JobType job) => job switch
    {
        JobType.Warrior => 20,
        JobType.Archer => 30,
        JobType.Mage => 40,
        _ => 10
    };

    private static int GetInitDefense(JobType job) => job switch
    {
        JobType.Warrior => 15,
        JobType.Archer => 10,
        JobType.Mage => 8,
        _ => 8
    };
    #endregion

    #region 전투 메서드
    public override int Attack(Character target)
    {
        return target.TakeDamage(TotalAttack);
    }

    public int SkillAttack(Character target)
    {
        if (CurrentMP < Data.GameConfig.SkillMpCost)
        {
            return 0;
        }

        CurrentMP -= Data.GameConfig.SkillMpCost;
        int skillDamage = (int)(TotalAttack * Data.GameConfig.SkillDamageMultiplier);
        return target.TakeDamage(skillDamage);
    }
    #endregion

    #region 골드 관리
    public void GainGold(int amount)
    {
        Gold += amount;
        Console.WriteLine($"{amount} 골드를 획득했습니다. 현재 골드: {Gold}");
    }

    public bool SpendGold(int amount)
    {
        if (Gold < amount)
        {
            Console.WriteLine("골드가 부족합니다.");
            return false;
        }

        Gold -= amount;
        return true;
    }

    /// <summary>
    /// 골드를 직접 설정합니다 (저장/불러오기용)
    /// </summary>
    internal void SetGold(int amount)
    {
        Gold = amount;
    }
    #endregion

    #region 장비 관리
    public void EquipItem(Equipment newEquipment)
    {
        Equipment? prevEquipment = newEquipment.Slot switch
        {
            EquipmentSlot.Weapon => EquippedWeapon,
            EquipmentSlot.Armor => EquippedArmor,
            _ => null
        };

        switch (newEquipment.Slot)
        {
            case EquipmentSlot.Weapon:
                EquippedWeapon = newEquipment;
                break;
            case EquipmentSlot.Armor:
                EquippedArmor = newEquipment;
                break;
        }

        if (prevEquipment != null)
        {
            Console.WriteLine($"{prevEquipment.Name}을(를) 해제하였습니다.");
        }
        Console.WriteLine($"{newEquipment.Name}을(를) 장착했습니다.");
    }

    public Equipment? UnequipItem(EquipmentSlot slot)
    {
        Equipment? equipment = slot switch
        {
            EquipmentSlot.Weapon => EquippedWeapon,
            EquipmentSlot.Armor => EquippedArmor,
            _ => null
        };

        if (equipment != null)
        {
            switch (slot)
            {
                case EquipmentSlot.Weapon:
                    EquippedWeapon = null;
                    break;
                case EquipmentSlot.Armor:
                    EquippedArmor = null;
                    break;
            }
            Console.WriteLine($"{equipment.Name}을(를) 해제했습니다.");
        }

        return equipment;
    }
    #endregion

    #region 휴식
    public void Rest()
    {
        CurrentHP = MaxHP;
        CurrentMP = MaxMP;
        Console.WriteLine("휴식을 취했습니다. HP와 MP가 완전히 회복되었습니다.");
    }
    #endregion

    #region 정보 출력
    public override void DisplayInfo()
    {
        Console.Clear();
        Console.WriteLine($"=========== {Name} 정보 ===========");
        Console.WriteLine($"레벨: {Level}");
        Console.WriteLine($"골드: {Gold} G");
        Console.WriteLine($"HP: {CurrentHP}/{MaxHP} | MP: {CurrentMP}/{MaxMP}");
        Console.WriteLine($"ATK: {AttackPower} (+{EquippedWeapon?.AttackBonus ?? 0}) | DEF: {Defense} (+{EquippedArmor?.DefenseBonus ?? 0})");
        Console.WriteLine($"\n[ 장착 중인 장비 ]");
        Console.WriteLine($"무기: {EquippedWeapon?.Name ?? "없음"} | 방어구: {EquippedArmor?.Name ?? "없음"}");
        Console.WriteLine($"직업: {Job}");
        Console.WriteLine("================================");
    }
    #endregion
}