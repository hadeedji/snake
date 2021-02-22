using Microsoft.Xna.Framework;

namespace Snake {
public interface Configuration {
    public int framesPerSecond { get; }
    public bool wallsPresent { get; }

    public Dimensions dimensions { get; }
    public Colors colors { get; }
    public Speeds speed { get; }
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
