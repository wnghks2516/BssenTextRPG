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
    maxHP: 100,
    maxMP: 50,
    attackPower: 20,
    defense: 10,
    level: 1
    )
    {
        Job = job;
        Gold = 1000;
    }

    #endregion
}