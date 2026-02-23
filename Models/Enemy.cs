namespace TextRPG.Models;

public class Enemy : Character
{
    #region 프로퍼티
    public int GoldReward { get; private set; }
    #endregion



    #region 생성자
    public Enemy(string name, int maxHP, int maxMP, int attackPower, int defense, int level, int goldReward) : base(
        name: name,
        maxHP: maxHP,
        maxMP: maxMP,
        attackPower: attackPower,
        defense: defense,
        level: level
    )
    {
        GoldReward = goldReward;
    }
    #endregion

    #region 매서드

    //적 생성 매서드 ( 레벨에 따른 난이도 조절 )
    public static Enemy CreateEnemy(int playerLevel)
    {
        //난수 생성기
        Random random = new Random();

        //적 캐릭터의 레벨 ( 플레이어 레벨의 +- 1 범위 )
        int enemyLevel = Math.Max(1, playerLevel + random.Next(-1, 2)); // 최소 레벨은 1
        //적 캐릭터의 종류
        string[] enemyTypes = {"고블린", "오크", "트롤", "슬라임"};
        string enemyName = enemyTypes[random.Next(enemyTypes.Length)];

        int maxHP = 50 + (enemyLevel - 1) * 10; // 레벨에 따른 최대 HP 증가
        int maxMP = 20 + (enemyLevel - 1) * 5; // 레벨에 따른 최대 MP 증가
        int attackPower = 10 + (enemyLevel - 1) * 2; // 레벨에 따른 공격력 증가
        int defense = 5 + (enemyLevel - 1); // 레벨에 따른 방어력 증가
        int goldReward = 20 + (enemyLevel - 1) * 5; // 레벨에 따른 골드 보상 증가
        
        return new Enemy(enemyName, maxHP, maxMP, attackPower, defense, enemyLevel, goldReward);
    }
    //적 캐릭터 정보

    public override void DisplayInfo()
    {
        Console.WriteLine($"==== {Name} 정보====");
        Console.WriteLine($"레벨: {Level}");
        Console.WriteLine($"HP: {CurrentHP}/{MaxHP}");
        Console.WriteLine($"공격력: {AttackPower}");
        Console.WriteLine($"방어력: {Defense}");

    }

    
    public override int Attack(Character target)
    {
        return target.TakeDamage(AttackPower);
    }
    #endregion
}