using System;
using System.IO;
using System.Text.Json;

namespace Snake {
public static class GameConfig {
    private static ConfigValues _configValues;

    public static int lineWidth => _configValues.lineWidth;
    public static int boxWidth => _configValues.boxWidth;
    public static int gameWidth => _configValues.gameWidth;
    public static int gameHeight => _configValues.gameHeight;


    static GameConfig() {
        string jsonString = File.ReadAllText("GameConfig.json");
        _configValues = JsonSerializer.Deserialize<ConfigValues>(jsonString, new JsonSerializerOptions() {
            PropertyNameCaseInsensitive = true
        });
    }

    private class ConfigValues {
        public int lineWidth { get; set; }
        public int boxWidth { get; set; }
        public int gameWidth { get; set; }
        public int gameHeight { get; set; }
    }
}
}
