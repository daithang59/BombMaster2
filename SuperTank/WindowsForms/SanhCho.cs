using SuperTank.General;
using SuperTank.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperTank.WindowsForms
{
    public partial class SanhCho : Form
    {
        private Graphics graphics1;
        private Bitmap background1;
        private Bitmap bufferImage; // Buffer để double buffering
        private int[,] map;
        private int level;
        private WallManagement wallManager;
        private List<PlayerTank> playerTanks;
        private Point[] spawnPoints;
        private frmMenu formMenu;
        private Bitmap tankSpritesheet;
        private Rectangle[] tankSourceRects;
        private Timer gameTimer;
        private Dictionary<PlayerTank, PlayerControls> playerControls;
        // Thêm dictionary để theo dõi trạng thái phím cho mỗi người chơi
        private Dictionary<Keys, bool> keyStates;
         // Dictionary để lưu hướng hiện tại của mỗi xe tăng
    private Dictionary<PlayerTank, Direction> currentDirections;


        // Struct để lưu các phím điều khiển cho mỗi người chơi
        private struct PlayerControls
        {
            public Keys Up, Down, Left, Right, Fire;
            public PlayerControls(Keys up, Keys down, Keys left, Keys right, Keys fire)
            {
                Up = up;
                Down = down;
                Left = left;
                Right = right;
                Fire = fire;
            }
        }

        public SanhCho()
        {
            InitializeComponent();
            InitializeSpawnPoints();
            playerTanks = new List<PlayerTank>();
            playerControls = new Dictionary<PlayerTank, PlayerControls>();
            keyStates = new Dictionary<Keys, bool>();
            currentDirections = new Dictionary<PlayerTank, Direction>();

            tankSpritesheet = new Bitmap(Path.Combine(Common.path, "Images", "tank1.png"));
            InitializeTankSourceRects();

            gameTimer = new Timer();
            gameTimer.Interval = 16;
            gameTimer.Tick += GameTimer_Tick;

            this.KeyPreview = true;
            this.KeyDown += SanhCho_KeyDown;
            this.KeyUp += SanhCho_KeyUp;
        }
        private void InitializeTankSourceRects()
        {
            tankSourceRects = new Rectangle[4];
            int tankWidth = tankSpritesheet.Width / 8;
            int tankHeight = tankSpritesheet.Height / 8;

            tankSourceRects[0] = new Rectangle(0, 0, tankWidth, tankHeight);
            tankSourceRects[1] = new Rectangle(tankWidth, 0, tankWidth, tankHeight);
            tankSourceRects[2] = new Rectangle(tankWidth * 2, 0, tankWidth, tankHeight);
            tankSourceRects[3] = new Rectangle(tankWidth * 3, 0, tankWidth, tankHeight);
        }

        private void InitializeSpawnPoints()
        {
            spawnPoints = new Point[]
            {
                new Point(Common.STEP * 2, Common.STEP * (Common.NUMBER_OBJECT_HEIGHT - 4)),
                new Point(Common.STEP * (Common.NUMBER_OBJECT_WIDTH - 4), Common.STEP * (Common.NUMBER_OBJECT_HEIGHT - 4)),
                new Point(Common.STEP * 2, Common.STEP * 2),
                new Point(Common.STEP * (Common.NUMBER_OBJECT_WIDTH - 4), Common.STEP * 2)
            };
        }

        private void InitializePlayers()
        {
            // Tạo 2 người chơi
            for (int i = 0; i < 2; i++)
            {
                PlayerTank tank = new PlayerTank();
                tank.RectX = spawnPoints[i].X;
                tank.RectY = spawnPoints[i].Y;
                tank.LoadImage(Path.Combine(Common.path, "Images", "tank1.png"));
                tank.IsActivate = true;
                playerTanks.Add(tank);
            }

            // Khởi tạo controls cho player 1
            var player1Controls = new PlayerControls(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space);
            playerControls.Add(playerTanks[0], player1Controls);

            // Thêm các phím vào keyStates cho player 1
            keyStates[Keys.W] = false;
            keyStates[Keys.S] = false;
            keyStates[Keys.A] = false;
            keyStates[Keys.D] = false;
            keyStates[Keys.Space] = false;

            // Khởi tạo controls cho player 2
            var player2Controls = new PlayerControls(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Enter);
            playerControls.Add(playerTanks[1], player2Controls);

            // Thêm các phím vào keyStates cho player 2
            keyStates[Keys.Up] = false;
            keyStates[Keys.Down] = false;
            keyStates[Keys.Left] = false;
            keyStates[Keys.Right] = false;
            keyStates[Keys.Enter] = false;
        }

        private void SanhCho_Load(object sender, EventArgs e)
        {
            graphics1 = pnMulti.CreateGraphics();
            pnMulti.Paint += PnMulti_Paint;

            // Khởi tạo buffer với kích thước panel
            bufferImage = new Bitmap(Common.SCREEN_WIDTH, Common.SCREEN_HEIGHT);

            InitializePlayers();
            GameStart();
            gameTimer.Start();
        }

        private void GameStart()
        {
            string FilePath = Common.path + @"\Maps\Map01.txt";
            map = Common.ReadMap(FilePath, Common.NUMBER_OBJECT_HEIGHT, Common.NUMBER_OBJECT_WIDTH);
            wallManager = new WallManagement();
            wallManager.CreatWall(this.map, this.level);

            int mapWidth = Common.NUMBER_OBJECT_WIDTH * Common.STEP;
            int mapHeight = Common.NUMBER_OBJECT_HEIGHT * Common.STEP;
            pnMulti.Size = new Size(mapWidth, mapHeight);

            // Vẽ background một lần duy nhất
            background1 = new Bitmap(mapWidth, mapHeight);
            using (Graphics g = Graphics.FromImage(background1))
            {
                g.Clear(Color.Black);
                wallManager.ShowAllWall(background1);
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            UpdateGame();
            RenderGame();
        }
        private void UpdateGame()
        {
            // Cập nhật trạng thái di chuyển cho từng tank dựa trên keyStates
            foreach (var pair in playerControls)
            {
                var tank = pair.Key;
                var controls = pair.Value;

                // Reset trạng thái di chuyển
                tank.Up = tank.Down = tank.Left = tank.Right = false;
                tank.IsMove = false;

                // Kiểm tra trạng thái các phím và cập nhật tank
                if (keyStates[controls.Up])
                {
                    tank.Up = true;
                    tank.IsMove = true;
                }
                else if (keyStates[controls.Down])
                {
                    tank.Down = true;
                    tank.IsMove = true;
                }
                else if (keyStates[controls.Left])
                {
                    tank.Left = true;
                    tank.IsMove = true;
                }
                else if (keyStates[controls.Right])
                {
                    tank.Right = true;
                    tank.IsMove = true;
                }

                // Cập nhật frame nếu đang di chuyển
                if (tank.IsMove)
                {
                    tank.RotateFrame();
                }

                // Xử lý di chuyển và va chạm
                if (tank.IsMove)
                {
                    int oldX = tank.RectX;
                    int oldY = tank.RectY;

                    tank.Move();

                    if (tank.IsWallCollision(wallManager.Walls, tank.DirectionTank))
                    {
                        tank.RectX = oldX;
                        tank.RectY = oldY;
                    }
                }
            }
        }

        private void RenderGame()
        {
            // Vẽ lên buffer thay vì vẽ trực tiếp
            using (Graphics g = Graphics.FromImage(bufferImage))
            {
                // Vẽ background
                g.DrawImage(background1, 0, 0);

                // Vẽ các tank và đạn
                foreach (var tank in playerTanks)
                {
                    tank.Show(bufferImage);
                    tank.ShowBulletAndMove(bufferImage);
                }
            }

            // Yêu cầu vẽ lại panel
            pnMulti.Invalidate();
        }

        private void SanhCho_KeyDown(object sender, KeyEventArgs e)
        {
            // Cập nhật trạng thái phím trong keyStates
            if (keyStates.ContainsKey(e.KeyCode))
            {
                keyStates[e.KeyCode] = true;
            }

            // Xử lý bắn đạn
            foreach (var pair in playerControls)
            {
                var tank = pair.Key;
                var controls = pair.Value;

                if (e.KeyCode == controls.Fire)
                {
                    tank.CreatBullet(@"\Images\bullet1.png", @"\Images\bullet2.png");
                }
            }
        }

        private void SanhCho_KeyUp(object sender, KeyEventArgs e)
        {
            // Cập nhật trạng thái phím trong keyStates
            if (keyStates.ContainsKey(e.KeyCode))
            {
                keyStates[e.KeyCode] = false;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Xử lý các phím mũi tên
            if (keyStates.ContainsKey(keyData))
            {
                KeyEventArgs args = new KeyEventArgs(keyData);
                SanhCho_KeyDown(this, args);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void PnMulti_Paint(object sender, PaintEventArgs e)
        {
            if (bufferImage != null)
            {
                e.Graphics.DrawImage(bufferImage, 0, 0);
            }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Bật WS_EX_COMPOSITED
                return cp;
            }
        }

    }
}