//Copyright, Sean T. McBeth, 2007
//sean.mcbeth@gmail.com
//www.seanmcbeth.com
namespace FishTank.Common
{
    public class Pump : TankObject
    {
        public static int AvatarWidth, AvatarHeight;
        public Pump() : base(0, 0, AvatarWidth, AvatarHeight) { }
        public override string ActionDescription
        {
            get
            {
                return "pumping";
            }
        }
        public override void Update(Tank tank, int millis)
        {
            this.age++;
            tank.Oxygen++;
            int chance = tank.Random.Next(millis * 2);
            if (chance == 0)
            {
                tank.Objects.Add(new Bubble(Pump.AvatarWidth, Pump.AvatarHeight+Bubble.AvatarHeight));
            }
        }
    }
}
