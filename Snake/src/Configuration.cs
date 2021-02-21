using System.Drawing;
using System.IO;
using System.Text.Json;
using Color = Microsoft.Xna.Framework.Color;

namespace Snake {
public static class Configuration {
    public static readonly int FramesPerSecond;
    public static readonly bool WallsPresent;

    public static readonly int LineWidth;
    public static readonly int CellSize;
    public static readonly int BoardWidth;
    public static readonly int BoardHeight;

    public static readonly Color BackgroundColor;
    public static readonly Color WallColor;
    public static readonly Color SnakeColor;
    public static readonly Color AppleColor;
    
    public static readonly int StartingSpeed;
    public static readonly int MaxSpeed;
    public static readonly int SpeedIncrement;

    static Configuration() {
        using (JsonDocument configurationFile = JsonDocument.Parse(File.ReadAllText("config.json"))) {
            FramesPerSecond = configurationFile.RootElement.GetProperty("FPS").GetInt32();
            WallsPresent = configurationFile.RootElement.GetProperty("Walls").GetBoolean();

            JsonElement dimensions = configurationFile.RootElement.GetProperty("Dimensions");
            LineWidth = dimensions.GetProperty("Line").GetInt32();
            CellSize = dimensions.GetProperty("Cell").GetInt32();
            BoardWidth = dimensions.GetProperty("Width").GetInt32();
            BoardHeight = dimensions.GetProperty("Height").GetInt32();

            JsonElement colors = configurationFile.RootElement.GetProperty("Colors");
            BackgroundColor = ColorTranslator.FromHtml(colors.GetProperty("Background").GetString()).ToXna();
            WallColor = ColorTranslator.FromHtml(colors.GetProperty("Wall").GetString()).ToXna();
            SnakeColor = ColorTranslator.FromHtml(colors.GetProperty("Snake").GetString()).ToXna();
            AppleColor = ColorTranslator.FromHtml(colors.GetProperty("Apple").GetString()).ToXna();

            JsonElement speed = configurationFile.RootElement.GetProperty("Speed");
            StartingSpeed = speed.GetProperty("Starting").GetInt32();
            MaxSpeed = speed.GetProperty("Max").GetInt32();
            SpeedIncrement = speed.GetProperty("Increment").GetInt32();
        }
    }
}
}
