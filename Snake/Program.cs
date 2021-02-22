using System;
namespace Snake {
public static class Program {

    
    [STAThread]
    static void Main() {
        Configuration jsonConfiguration = new JsonConfiguration("config.json");
        using (var game = new MainGame(jsonConfiguration))
            game.Run();
    }
}
}
