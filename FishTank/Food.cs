//Copyright, Sean T. McBeth, 2007
//sean.mcbeth@gmail.com
//www.seanmcbeth.com
namespace FishTank.Common
{
    public class Food : TankObject
    {
        public static int AvatarWidth, AvatarHeight;
        private bool wasEaten;
        public Food(int x) : base(x, 0, AvatarWidth, AvatarHeight)
        {
            this.wasEaten = false;
        }
        public override void Update(Tank tank, int millis)
        {
            this.age++;
            if (this.age > 10 && this.y < tank.Height - 1)
            {
                this.y++;
            }
            if (this.age > 20)
            {
                int dirt = (int)(this.age / 2);
                tank.Dirt += dirt;
            }
        }

        public override string ActionDescription
        {
            get
            {
                if (this.age <= 10)
                {
                    return "floating on the surface";
                }
                return "floating to the bottom";
            }
        }
        public override bool IsReadyForCleanup
        {
            get
            {
                return this.age >= 30 || wasEaten;
            }
        }

        public int Eat()
        {
            this.wasEaten = true;
            return (int)(30 - this.age);
        }
    }
}