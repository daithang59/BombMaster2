using SuperTank.General;
using SuperTank.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperTank
{
    public partial class Multi_Mode : Form
    {
        #region Đồ họa graphics
        private Graphics graphics;
        #endregion Đồ họa graphics

        #region các thuộc tính chung
        private Bitmap background;
        private Bitmap bmpCastle;
        private int[,] map;
        #endregion các thuộc tính chung

        #region Đối tượng
        private WallManagement wallManager;
        private ExplosionManagement explosionManager;
        private PlayerTank playerTank;
        private EnemyTankManagement enemyTankManager;
        private Item item;
        #endregion Đối tượng

        #region thuộc tính thông tin
        private PictureBox[] picNumberEnemyTanks;
        private int level;
        private int scores;
        private int killed;
        private InforStyle inforStyle;
        #endregion thuộc tính thông tin

        #region thuộc tính thời gian
        private int timeItem = 50;
        private int timeItemActive = 15;
        private bool isTimeItemActive;
        private int time_delay = 0;
        #endregion thuộc tính thời gian
        public Multi_Mode()
        {
            InitializeComponent();
        }

        private void Multi_Mode_Load(object sender, EventArgs e)
        {

        }
        private void frmGame_Load(object sender, EventArgs e)
        {
            // load ảnh heart cho hp playertank
            picHeart.Image = Image.FromFile(Common.path + @"\Images\heart.png");


            // add picture box vào mảng hiển thị số lượng địch

            // khởi tạo graphics
            graphics = pnMainGame.CreateGraphics();
            // khỏi tạo background
            background = new Bitmap(Common.SCREEN_WIDTH, Common.SCREEN_HEIGHT);
            // khởi tạo bitmap castle
            bmpCastle = new Bitmap(Common.STEP * 3, Common.STEP * 3);
            // khởi tạo map
            map = new int[Common.NUMBER_OBJECT_HEIGHT, Common.NUMBER_OBJECT_WIDTH];
            // tạo đối tượng quản lí tường
            wallManager = new WallManagement();
            // tạo đối tượng quản lí vụ nổ
            explosionManager = new ExplosionManagement();
            // tạo đối tượng xe tăng player
            playerTank = new PlayerTank();
            playerTank.LoadImage(Common.path + @"\Images\tank0.png");
            // khởi tạo danh sách địch
            enemyTankManager = new EnemyTankManagement();
            // khởi tạo vật phẩm
            item = new Item();

            // khởi tạo game
            this.GameStart();
        }
        private void GameStart()
        {
            // phát âm thanh
            Sound.PlayStartSound();


            // load map
            Array.Copy(Common.ReadMap(String.Format("{0}{1:00}.txt", Common.path + @"\Maps\Map", this.level),
                Common.NUMBER_OBJECT_HEIGHT, Common.NUMBER_OBJECT_WIDTH),
            this.map, Common.NUMBER_OBJECT_HEIGHT * Common.NUMBER_OBJECT_WIDTH);


            // giải phóng danh sách tường cũ
            wallManager.WallsClear();

            // giải phóng danh sách địch
            enemyTankManager.EnemyTanksClear();

            // giải phóng tất cả vụ nổ
            explosionManager.Explosions.Clear();
            GC.Collect();


            // tạo danh sách tường
            wallManager.CreatWall(this.map, this.level);

            // khởi tạo danh sách địch
            //Phần này trong multi mode bỏ đi
            /* enemyTankManager.Init_EnemyTankManagement(String.Format("{0}{1:00}.txt",
                 Common.path + @"\EnemyTankParameters\EnemyParameter", this.level));*/


            // hiển thị thông tin level hiện tại
            /*lblLevel.Text = String.Format("LEVEL {0}", this.level);
            // hiển thị số lượng xe tăng địch cần tiêu diệt bên bảng thông tin
            ShowNumberEnemyTankDestroy(enemyTankManager.NumberEnemyTank());*/


            // cập nhật vị trí xe tăng player
            playerTank.SetLocation();
            // cập nhật năng lượng xe tăng player 
            playerTank.Energy = 100;
            // cập nhật khiên bảo vệ
            playerTank.IsShield = false;
            // cập nhật loại đạn
            playerTank.BulletType = BulletType.eTriangleBullet;


            /*// cập nhật thông tin máu hiển thị của xe tăng player
            this.lblHpTankPlayer.Width = playerTank.Energy;
            // cập nhật thông tin máu hiển thị của thành
            this.lblCastleBlood.Width = 60;
            // cập nhật thông tin vật phẩm đang ăn
            this.picItem.Image = null;
            this.lblItemActive.Text = "";*/


            // load hình castle 
            bmpCastle = (Bitmap)Image.FromFile(Common.path + @"\Images\castle.png");
            // điểm và số lượng địch tiêu diệt được là 0
            this.scores = 0;
            this.killed = 0;
            // hủy hình ảnh item
            item.BmpObject = null;
            item.IsOn = false;
            // bật biến hoạt động của item về false
            isTimeItemActive = false;
            // bật các nút chức năng trên game 
            this.LabelEnableOn();
            // set thời gian item và chạy item
            timeItem = 50;
            timeItemActive = 15;
            /*tmrShowItem.Start();
            tmrGameLoop.Start();*/
        }
        private void LabelEnableOn()
        {
            this.lblInforPandP.Enabled = true;
            this.lblInforMenu.Enabled = true;
            this.lblInforExit.Enabled = true;
        }

        private void LabelEnableOff()
        {
            this.lblInforPandP.Enabled = false;
            this.lblInforMenu.Enabled = false;
            this.lblInforExit.Enabled = false;
        }



        private void tmrGameLoop_Tick(object sender, EventArgs e)
        {
            // xóa background
            Common.PaintClear(this.background);

            // hiển thị castle
            Common.PaintObject(this.background, bmpCastle,
                420, 700, 0, 0, 60, 60);


            // vẽ và di chuyển đạn player
            playerTank.ShowBulletAndMove(this.background);


            #region đạn player và đạn địch trúng tường
            for (int i = wallManager.Walls.Count - 1; i >= 0; i--)
            {
                // chạy danh sách đạn player và kiểm tra
                for (int j = 0; j < playerTank.Bullets.Count; j++)
                {
                    // nếu đạn xe tăng player trúng tường 
                    if (Common.IsCollision(playerTank.Bullets[j].Rect, wallManager.Walls[i].Rect))
                    {
                        // viên đạn bị hủy nếu nó trúng, không phải bụi cây(4)
                        if (wallManager.Walls[i].WallNumber != 4 &&
                            wallManager.Walls[i].WallNumber != 5)
                        {
                            // thêm vụ nổ vào danh sách
                            explosionManager.CreateExplosion(ExplosionSize.eSmallExplosion, playerTank.Bullets[j].Rect);
                            // viên đạn xe tăng player này bị hủy
                            playerTank.RemoveOneBullet(j);
                        }

                        // hủy viên gạch đi khi nó là gạch có thể phá hủy
                        if (wallManager.Walls[i].WallNumber == 1)
                        {
                            //Console.WriteLine("Ta bắn trúng tường có thể phá.");
                            wallManager.RemoveOneWall(i);
                        }
                        else
                        // player tự bắn trúng boss của player
                        if (wallManager.Walls[i].WallNumber == 6)
                        {
                            //Console.WriteLine("player bắn trúng boss player!");
                            lblCastleBlood.Width -= 6;
                            if (lblCastleBlood.Width == 0)
                            {
                                // game over
                                inforStyle = InforStyle.eGameOver;
                                // lâu đài bị hỏng
                                bmpCastle = (Bitmap)Image.FromFile(Common.path + @"\Images\ruinedcastle.png");
                                /*// dừng timer show vật phẩm
                                tmrShowItem.Stop();
                                // thời gian delay
                                tmrDelay.Start();*/
                            }
                        }
                    }
                }

                
            }
            #endregion

            

            // xe tăng player di chuyển
            if (!playerTank.IsWallCollision(wallManager.Walls, playerTank.DirectionTank) &&
                !playerTank.IsEnemyTankCollisions(enemyTankManager.EnemyTanks))
                playerTank.Move();

            // hiển thị xe tăng của player
            playerTank.Show(this.background);

            // hiển thị tất cả tường lên background
            wallManager.ShowAllWall(this.background);

            //hiển thị vụ nổ
            explosionManager.ShowAllExplosion(this.background);

            

            //vẽ lại Bitmap background lên form
            graphics.DrawImageUnscaled(this.background, 0, 0);
        }

        // hàm delay vòng lặp game sau khi game kết thúc
        private void tmrDelay_Tick(object sender, EventArgs e)
        {
            this.LabelEnableOff();
            time_delay += 1;
            if (time_delay > 1)
            {
                time_delay = 0;
 /*               tmrDelay.Stop();
                tmrGameLoop.Stop();*/
                // hiển thị theo loại thông báo 
                switch (inforStyle)
                {
                    case InforStyle.eGameOver:
                        // phát âm thanh
                        Sound.PlayGameOverSound();
                        this.GameOver(this.scores, this.killed);
                        break;
                    case InforStyle.eGameNext:
                        // phát âm thanh
                        Sound.PlayNextLevelSound();
                        this.GameNext(this.scores, this.killed);
                        break;
                    case InforStyle.eGameWin:
                        // phát âm thanh
                        Sound.PlayGameWinSound();
                        this.GameWin(this.scores, this.killed);
                        break;
                }
            }

        }
        private void btn_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Tag.ToString())
            {
                case "tag_menu":
                    
                    this.Close();
                    break;
                case "tag_gamestart":
                    // khởi động lại vòng lặp game
                    this.GameStart();
                    /*pnGameOver.Location = new Point(3, -900);
                    pnNextLevel.Location = new Point(3, -900);
                    pnGameWin.Location = new Point(3, -900);
                    pnGameOver.Enabled = false;
                    pnNextLevel.Enabled = false;
                    pnGameWin.Enabled = false;*/
                    break;
            }
        }

        private void GameOver(int scores, int killed)
        {
            // dừng các timer
            /*tmrGameLoop.Stop();
            tmrShowItem.Stop();
            tmrItemActive.Stop();*/
            isTimeItemActive = false;
            
            // hiển thị panel GameOver
            /*pnGameOver.Top = 3;
            pnGameOver.Left = 3;
            pnGameOver.Enabled = true;*/
            
        }
        private void GameNext(int scores, int killed)
        {
            // dừng các timer
            /*tmrGameLoop.Stop();
            tmrShowItem.Stop();
            tmrItemActive.Stop();*/
            isTimeItemActive = false;

        }

        // game win
        private void GameWin(int scores, int killed)
        {
            // dừng các timer
           /* tmrGameLoop.Stop();
            tmrShowItem.Stop();
            tmrItemActive.Stop();*/
            isTimeItemActive = false;
            // hiển thị panel GameWin
            /*pnGameWin.Top = 3;
            pnGameWin.Left = 3;
            pnGameWin.Enabled = true;*/
        }

    }
}
