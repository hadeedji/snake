using System;
using System.IO;
using System.Text.Json;

namespace Snake {
public static class Configuration {
    public static int lineWidth { get; }
    public static int cellSize { get; }
    public static int boardWidth { get; }
    public static int boardHeight { get; }

    static Configuration() {
        using (JsonDocument configurationFile = JsonDocument.Parse(File.ReadAllText("config.json"))) {
            JsonElement dimensions = configurationFile.RootElement.GetProperty("Dimensions");
            lineWidth = dimensions.GetProperty("Line").GetInt32();
            cellSize = dimensions.GetProperty("Cell").GetInt32();
            boardWidth = dimensions.GetProperty("Width").GetInt32();
            boardHeight = dimensions.GetProperty("Height").GetInt32();
        }
    }
}
}
