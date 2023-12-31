using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dalamud.Plugin.Services;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace RotationMaster.Data;

public enum RotationMasterActionType
{
    AutoAttack,
    GCD,
    OGCD,
    Item,
    NA
}

public static class FFAction
{
    private static Dictionary<uint, Action> actionsById = new();

    public static Action ById(uint id)
    {
        TryInit();

        return actionsById[id];
    }

    public static IntPtr GetIconHandle(uint actionId, bool highRez = false)
    {
        var flags = highRez
                        ? ITextureProvider.IconFlags.HiRes
                        : ITextureProvider.IconFlags.None;

        return Plugin.TextureProvider.GetIcon(ById(actionId).Icon, flags)!.ImGuiHandle;
    }

    public static bool TryById(uint id, out Action action)
    {
        action = null;

        TryInit();

        if (actionsById.TryGetValue(id, out var a))
        {
            action = a;
            return true;
        }

        return false;
    }

    public static bool Exists(uint id)
    {
        return actionsById.ContainsKey(id);
    }

    private static void TryInit()
    {
        if (actionsById.Count == 0)
        {
            actionsById = Plugin.DataManager.GetExcelSheet<Action>().ToDictionary(a => a.RowId);
        }
    }

    public static string[] SplitActionName(this Action action)
    {
        var split = action.Name.RawString.Split(" ").SelectMany(part =>
        {
            const int max = 8;
            const int sliceAmount = max - 1;
            if (part.Length <= max)
            {
                return new List<string>() { part };
            }

            return new List<string>() { $"{part[..(sliceAmount)]}-", part[(sliceAmount)..] };
        }).ToArray();

        return split;
    }

    public static RotationMasterActionType GetActionType(this Action action) =>
        action.ActionCategory?.Row switch
        {
            1 => RotationMasterActionType.AutoAttack,
            2 or 3 => RotationMasterActionType.GCD,
            4 => RotationMasterActionType.OGCD,
            _ => RotationMasterActionType.NA
        };
}
