using System;
using System.Text.Json;
using TextRPG.Models;
using TextRPG.Data;
using System.Text.Encodings.Web;
using TextRPG.Systems;

public class SaveLoadSystem
{
    // 게임 저장 및 불러오기 시스템

    private const string SaveFilePath = "savegame.json";

    //Json Serializer 옵션
    //Serializer란 객체를 JSON 문자열로 변환하거나 JSON 문자열을 객체로 변환하는 역할을 하는 도구입니다.


    //
    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        WriteIndented = true, // JSON 문자열을 읽기 쉽게 들여쓰기
        IncludeFields = true, // 필드도 직렬화/역직렬화에 포함
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    #region 저장
    public static bool SaveGame(Player player, InventorySystem inventory)
    {
        try
        {
            // 1. 게임 객체 생성 클래스 -> DTO(Data Transfer Object)로 변환
            var saveData = new GameSaveData
            {
                Player = ConvertToPlayerData(player),
                InventoryData = ConvertToItemData(inventory),

            };

            // 2. DTO 객체 => JSON 문자열로 직렬화
            string jsonString = JsonSerializer.Serialize(saveData, jsonOptions);

            // 3. JSON 문자열을 파일로 저장
            File.WriteAllText(SaveFilePath, jsonString);
            return true;

        }
        catch (Exception e)
        {
            Console.WriteLine($"게임 저장 중 오류 발생: {e.Message}");
            return false;
        }
    }

    // Player->PlayerData로 변환
    private static PlayerData ConvertToPlayerData(Player player)
    {
        return new PlayerData
        {
            Name = player.Name,
            Job = player.Job.ToString(),
            Level = player.Level,
            CurrentHP = player.CurrentHP,
            MaxHP = player.MaxHP,
            CurrentMP = player.CurrentMP,
            MaxMP = player.MaxMP,
            Attack = player.AttackPower,
            Defense = player.Defense,
            EquippedWeaponName = player.EquippedWeapon?.Name,
            EquippedArmorName = player.EquippedArmor?.Name,
            Gold = player.Gold
        };
    }
    // Inventory -> ItemData로 변환
    private static List<ItemData> ConvertToItemData(InventorySystem inventory)
    { 
        var itemDataList = new List<ItemData>();
        for (int i = 0; i < inventory.Count; i++)
        {
            var item = inventory.GetItem(i);
            if (item == null) continue;
            var itemData = new ItemData
            {
                Name = item.Name,
            };

            if ( item is Equipment equipment)
            {
                itemData.ItemType = "Equipment";
                itemData.Slot =equipment.Slot.ToString();
               
            }
            else if ( item is Consumable consumable)
            {
                itemData.ItemType = "Consumable";
            }
            itemData.Disciption = item.Description;
                itemDataList.Add(itemData);

        }
        return itemDataList;
    }

    #endregion


    #region 게임 불러오기

    //저장된 파일 체크
    public static bool IsSaveFileExists()
    {
        return File.Exists(SaveFilePath);
    }

    public static GameSaveData? LoadGame()
    {
        try
        {
            // 1. Json 파일 읽어오기
            string jsonString = File.ReadAllText(SaveFilePath);
            Console.WriteLine("저장된 게임 데이터 확인 : ");
            Console.WriteLine(jsonString);

            // 2. Json 문자열 -> DTO 변환 
            var saveData = JsonSerializer.Deserialize<GameSaveData>(jsonString, jsonOptions);
            Console.WriteLine("게임 데이터 역직렬화 완료 ");
            return saveData;
        }
        catch (Exception e)
        {
            Console.WriteLine($"게임 불러오기 중 오류 발생: {e.Message}");
            return null;
        }
    }

    //PlayerData DTO -> Player 객체로 변환

    public static Player LoadPlayer(PlayerData data)
    {
        // JobType을 문자열로 저장했기 때문에 , Enum으로 변환하는 작업
        var job = Enum.Parse<JobType>(data.Job);

        //Player 객체 생성
        var player = new Player(data.Name, job);

        player.Level = data.Level;
        player.CurrentHP = data.CurrentHP;
        player.MaxHP = data.MaxHP;
        player.CurrentMP = data.CurrentMP;
        player.MaxMP = data.MaxMP;
        player.Gold = data.Gold;

        return player;

    }

    //ItemData DTO -> Item 객체로 변환

    //저장된 장착 아이템을 복원 매서드 ( 무기 / 방어구 )

    //아이템 생성 -> Inventory추가

    #endregion
}