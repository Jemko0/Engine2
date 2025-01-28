using Engine2.core.classes;
using Engine2.core.classes.objects;
using Engine2.core.classes.objects.rendering;
using Engine2.DataStructures;
using Engine2.Entities;

namespace Engine2
{
    public partial class Engine : Form
    {
        private core.classes.ObjectManager objectManager = new core.classes.ObjectManager();
        private Renderer Renderer = new();
        public static Camera? Camera;
        public Engine()
        {
            InitializeComponent();
            Application.Idle += TickEngine;
        }

        private void TickEngine(object? sender, EventArgs e)
        {
            Frame.StartDeltaCapture();
            objectManager.UpdateObjects();
            Invalidate();
            Frame.EndDeltaCapture();
            FPSDisplay.Text = (1 / Frame.deltaTime).ToString();
        }

        private void EngineInit(object sender, EventArgs e)
        {
            Camera = new();

            //TEST PURPOSES
            ECharacter char2 = new ECharacter();
            EInstance.Create<ECharacter>(new ECharacter());
            EInstance.Create<ECharacter>(char2);
            char2.SetLocation(new FVector(-100, 0));
            char2.Transform.Scale = new FVector(10, 10);
            char2.Velocity = new FVector(1500, 0);
        }

        private void Engine_Paint(object sender, PaintEventArgs e)
        {
            Renderer.RenderFrame(e);
        }

        private void Engine_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }

            if (e.KeyCode == Keys.Left)
            {
                Camera.position.x -= 5;
            }

            if (e.KeyCode == Keys.Right)
            {
                Camera.position.x += 5;
            }

            if (e.KeyCode == Keys.Up)
            {
                Camera.position.y += 5;
            }

            if (e.KeyCode == Keys.Down)
            {
                Camera.position.y -= 5;
            }

            if (e.KeyCode == Keys.Add)
            {
                Camera.zoom *= 1.1f;
            }

            if (e.KeyCode == Keys.Subtract)
            {
                Camera.zoom *= 0.9f;
            }
        }
    }
}
