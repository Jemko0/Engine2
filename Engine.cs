using Engine2.core.classes;
using Engine2.core.classes.lookupTables;
using Engine2.Object;
using Engine2.Rendering;
using Engine2.Entities;
using Engine2.DataStructures;
using Engine2.World;

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
            GlobalLookup.InitAll();
            PlayerController = new EPlayerController();
            ELocalPlayer ply = new ELocalPlayer();
            PlayerController.Posess(ply);
            Camera.SetTarget(ply);
            ObjectManager.CreateWorld(new IVector(100, 4));
        }

        private void TickEngine(object? sender, EventArgs e)
        {
            Frame.StartDeltaCapture();
            objectManager.UpdateObjects();
            Invalidate();
#if !RELEASE
            Thread.Sleep(12);
#endif
            UpdateDebugText();
            Frame.EndDeltaCapture();
        }

        private void UpdateDebugText()
        {
            Captures.Text = "";
            foreach(var c in Frame.captures)
            {
                if(c.Key != "engineLoop")
                {
                    Captures.Text += c.Key + " | " + Frame.GetCapture(c.Key) + "\n";
                }
                
            }
            
            FPSDisplay.Text = "Delta: " + Frame.deltaTime.ToString() + "\n" + "FPS: " + ((int)Frame.fps).ToString();
        }

        private void EngineInit(object sender, EventArgs e)
        {
            //TEST PURPOSES
            ECharacter char2 = EInstance.Create<ECharacter>(new ECharacter());
            char2.brush = new SolidBrush(Color.HotPink);
            char2.Transform.Scale = new DataStructures.FVector(500, 40);
            char2.Transform.Translation = new DataStructures.FVector(-100, 10);

            ECharacter char3 = EInstance.Create<ECharacter>(new ECharacter());
            char3.Transform.Scale = new DataStructures.FVector(50, 50);
            char3.Transform.Translation = new DataStructures.FVector(0, 50);
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
