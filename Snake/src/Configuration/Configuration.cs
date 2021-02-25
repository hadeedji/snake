using Microsoft.Xna.Framework;

namespace Snake {
public abstract class Configuration {
    public int framesPerSecond { get; init; }
    public bool wallsPresent { get; init; }

    public Dimensions dimensions { get; init; }
    public Colors colors { get; init; }
    public Speeds speed { get; init; }
}

public readonly struct Dimensions {
    public int line { get; init; }
    public int cell { get; init; }
    public int width { get; init; }
    public int height { get; init; }
}

public readonly struct Colors {
    public Color background { get; init; }
    public Color wall { get; init; }
    public Color snake { get; init; }
    public Color apple { get; init; }
}

public readonly struct Speeds {
    public int starting { get; init; }
    public int maximum { get; init; }
    public int increment { get; init; }
}
}
