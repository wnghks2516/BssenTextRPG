namespace TextRPG.Models;

public class  Player : Character
{

    #region 프로퍼티
    //직업
    public JobType Job { get; private set; }
    
    //골드
    public int Gold { get; private set; }
    //Todo 장착 무기 
    //Todo 장착 방어구
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
            JobType.Warrior => 40,
            JobType.Archer => 20,
            JobType.Mage => 10,
            _ => 10
        };
    #endregion

    #region 매서드
    //플레이어 정보 출력 ( 오버라이드 )
    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"골드 : {Gold}");
        Console.WriteLine($"직업 : {Job}");
    }

    public override int Attack(Character target)
    {
        int attackDamage = AttackPower;
        //todo 장착무기 또는 방어구에 따른 추가 데미지 계산

        return target.TakeDamage(attackDamage);
    }
    #endregion
}