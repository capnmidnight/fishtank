//Copyright, Sean T. McBeth, 2007
//sean.mcbeth@gmail.com
//www.seanmcbeth.com
namespace FishTank.Common
{
    public class Poop : TankObject
    {
        public static int AvatarWidth, AvatarHeight;
        public Poop(int x, int y) : base(x, y, AvatarWidth, AvatarHeight) {}
        public override void Update(Tank tank, int millis)
        {
            this.age++;
            int dirt = (int)(this.age / 3);
            if (this.y < tank.Height - 1)
            {
                this.y++;
            }
            tank.Dirt += dirt;
        }

        public override string ActionDescription
        {
            get
            {
                return "mucking up the water";
            }
        }

        public override bool IsReadyForCleanup
        {
            get
            {
                return this.age >= 100;
            }
        }
    }
}