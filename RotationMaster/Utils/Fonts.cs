﻿using Dalamud.Interface;
using Dalamud.Interface.GameFonts;
using ImGuiNET;

namespace RotationMaster.Utils;

public static class Fonts
{
    public static GameFontHandle Axis32;
    public static GameFontHandle Axis18;
    public static GameFontHandle Jupiter23;

    public static void Init(UiBuilder uiBuilder)
    {
        Axis18 = uiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Axis18));
        Axis32 = uiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Axis36));
        Jupiter23 = uiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter23));
    }

    public static void WriteWithFont(GameFontHandle font, string text)
    {
        ImGui.PushFont(font.ImFont);
        ImGui.TextUnformatted(text);
        ImGui.PopFont();
    }
}
