using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using SuperTank.General;
using System.Windows.Forms;

namespace SuperTank.Objects
{
    class PlayerTank : Tank
    {
        public Keys ControlUp { get; private set; }
        public Keys ControlDown { get; private set; }
        public Keys ControlLeft { get; private set; }
        public Keys ControlRight { get; private set; }
        public Keys ControlShoot { get; private set; }
        public int PlayerNumber { get; set; }
        private bool isShield;
        protected Bitmap bmpShield;
        private int x;
        private int y;
        protected Bitmap bmpObject;

        public PlayerTank()
        {
            try
            {
                // Các thuộc tính cơ bản
                this.moveSpeed = 10;
                this.tankBulletSpeed = 20;
                this.energy = 100;
                this.DirectionTank = Direction.eUp;
                this.SkinTank = Skin.eYellow;

                // Quan trọng: Khởi tạo bitmap
                this.bmpObject = Common.LoadImage(@"\Images\tank1.png");

                // Kiểm tra nếu load thành công
                if (this.bmpObject == null)
                {
                    throw new Exception("Cannot load tank image");
                }

                // Đặt IsActivate để tank có thể hiển thị
                this.IsActivate = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tank resources: {ex.Message}");
            }
        }

        public PlayerTank(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.moveSpeed = 10;
            this.tankBulletSpeed = 20;
            this.energy = 100;
            this.SetLocation();
            this.DirectionTank = Direction.eDown;
            this.SkinTank = Skin.eYellow;
        }

        // cập nhật vị trí xe tăng player
        public void SetLocation()
        {
            int i = 17, j = 36;
            this.RectX = i * Common.STEP;
            this.RectY = j * Common.STEP;
        }
        public void SetLocation(int x, int y)
        {
            this.RectX = x * Common.STEP;  // Nhân với STEP để đúng với lưới của map
            this.RectY = y * Common.STEP;
        }
        // kiểm tra xe tăng player va chạm với xe tăng địch
        public bool IsEnemyTankCollisions(List<EnemyTank> enemyTanks)
        {
            foreach (EnemyTank enemyTank in enemyTanks)
            {
                if (this.IsObjectCollision(enemyTank.Rect))
                    return true;
            }
            return false;
        }
        // Thêm phương thức để xử lý điều khiển cho từng player
        public void SetControls(Keys up, Keys down, Keys left, Keys right, Keys shoot)
        {
            this.ControlUp = up;
            this.ControlDown = down;
            this.ControlLeft = left;
            this.ControlRight = right;
            this.ControlShoot = shoot;
        }
        // hiển thị xe tăng player
        public override void Show(Bitmap background)
        {
            // nếu xe tăng đang bật chế độ hoạt động sẽ hiển thị xe tăng, 
            // ngược lại hiện thị hiệu ứng xuất hiện
            if (background == null)
            {
                throw new ArgumentNullException(nameof(background));
            }

            if (IsActivate)
            {
                if (bmpObject == null)
                {
                     // Thử tải lại nếu bitmap bị null
                        bmpObject = Common.LoadImage(@"\Images\tank1.png");
                      if (bmpObject == null)
                         {
                           return; // Thoát nếu không thể tải
                         }
                        }

        switch (directionTank)
        {
                    case Direction.eUp:
                        Common.PaintObject(background, this.bmpObject, rect.X, rect.Y,
                               (int)skinTank * Common.tankSize, frx_tank * Common.tankSize, this.RectWidth, this.RectHeight);
                        break;
                    case Direction.eDown:
                        Common.PaintObject(background, this.bmpObject, rect.X, rect.Y,
                               (MAX_NUMBER_SPRITE_TANK - (int)skinTank) * Common.tankSize, frx_tank * Common.tankSize, this.RectWidth, this.RectHeight);
                        break;
                    case Direction.eLeft:
                        Common.PaintObject(background, this.bmpObject, rect.X, rect.Y,
                                 frx_tank * Common.tankSize, (MAX_NUMBER_SPRITE_TANK - (int)skinTank) * Common.tankSize, this.RectWidth, this.RectHeight);
                        break;
                    case Direction.eRight:
                        Common.PaintObject(background, this.bmpObject, rect.X, rect.Y,
                            frx_tank * Common.tankSize, (int)skinTank * Common.tankSize, this.RectWidth, this.RectHeight);
                        break;

                }

        if (isShield && bmpShield != null)
        {
            Common.PaintObject(background, bmpShield, rect.X, rect.Y, 0, 0, 40, 40);
        }

        if (isMove)
        {
            frx_tank--;
            if (frx_tank == -1)
                frx_tank = MAX_NUMBER_SPRITE_TANK;
        }
    }
    else
    {
        if (bmpEffect != null)
        {
            Common.PaintObject(background, bmpEffect, RectX, RectY,
                frx_effect * RectWidth, fry_effect * RectHeight, 
                RectWidth, RectHeight);
            frx_effect++;
            if (frx_effect == MAX_NUMBER_SPRITE_EFFECT)
            {
                frx_effect = 0;
                fry_effect++;
                if (fry_effect == MAX_NUMBER_SPRITE_EFFECT)
                {
                    fry_effect = 0;
                    IsActivate = true;
                }
            }
        }
    }
        }

        #region properties
     
        #endregion properties
    }
}
