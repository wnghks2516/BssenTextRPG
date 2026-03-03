using TextRPG.Models;
using TextRPG.Utils;
using TextRPG.Data;

namespace TextRPG.Systems;

/// <summary>
/// 전투 시스템을 관리하는 클래스
/// </summary>
public class BattleSystem
{
    private enum BattleAction
    {
        Attack = 1,
        Skill = 2,
        Escape = 3
    }

    #region 전투 시작
    public bool StartBattle(Player player, Enemy enemy)
    {
        ConsoleUI.ShowBattleStart();
        enemy.DisplayInfo();
        InputHelper.PressAnyKey("전투를 시작합니다...");

        int turn = 1;

        while (player.IsAlive && enemy.IsAlive)
        {
            ConsoleUI.ShowBattleTurn(turn);

            if (!ExecutePlayerTurn(player, enemy))
            {
                Console.WriteLine("\n전투에서 도망쳤습니다!");
                InputHelper.PressAnyKey();
                return false;
            }

            if (!enemy.IsAlive) break;

            ExecuteEnemyTurn(player, enemy);

            if (!player.IsAlive) break;

            turn++;
        }

        return HandleBattleResult(player, enemy);
    }
    #endregion

    #region 플레이어 턴
    private bool ExecutePlayerTurn(Player player, Enemy enemy)
    {
        ConsoleUI.ShowBattleMenu(player);
        
        BattleAction action = (BattleAction)InputHelper.GetIntInput("\n선택: ", 1, 3);

        return action switch
        {
            BattleAction.Attack => HandlePlayerAttack(player, enemy),
            BattleAction.Skill => HandlePlayerSkill(player, enemy),
            BattleAction.Escape => TryEscape(),
            _ => true
        };
    }

    private bool HandlePlayerAttack(Player player, Enemy enemy)
    {
        int damage = player.Attack(enemy);
        Console.WriteLine($"\n{player.Name}이(가) {enemy.Name}에게 {damage}의 피해를 입혔습니다!");
        ShowEnemyStatus(enemy);
        InputHelper.PressAnyKey();
        return true;
    }

    private bool HandlePlayerSkill(Player player, Enemy enemy)
    {
        if (player.CurrentMP < GameConfig.SkillMpCost)
        {
            Console.WriteLine("\nMP가 부족하여 스킬을 사용할 수 없습니다!");
            InputHelper.PressAnyKey();
            return ExecutePlayerTurn(player, enemy);
        }

        int damage = player.SkillAttack(enemy);
        Console.WriteLine($"\n{player.Name}이(가) 스킬을 사용하여 {enemy.Name}에게 {damage}의 피해를 입혔습니다!");
        Console.WriteLine($"MP: {player.CurrentMP}/{player.MaxMP}");
        ShowEnemyStatus(enemy);
        InputHelper.PressAnyKey();
        return true;
    }

    private bool TryEscape()
    {
        Random random = new();
        bool success = random.NextDouble() < GameConfig.EscapeSuccessRate;
        
        Console.WriteLine(success ? "\n도망에 성공했습니다!" : "\n도망에 실패했습니다!");
        InputHelper.PressAnyKey();
        
        return !success; // 성공하면 false(전투 종료), 실패하면 true(전투 계속)
    }

    private void ShowEnemyStatus(Enemy enemy)
    {
        Console.WriteLine($"{enemy.Name}의 남은 HP: {enemy.CurrentHP}/{enemy.MaxHP}");
    }
    #endregion

    #region 적 턴
    private void ExecuteEnemyTurn(Player player, Enemy enemy)
    {
        ConsoleUI.ShowEnemyTurn(enemy.Name);
        int damage = enemy.Attack(player);
        Console.WriteLine($"{enemy.Name}이(가) {player.Name}에게 {damage}의 피해를 입혔습니다!");
        Console.WriteLine($"{player.Name}의 남은 HP: {player.CurrentHP}/{player.MaxHP}");
        InputHelper.PressAnyKey();
    }
    #endregion

    #region 전투 결과
    private bool HandleBattleResult(Player player, Enemy enemy)
    {
        if (player.IsAlive)
        {
            ConsoleUI.ShowBattleVictory();
            Console.WriteLine($"보상: {enemy.GoldReward} 골드");
            player.GainGold(enemy.GoldReward);
            InputHelper.PressAnyKey();
            return true;
        }
        
        ConsoleUI.ShowBattleDefeat();
        InputHelper.PressAnyKey();
        return false;
    }
    #endregion
}