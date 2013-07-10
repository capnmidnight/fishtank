//Copyright, Sean T. McBeth, 2007
//sean.mcbeth@gmail.com
//www.seanmcbeth.com
namespace FishTank.Common
{
    public class Snail : TankObject
    {
        public static int AvatarWidth, AvatarHeight;
        public Snail(int x, int y) : base(x, y, AvatarWidth, AvatarHeight) { }
        public override void Update(Tank environ, int millis)
        {
            //TODO: make the snails eat the algae
        }
        public override string ActionDescription
        {
            get { return "sticking to the glass"; }
        }
    }
}
