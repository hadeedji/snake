using System.Drawing;
using System.IO;
using System.Text.Json;
using Color = Microsoft.Xna.Framework.Color;

namespace Snake {
public class JsonConfiguration : Configuration {
    public int framesPerSecond { get; }
    public bool wallsPresent { get; }

    public Dimensions dimensions { get; }
    public Colors colors { get; }
    public Speeds speed { get; }

    public JsonConfiguration(string filePath) {
        using (JsonDocument configurationFile = JsonDocument.Parse(File.ReadAllText(filePath))) {
            framesPerSecond = configurationFile.RootElement.GetProperty("FPS").GetInt32();
            wallsPresent = configurationFile.RootElement.GetProperty("Walls").GetBoolean();

            JsonElement dimensionsElement = configurationFile.RootElement.GetProperty("Dimensions");
            dimensions = new Dimensions() {
                line = dimensionsElement.GetProperty("Line").GetInt32(),
                cell = dimensionsElement.GetProperty("Cell").GetInt32(),
                width = dimensionsElement.GetProperty("Width").GetInt32(),
                height = dimensionsElement.GetProperty("Height").GetInt32()
            };


            JsonElement colorsElement = configurationFile.RootElement.GetProperty("Colors");
            colors = new Colors() {
                background = ColorTranslator.FromHtml(colorsElement.GetProperty("Background").GetString()).ToXna(),
                wall = ColorTranslator.FromHtml(colorsElement.GetProperty("Wall").GetString()).ToXna(),
                snake = ColorTranslator.FromHtml(colorsElement.GetProperty("Snake").GetString()).ToXna(),
                apple = ColorTranslator.FromHtml(colorsElement.GetProperty("Apple").GetString()).ToXna()
            };

            JsonElement speedElement = configurationFile.RootElement.GetProperty("Speed");
            speed = new Speeds() {
                starting = speedElement.GetProperty("Starting").GetInt32(),
                maximum = speedElement.GetProperty("Max").GetInt32(),
                increment = speedElement.GetProperty("Increment").GetInt32(),
            };
        }
    }
}

static class ColorExtensions {
    public static Color ToXna(this System.Drawing.Color color) => new Color(color.R, color.G, color.B);
}
}
