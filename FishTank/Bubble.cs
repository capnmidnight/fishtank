//Copyright, Sean T. McBeth, 2007
//sean.mcbeth@gmail.com
//www.seanmcbeth.com
namespace FishTank.Common
{
    public class Bubble : TankObject
    {
        public static int AvatarWidth, AvatarHeight;
        public Bubble(int x, int y) : base(x, y, AvatarWidth, AvatarHeight) { }
        public override string ActionDescription
        {
            get
            {
                return "bubbling";
            }
        }
        public override void Update(Tank tank, int millis)
        {
            this.y -= millis/2;
            this.age++;
        }
        public override bool IsReadyForCleanup
        {
            get
            {
                return y < 0;
            }
        }
    }
}
