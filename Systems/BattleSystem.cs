using TextRPG.Models;

namespace TextRPG.Systems;

public class BattleSystem
{
    #region 던전 입장 - 전투 실행

    //전투 시작 매서드
    //반환값 : 전투 승리 여부 

    public bool StartBattle(Player player, Enemy enemy)
    {
        Console.Clear();
        Console.WriteLine("\n╔════════════════════════════════╗");
        Console.WriteLine("║       전투 시작!               ║");
        Console.WriteLine("╚════════════════════════════════╝\n");

        // 등장한 적 몬스터 정보 출력
        enemy.DisplayInfo();

        // 턴 변수 정의
        int turn = 1;

        // 전투 루프

        while (player.IsAlive && enemy.IsAlive)
        {
            Console.WriteLine($"\n║           턴 {turn} 시작!             ║");
            //플레이어 턴
            //PlayerTurn(player, enemy);

            if (!PlayerTurn(player, enemy))
            {
                //플레이어 도망
              
                return false;
            }

            // 적 사망여무 판다
            if(!enemy.IsAlive)
            {
                break;
            }

            // 적 턴
            EnemyTurn(player, enemy);

            // 플레이어 사망 여부 판단
            if(!player.IsAlive)
            {
                break;
            }

            turn++;
        }

        // 전투 결과 반환

        if (player.IsAlive)
        {
            int gainGold = enemy.GoldReward;
            Console.WriteLine("전투에서 승리했습니다!");
            Console.WriteLine($"골드 {gainGold}를 획득했습니다!");

            player.GainGold(gainGold);

            return true;
        }
        else
        {
            Console.WriteLine("전투에서 패배했습니다...");
            return false;
        }



        //Todo 적 사망여부 판단
        //Todo 적 턴


        return player.IsAlive;
    }
    #endregion





    #region 플레이어 턴
    //플레이어 행동 ( 1. 공격 2. 스킬 3. 도망 )
    private bool PlayerTurn(Player player, Enemy enemy)
    {
        Console.WriteLine("\n╔════════════════════════════════╗");
        Console.WriteLine($"║         플레이어 턴!           ║");
        Console.WriteLine("╚════════════════════════════════╝\n");
        Console.WriteLine($"HP : {player.CurrentHP}/{player.MaxHP}  MP: {player.CurrentMP}/{player.MaxMP}");
        Console.WriteLine($"Att : {player.AttackPower} , Def: {player.Defense}");
        Console.WriteLine("\n 행동을 선택하세요.");
        Console.WriteLine("1. 공격");
        Console.WriteLine("2. 스킬");
        Console.WriteLine("3. 도망");
        Console.WriteLine("");
        while (true)
        {
            Console.Write("선택: ");
            string? input = Console.ReadLine();

            switch(input)
            {
                case "1":
                    //공격 매서드 호출
                    int damage = player.Attack(enemy);
                    Console.WriteLine($"{player.Name}이 {enemy.Name}에게 {damage}의 피해를 입혔습니다.");
                    Console.WriteLine($"{enemy.Name}의 남은 HP: {enemy.CurrentHP}/{enemy.MaxHP}");
                    return true;
                case "2":
                    //스킬 매서드 호출
                    if(player.CurrentMP < 20)
                    {
                        Console.WriteLine("MP가 부족하여 스킬을 사용할 수 없습니다.");
                        continue;
                    }
                   // 스킬 발동
                    int skillDamage = player.SkillAttack(enemy);
                    Console.WriteLine($"{player.Name}이 스킬을 사용하여 {enemy.Name}에게 {skillDamage}의 피해를 입혔습니다.");
                    Console.WriteLine($"{enemy.Name}의 남은 HP: {enemy.CurrentHP}/{enemy.MaxHP}");
                    return true;
                case "3":
                    //도망 매서드 호출
                    // 50프로 확률로 성공
                    Random random = new Random();
                    if(random.NextDouble() < 0.5)
                    {
                        Console.WriteLine("도망에 성공했습니다!");
                        return false;
                    }
                    else
                    {
                        Console.WriteLine("도망에 실패했습니다!");
                        return true;
                    }
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
                    break;
            }
        }
    }

    #endregion

    #region 적 턴

    private void EnemyTurn (Player player, Enemy enemy)
    {
        Console.WriteLine($"{enemy.Name}의 턴입니다.");

        int damage = enemy.Attack(player);
        Console.WriteLine($"{enemy.Name}이 {player.Name}에게 {damage}의 피해를 입혔습니다.");
        Console.WriteLine($"{player.Name}의 남은 HP: {player.CurrentHP}/{player.MaxHP}");
    }
    #endregion
}