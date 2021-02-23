using System;

namespace Snake {
public static class Program {
    public static Configuration configuration { get; private set; }
    
    [STAThread]
    static void Main() {
        configuration = new JsonConfiguration("config.json");
        using (var game = new MainGame())
            game.Run();
    }
}
}
