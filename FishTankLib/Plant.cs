//Copyright, Sean T. McBeth, 2007
//sean.mcbeth@gmail.com
//www.seanmcbeth.com
namespace FishTank.Common
{
    public class Plant : TankObject
    {
        public static int AvatarWidth, AvatarHeight;
        public Plant(int x, int y) : base(x, y, AvatarWidth, AvatarHeight) { }
        public override void Update(Tank tank, int millis)
        {
            this.age++;
        }
        public override string ActionDescription
        {
            get
            {
                return "waving about";
            }
        }
    }
}