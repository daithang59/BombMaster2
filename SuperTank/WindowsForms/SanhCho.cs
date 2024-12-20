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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace SuperTank.WindowsForms
{
    public partial class SanhCho : Form
    {
        private Graphics graphics1;
        private Bitmap background1;
        private int[,] map;
        private int level;
        private WallManagement wallManager;
        private ExplosionManagement explosionManager;
        private List<PlayerTank> playerTanks;
        private Point[] spawnPoints;
        private frmMenu formMenu;
        private Point titleClickPoint;
        private int w, h;
        private Bitmap tankSpritesheet;  // Chứa toàn bộ spritesheet
        private Rectangle[] tankSourceRects; // Chứa vị trí cắt của từng loại tank

        public SanhCho()
        {
            this.level = 1;
            InitializeComponent();
            InitializeSpawnPoints();
            playerTanks = new List<PlayerTank>();

            // Load shared path
            Common.path = Application.StartupPath + @"\Content";
            // Load spritesheet
            tankSpritesheet = new Bitmap(Path.Combine(Common.path, "Images", "tank1.png"));

            // Khởi tạo source rectangles cho 4 loại tank khác nhau
            tankSourceRects = new Rectangle[4];
            int tankWidth = tankSpritesheet.Width / 8;  // Chiều rộng mỗi tank (8 cột)
            int tankHeight = tankSpritesheet.Height / 8; // Chiều cao mỗi tank (8 hàng)

            // Chọn 4 màu tank khác nhau (ví dụ: xanh lá, đỏ, xanh dương, tím)
            tankSourceRects[0] = new Rectangle(0, 0, tankWidth, tankHeight);          // Tank xanh lá
            tankSourceRects[1] = new Rectangle(tankWidth, 0, tankWidth, tankHeight);  // Tank đỏ
            tankSourceRects[2] = new Rectangle(tankWidth * 2, 0, tankWidth, tankHeight); // Tank xanh dương
            tankSourceRects[3] = new Rectangle(tankWidth * 3, 0, tankWidth, tankHeight); // Tank tím
        }
        private void InitializeSpawnPoints()
        {
            // Define spawn points for different players
            spawnPoints = new Point[]
              {
                  new Point(Common.STEP * 2, Common.STEP * (Common.NUMBER_OBJECT_HEIGHT - 3)),  // Bottom left
                  new Point(Common.STEP * (Common.NUMBER_OBJECT_WIDTH - 3), Common.STEP * (Common.NUMBER_OBJECT_HEIGHT - 3)),  // Bottom right
                  new Point(Common.STEP * 2, Common.STEP * 2),  // Top left  
                  new Point(Common.STEP * (Common.NUMBER_OBJECT_WIDTH - 3), Common.STEP * 2)   // Top right
              };
        }
        private void SanhCho_Load(object sender, EventArgs e)
        {
            graphics1 = pnMulti.CreateGraphics();
            background1 = new Bitmap(Common.SCREEN_WIDTH, Common.SCREEN_HEIGHT);
            pnMulti.Paint += PnMulti_Paint;
            PlayerTank player = new PlayerTank();
            
            this.GameStart();
        }

        private void GameStart()
        {
            string FilePath = Common.path + @"\Maps\Map04.txt";
            map = Common.ReadMap(FilePath, Common.NUMBER_OBJECT_HEIGHT, Common.NUMBER_OBJECT_WIDTH);
            wallManager = new WallManagement();
            wallManager.CreatWall(this.map, this.level);

            // Tính toán kích thước map
            int mapWidth = Common.NUMBER_OBJECT_WIDTH * Common.STEP;
            int mapHeight = Common.NUMBER_OBJECT_HEIGHT * Common.STEP;
            pnMulti.Size = new Size(mapWidth, mapHeight);

            // Tạo background mới
            background1 = new Bitmap(mapWidth, mapHeight);

            // Xóa và tạo lại danh sách tank
            playerTanks.Clear();
            AddPlayer();

            using (Graphics g = Graphics.FromImage(background1))
            {
                // Xóa nền
                g.Clear(Color.Black);

                // Vẽ tường trước
                wallManager.ShowAllWall(background1);

                // Vẽ các tank
                foreach (var tank in playerTanks)
                {
                    if (tank != null && tank.BmpObject != null)
                    {
                        tank.Show(background1);
                    }
                }
            }

            // Yêu cầu vẽ lại
            pnMulti.Invalidate();
        }
        private void PnMulti_Paint(object sender, PaintEventArgs e)
        {
            if (background1 != null && tankSpritesheet != null)
            {
                e.Graphics.DrawImage(background1, 0, 0);

                // Vẽ tank tại các điểm spawn
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    // Vẽ từng tank với source rectangle tương ứng
                    e.Graphics.DrawImage(
                        tankSpritesheet,                    // Spritesheet gốc
                        new Rectangle(                      // Vị trí đích
                            spawnPoints[i].X,
                            spawnPoints[i].Y,
                            32,                            // Chiều rộng tank khi hiển thị
                            32                             // Chiều cao tank khi hiển thị
                        ),
                        tankSourceRects[i],                // Vùng cắt từ spritesheet
                        GraphicsUnit.Pixel
                    );
                }
            }
        }
        public void AddPlayer()
        {
            for (int i = 0; i < 4; i++)
            {
                PlayerTank newPlayer = new PlayerTank();
                newPlayer.PlayerNumber = i + 1;

                // Set vị trí theo spawnPoints
                int x = spawnPoints[i].X / Common.STEP;  // Chuyển đổi pixel sang ô lưới
                int y = spawnPoints[i].Y / Common.STEP;
                newPlayer.SetLocation(x, y);

                AssignPlayerControls(newPlayer, i);
                playerTanks.Add(newPlayer);

                // Debug check
                Console.WriteLine($"Added tank {i + 1} at position: {x}, {y}");
            }
        }

        private void AssignPlayerControls(PlayerTank player, int playerIndex)
        {
            switch (playerIndex)
            {
                case 0: // First player - Arrow keys
                    player.SetControls(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Space);  
                    break;
                case 1: // Second player - WASD
                    player.SetControls(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Q);
                    break;
                case 2: // Third player - IJKL
                    player.SetControls(Keys.I, Keys.K, Keys.J, Keys.L, Keys.U);
                    break;
                case 3: // Fourth player - Numpad
                    player.SetControls(Keys.NumPad8, Keys.NumPad5, Keys.NumPad4, Keys.NumPad6, Keys.NumPad0);
                    break;
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void ReorganizePlayers()
        {
            for (int i = 0; i < playerTanks.Count; i++)
            {
                playerTanks[i].SetLocation(spawnPoints[i].X, spawnPoints[i].Y);
                playerTanks[i].PlayerNumber = i + 1;
            }
        }
    }
}
