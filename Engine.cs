using Engine2.core.classes;
using Engine2.core.classes.lookupTables;
using Engine2.core.classes.objects;
using Engine2.core.classes.objects.controller;
using Engine2.core.classes.objects.rendering;
using Engine2.Entities;
using Microsoft.VisualBasic.ApplicationServices;

namespace Engine2
{
    public partial class Engine : Form
    {
        private ObjectManager objectManager = new ObjectManager();
        private Renderer Renderer = new();
        public Camera? Camera;
        public EPlayerController PlayerController;
        public List<Keys> heldKeys = new List<Keys>();

        public Engine()
        {
            InitializeComponent();
            Application.Idle += TickEngine;
        }

        public void Init()
        {
            Camera = new Camera();
            GlobalLookup.KeyMappings.Init();
            PlayerController = new EPlayerController();
            ELocalPlayer ply = new ELocalPlayer();
            PlayerController.Posess(ply);
            Camera.SetTarget(ply);
        }

        private void TickEngine(object? sender, EventArgs e)
        {
            Frame.StartDeltaCapture();
            objectManager.UpdateObjects();
            Invalidate();
            axestxt.Text = string.Join(", ", heldKeys.ToArray());
            FPSDisplay.Text = (Frame.deltaTime).ToString();
            Thread.Sleep(8);
            Frame.EndDeltaCapture();
        }

        private void EngineInit(object sender, EventArgs e)
        {
            //TEST PURPOSES
            ECharacter char2 = new ECharacter();
            EInstance.Create<ECharacter>(new ECharacter());
        }

        private void Engine_Paint(object sender, PaintEventArgs e)
        {
            Renderer.RenderFrame(e);
        }

        private void Engine_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (!heldKeys.Contains(e.KeyCode))
            {
                heldKeys.Add(e.KeyCode);
            }
            

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

        private void Engine_KeyUp(object sender, KeyEventArgs e)
        {
            heldKeys.Remove(e.KeyCode);
        }
    }
}
