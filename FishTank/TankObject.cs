//Copyright, Sean T. McBeth, 2007
//sean.mcbeth@gmail.com
//www.seanmcbeth.com
namespace FishTank.Common
{
    /// <summary>
    /// Abstraction class for objects that get stored in the tank
    /// </summary>
    public abstract class TankObject
    {
        protected int x, y, width, height;
        protected long age;
        public TankObject(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.age = 0;
        }
        public abstract string ActionDescription { get; }
        public virtual bool IsReadyForCleanup { get { return false; } }
        public int X { get { return this.x; } }
        public int Y { get { return this.y; } }
        public int Width { get { return this.width; } }
        public int Height { get { return this.height; } }
        //int Z { get; }
        public long Age { get { return this.age; } }
        public abstract void Update(Tank environ, int millis);
    }
}
