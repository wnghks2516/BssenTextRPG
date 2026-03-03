using System;
using System.Text.Json;
using TextRPG.Models;
using TextRPG.Data;
using System.Text.Encodings.Web;
using TextRPG.Systems;

/// <summary>
/// 게임 저장 및 불러오기 시스템
/// </summary>
public static class SaveLoadSystem
{
    #region JSON 옵션
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        IncludeFields = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
    #endregion

    #region 파일 체크
    public static bool IsSaveFileExists()
    {
        return File.Exists(GameConfig.SaveFilePath);
    }
    #endregion

    #region 저장
    public static bool SaveGame(Player player, InventorySystem inventory)
    {
        try
        {
            var saveData = new GameSaveData
            {
                Player = ConvertToPlayerData(player),
                InventoryData = ConvertToInventoryData(inventory)
            };

            string jsonString = JsonSerializer.Serialize(saveData, JsonOptions);
            File.WriteAllText(GameConfig.SaveFilePath, jsonString);
            
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"게임 저장 중 오류 발생: {e.Message}");
            return false;
        }
    }

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
            Gold = player.Gold,
            EquippedWeaponName = player.EquippedWeapon?.Name,
            EquippedArmorName = player.EquippedArmor?.Name
        };
    }

    private static List<ItemData> ConvertToInventoryData(InventorySystem inventory)
    {
        var itemDataList = new List<ItemData>();
        
        for (int i = 0; i < inventory.Count; i++)
        {
            var item = inventory.GetItem(i);
            if (item == null) continue;

            var itemData = new ItemData
            {
                Name = item.Name,
                Disciption = item.Description
            };

            switch (item)
            {
                case Equipment equipment:
                    itemData.ItemType = "Equipment";
                    itemData.Slot = equipment.Slot.ToString();
                    break;
                case Consumable:
                    itemData.ItemType = "Consumable";
                    break;
            }

            itemDataList.Add(itemData);
        }

        return itemDataList;
    }
    #endregion

    #region 불러오기
    public static GameSaveData? LoadGame()
    {
        try
        {
            if (!IsSaveFileExists())
            {
                Console.WriteLine("저장된 게임 파일이 없습니다.");
                return null;
            }

            string jsonString = File.ReadAllText(GameConfig.SaveFilePath);
            var saveData = JsonSerializer.Deserialize<GameSaveData>(jsonString, JsonOptions);
            
            return saveData;
        }
        catch (Exception e)
        {
            Console.WriteLine($"게임 불러오기 중 오류 발생: {e.Message}");
            return null;
        }
    }

    public static Player LoadPlayer(PlayerData data)
    {
        if (!Enum.TryParse<JobType>(data.Job, out var job))
        {
            job = JobType.Warrior; // 기본값
        }

        var player = new Player(data.Name, job)
        {
            Level = data.Level,
            CurrentHP = data.CurrentHP,
            MaxHP = data.MaxHP,
            CurrentMP = data.CurrentMP,
            MaxMP = data.MaxMP
        };

        player.SetGold(data.Gold);

        return player;
    }

    public static InventorySystem LoadInventory(List<ItemData> itemDataList, Player player)
    {
        var inventory = new InventorySystem();

        foreach (var itemData in itemDataList)
        {
            Item? item = CreateItemFromData(itemData);
            
            if (item != null)
            {
                inventory.AddItem(item);
            }
        }

        return inventory;
    }

    private static Item? CreateItemFromData(ItemData itemData)
    {
        return itemData.ItemType switch
        {
            "Equipment" => CreateEquipmentFromData(itemData),
            "Consumable" => Consumable.CreatePotion(itemData.Name),
            _ => null
        };
    }

    private static Equipment? CreateEquipmentFromData(ItemData itemData)
    {
        if (string.IsNullOrEmpty(itemData.Slot)) return null;

        if (!Enum.TryParse<EquipmentSlot>(itemData.Slot, out var slot))
        {
            return null;
        }

        return slot switch
        {
            EquipmentSlot.Weapon => Equipment.CreateSword(itemData.Name),
            EquipmentSlot.Armor => Equipment.CreateArmor(itemData.Name),
            _ => null
        };
    }

    public static void LoadEquippedItems(Player player, PlayerData data, InventorySystem inventory)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            var item = inventory.GetItem(i);
            
            if (item is not Equipment equipment) continue;

            bool shouldEquip = equipment.Slot switch
            {
                EquipmentSlot.Weapon => equipment.Name == data.EquippedWeaponName,
                EquipmentSlot.Armor => equipment.Name == data.EquippedArmorName,
                _ => false
            };

            if (shouldEquip)
            {
                player.EquipItem(equipment);
            }
        }
    }
    #endregion
}