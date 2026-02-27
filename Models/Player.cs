namespace TextRPG.Models;

public class  Player : Character
{

    #region 프로퍼티
    //직업
    public JobType Job { get; private set; }
    
    //골드
    public int Gold { get; private set; }
    // 장착 무기 
    public Equipment? EquippedWeapon { get; private set; }

    // 장착 방어구
    public Equipment? EquippedArmor { get; private set; }
    #endregion

    #region 생성자
    public Player(string name, JobType job, int gold) : base(
        name: name,
        maxHP: GetInitHP(job),
        maxMP: GetInitMP(job),
        attackPower: GetInitAttack(job),
        defense: GetInitDefense(job),
        level: 1
    )
        {
            Job = job;
            Gold = 1000;
        }

    #endregion

    #region 직업별 초기 스텟

    //체력
    private static int GetInitHP(JobType job)
    {
        switch(job)
        {
            case JobType.Warrior: return 150;
            case JobType.Archer: return 100;
            case JobType.Mage: return 80;
            default: return 100;
        }
    }

    private static int GetInitMP(JobType job)
    {
        switch (job)
        {
            case JobType.Warrior: return 30;
            case JobType.Archer: return 70;
            case JobType.Mage: return 110;
            default: return 100;
        }
    }

    private static int GetInitAttack(JobType job) =>
        job switch
        {
            JobType.Warrior => 20,
            JobType.Archer => 30,
            JobType.Mage => 40,
            _ => 10
        };

    private static int GetInitDefense(JobType job) =>
        job switch
        {
            JobType.Warrior => 15,
            JobType.Archer => 10,
            JobType.Mage => 8,
            _ => 8
        };
    #endregion

    #region 매서드
    //플레이어 정보 출력 ( 오버라이드 )
    public override void DisplayInfo()
    {
        //base.DisplayInfo();
        Console.Clear();
        Console.WriteLine($"=========== {Name} 정보 ===========");
        Console.WriteLine($"레벨 : {Level}");
        Console.WriteLine($"골드 : {Gold}");
        Console.WriteLine($"HP : {CurrentHP}/{MaxHP} | MP : {CurrentMP}/{MaxMP}");

        int attackBonus = EquippedWeapon != null ? EquippedWeapon.AttackBonus : 0;
        int defenseBonus = EquippedArmor != null ? EquippedArmor.DefenseBonus : 0;

        Console.WriteLine($"ATK : {AttackPower} (+{attackBonus}) | DEF : {Defense} (+{defenseBonus})");
        //장착 아이템 정보

        Console.WriteLine($"[ 장착 중인 장비 ] \n" +
            $"무기 : {(EquippedWeapon != null ? EquippedWeapon.Name : "없음")} | " +
            $"방어구 : {(EquippedArmor != null ? EquippedArmor.Name : "없음")}");

        Console.WriteLine($"직업 : {Job}");

       

    }
    #endregion

    #region 기본공격 매서드
    public override int Attack(Character target)
    {

        //장착무기 또는 방어구에 따른 추가 데미지 계산
        int attackDamage = AttackPower;

        // null 병합 연산자 : ??
        // int? a 란 int형 변수 a가 null이 될 수 있음을 나타냄.
        //int? a = null;

        attackDamage += EquippedWeapon?.AttackBonus ?? 0; // 무기 공격력 보너스
        // 아래의 코드가 위의 코드와 동일한 기능을 수행
        //if ( EquippedArmor != null)
        //{
        //    attackDamage += EquippedArmor.DefenseBonus; // 방어구 공격력 보너스
        //}

        return target.TakeDamage(attackDamage);
    }
    #endregion

    #region 마법공격 Player 전용 매서드

    public int SkillAttack ( Character target )
    {
        int mpCost = 20; //스킬 사용에 필요한 MP
        // 스킬 공격은 기본공격의 1.5배 대미지를 주지만 마나를 소모

        int totalDamage = (int)(AttackPower * 1.5);
        totalDamage += EquippedWeapon?.AttackBonus ?? 0; // 무기 공격력 보너스

        Console.WriteLine($"마법 공격력 : {totalDamage}");
        if (CurrentMP < mpCost)
        {
            Console.WriteLine("MP가 부족하여 스킬을 사용할 수 없습니다.");
            return 0;
        }
        CurrentMP -= mpCost;
        //대미지 전달
        target.TakeDamage(totalDamage);
        return CurrentMP;
    }
    #endregion


    #region 골드 획득 매서드
    public void GainGold(int amount)
    {
        Gold += amount;
        Console.WriteLine($"{amount} 골드를 획득했습니다. 현재 골드: {Gold}");
    }
    #endregion

    //장비 착용 매서드 
    public void EquipItem(Equipment newEquipment)
    {
        Equipment? prevEquipment = null;

        switch (newEquipment.Slot)
        {
            case EquipmentSlot.Weapon:
                prevEquipment = EquippedWeapon;
                EquippedWeapon = newEquipment;
                break;
            case EquipmentSlot.Armor:
                prevEquipment = EquippedArmor;
                EquippedArmor = newEquipment;
                break;
        }

        //이전 장비 해제 메시지
        if (prevEquipment != null)
        {
            Console.WriteLine($"{prevEquipment.Name}을(를) 해제하였습니다.");
        }
        Console.WriteLine($"{newEquipment.Name}을(를) 장착했습니다.");
    }
    //장비 해제
    public Equipment? UnequipItem(EquipmentSlot slot)
    {
     Equipment? equipment = null;
        switch (slot)
        {
            case EquipmentSlot.Weapon:
                equipment = EquippedWeapon;
                EquippedWeapon = null;
                break;

            case EquipmentSlot.Armor:
                equipment = EquippedArmor;
                EquippedArmor = null;
                break;
            default:
                Console.WriteLine("잘못된 장비 슬롯입니다.");
                break;
        }
        if(equipment != null)
        {
            Console.WriteLine($"{equipment.Name}을(를) 해제했습니다.");
        }
        return equipment;
    }

    #region 골드 사용 매서드
    public void SpendGold(int amount)
    {
        if (amount > Gold)
        {
            Console.WriteLine("골드가 부족합니다.");
            return;
        }
        Gold -= amount;
        Console.WriteLine($"{amount} 골드를 사용했습니다. 현재 골드: {Gold}");
    }
    #endregion

    #region 휴식 매서드

    public void Rest(int cost)
    {
        Gold -= cost;
        if ( CurrentHP == MaxHP && CurrentMP == MaxMP)
        {
            Console.WriteLine("이미 HP와 MP가 모두 회복된 상태입니다.");
            return;
        }
        CurrentHP = MaxHP;
        CurrentMP = MaxMP;

    }
    #endregion
}