using System;
using System.IO;
using System.Text.Json;

namespace Snake {
public static class Program {

    
    [STAThread]
    static void Main() {

        
        using (var game = new SnakeGame())
            game.Run();
    }
}
}
