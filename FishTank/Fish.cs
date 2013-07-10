//Copyright, Sean T. McBeth, 2007
//sean.mcbeth@gmail.com
//www.seanmcbeth.com
namespace FishTank.Common
{
    /// <summary>
    /// The Fish is an autonomous unit that lives within the tank.
    /// It is capable of performing its own actions. The user does not
    /// directly control the actions of the Fish, or any of the objects.
    /// in the tank.
    /// </summary>
    public class Fish : TankObject
    {
        public static int AvatarWidth, AvatarHeight;
        
        private int hunger;
        private int dx, dy;

        public Fish(int x, int y)
            : base(x, y, AvatarWidth, AvatarHeight)
        {
            this.hunger = 0;
            this.dx = 0;
            this.dy = 0;
        }

        #region ITankObject interface items
        public override string ActionDescription { get { return "swimming about"; } }
        public int Dx { get { return this.dx; } }
        public int Dy { get { return this.dy; } }
        public override void Update(Tank tank, int millis)
        {
            tank.Oxygen--;
            this.hunger++;
            this.age++;
            int fishCount, avgX, avgY, avgDx, avgDy, avoidDx, avoidDy, delX, delY;
            fishCount = avgDx = avgDy = avgX = avgY = avoidDx = avoidDy = 0;
            foreach (TankObject obj in tank.Objects)
            {
                if (obj is Fish && obj != this)
                {
                    Fish f = obj as Fish;
                    avgX += f.x;
                    avgY += f.y;
                    avgDx += f.dx;
                    avgDy += f.dy;
                    delX = f.x - this.x;
                    delY = f.y - this.y;
                    int distq = delX * delX + delY * delY;
                    if (distq < 100)
                    {
                        avoidDx -= delX;
                        avoidDy -= delY;
                    }
                }
            }
            if (fishCount > 0)
            {
                avgX /= fishCount;
                avgY /= fishCount;
                avgDx /= fishCount;
                avgDy /= fishCount;
                this.dx += (avgX - this.x) / 100 + avoidDx + (avgDx - this.dx) / 8;
                this.dy += (avgY - this.y) / 100 + avoidDy + (avgDy - this.dy) / 8;
                int speedq = this.dx * this.dx + this.dy * this.dy;
                if (speedq > 900)
                {
                    double speed = System.Math.Sqrt(speedq);
                    this.dx = (int)(this.dx * 100 / speed);
                    this.dy = (int)(this.dy * 100 / speed);
                }
            }
            else
            {
                this.dx += tank.Random.Next(20) - 10;
                this.dy += tank.Random.Next(20) - 10;
            }
            this.x += this.dx * millis / 1000;
            this.y += this.dy * millis / 1000;
            if (this.x < 0 || this.x + AvatarWidth > tank.Width)
            {
                this.dx *= -1;
                this.x += this.dx * millis / 1000;
            }
            if (this.y < 0 || this.y + AvatarHeight > tank.Height)
            {
                this.dy *= -1;
                this.y += this.dy * millis / 1000;
            }
        }

        #endregion
    }
}

