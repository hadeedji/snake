﻿using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using Color = Microsoft.Xna.Framework.Color;

namespace Snake {
public static class Configuration {
    public static int lineWidth { get; }
    public static int cellSize { get; }
    public static int boardWidth { get; }
    public static int boardHeight { get; }

    public static Color backgroundColor { get; }
    public static Color wallColor { get; }
    public static Color snakeColor { get; }
    public static Color appleColor { get; }

    static Configuration() {
        using (JsonDocument configurationFile = JsonDocument.Parse(File.ReadAllText("config.json"))) {
            JsonElement dimensions = configurationFile.RootElement.GetProperty("Dimensions");
            lineWidth = dimensions.GetProperty("Line").GetInt32();
            cellSize = dimensions.GetProperty("Cell").GetInt32();
            boardWidth = dimensions.GetProperty("Width").GetInt32();
            boardHeight = dimensions.GetProperty("Height").GetInt32();

            JsonElement colors = configurationFile.RootElement.GetProperty("Colors");
            backgroundColor = ColorTranslator.FromHtml(colors.GetProperty("Background").GetString()).ToXna();
            wallColor = ColorTranslator.FromHtml(colors.GetProperty("Wall").GetString()).ToXna();
            snakeColor = ColorTranslator.FromHtml(colors.GetProperty("Snake").GetString()).ToXna();
            appleColor = ColorTranslator.FromHtml(colors.GetProperty("Apple").GetString()).ToXna();
        }
    }

    private static Color ToXna(this System.Drawing.Color color) => new Color(color.R, color.G, color.B);
}
}
