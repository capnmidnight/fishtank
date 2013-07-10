//Copyright, Sean T. McBeth, 2007
//sean.mcbeth@gmail.com
//www.seanmcbeth.com
using System;
using System.Collections.Generic;

namespace FishTank.Common
{
    public class Tank
    {
        private int width, height, oxygen, dirt;
        private List<TankObject> objects;
        private Random rand;
        //private Algae algae;
        public Tank(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.objects = new List<TankObject>();
            this.rand = new Random();
            //this.algae = new Algae(width, height);
        }

        public void Update(int millis)
        {
            for (int i = 0; i < this.objects.Count; ++i)
            {
                this.objects[i].Update(this, millis);
            }
            for (int i = this.objects.Count - 1; i >= 0; --i)
            {
                if (this.objects[i].IsReadyForCleanup)
                {
                    this.objects.RemoveAt(i);
                }
            }
            //this.algae.Update(this, millis);
        }

        //public Algae Algae { get { return this.algae; } }
        public Random Random { get { return this.rand; } }
        public int Dirt
        {
            get { return this.dirt; }
            set { this.dirt = value; }
        }
        public int Width { get { return this.width; } }
        public int Height { get { return this.height; } }
        public int Oxygen
        {
            get { return this.oxygen; }
            set { this.oxygen = value; }
        }
        public List<TankObject> Objects { get { return this.objects; } }
        
        public List<TankObject> GetNearbyObjects(int x, int y, int distance)
        {
            List<TankObject> items = new List<TankObject>();
            int distSquare = distance * distance;
            int dx, dy;
            int distTo;
            foreach (TankObject obj in this.objects)
            {
                dx = x - obj.X;
                dy = y - obj.Y;
                distTo = dx * dx + dy * dy;
                if (distTo <= distSquare)
                {
                    items.Add(obj);
                }
            }
            return items;
        }
    }
}