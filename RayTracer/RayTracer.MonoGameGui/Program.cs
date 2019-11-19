using System;

namespace RayTracer.MonoGameGui
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new InteractiveRayTracer())
                game.Run();
        }
    }
}
