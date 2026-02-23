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

        while ( player.IsAlive && enemy.IsAlive)
        {
            //플레이어 턴
            PlayerTurn(player, enemy);
            //Todo 적 사망여부 판단
            //Todo 적 턴

            Console.WriteLine($"║       턴 {turn} 시작!             ║");
            turn++;
        }
        return player.IsAlive;
    }
    #endregion

    #region 플레이어 턴
    //플레이어 행동 ( 1. 공격 2. 스킬 3. 도망 )
    private void PlayerTurn(Player player, Enemy enemy)
    {
        Console.WriteLine($"║       플레이어 턴!             ║");
        Console.WriteLine($"HP : {player.CurrentHP}/{player.MaxHP}  MP: {player.CurrentMP}/{player.MaxMP}");
        Console.WriteLine("\n 행동을 선택하세요.");
        Console.WriteLine("1. 공격");
        Console.WriteLine("2. 스킬");
        Console.WriteLine("3. 도망");
       
        while(true)
        {
            Console.Write("선택: ");
            string? input = Console.ReadLine();

            switch(input)
            {
                case "1":
                    //공격 매서드 호출
                    Console.WriteLine("공격을 선택했습니다.");
                    break;
                case "2":
                    //스킬 매서드 호출
                    Console.WriteLine("스킬을 선택했습니다.");
                    break;
                case "3":
                    //도망 매서드 호출
                    Console.WriteLine("도망을 선택했습니다.");
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
                    continue;
            }
        }
    }

    #endregion

    #region 적 턴

    #endregion
}