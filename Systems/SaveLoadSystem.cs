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

}