using Engine2.core.classes;
using Engine2.core.classes.objects;
using Engine2.core.classes.objects.entities;
using Engine2.core.classes.objects.rendering;
using Engine2.core.interfaces;
using System.Runtime.Serialization;

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
            objectManager.UpdateObjects();
            System.Diagnostics.Debug.WriteLine("tick");
            Invalidate();
        }

        private void EngineInit(object sender, EventArgs e)
        {
            Camera = new();
            EInstance.New(new EEntity());
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

        }
    }
}
