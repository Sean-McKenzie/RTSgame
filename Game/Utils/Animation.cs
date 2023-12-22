using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Utilities
{
    public class Animation
    {
        public double AniSpeed;
        protected List<Image> frames = new List<Image>();
        public int FrameCount
        {
            get
            {
                return frames.Count;
            }
        }

        //public double Index;// deprecated now that we are using a DTA object 
        protected int width, height;
        public bool Continuous;
        public int Width
        {
            get
            { // transparency check tool
                return width;
            }
        }
        public int Height
        {
            get
            { // transparency check tool
                return height;
            }
        }

        #region DeprecatedProperty_DO_ NOT_USE
        //public Image Frame
        //{
        //    get
        //    {
        //        Index+= AniSpeed;
        //        if (Index >= frames.Count)
        //        {
        //            if (Continuous)
        //            {
        //                Index = 0;
        //            }
        //            else
        //            {
        //                Index = frames.Count - 1;
        //            }
        //        }
        //        return frames[(int)Index];
        //    }
        //}
        #endregion

        public Image GetFrame(int idx)
        {
            return frames[idx];
        }

        public Animation(Image spriteMap, int width, int height, int xStart, int yStart, int rows, int cols, double scale,double aniSpeed, bool continuous)
        {
            AniSpeed = aniSpeed;
            Continuous = continuous;
            Bitmap orig = new Bitmap(spriteMap, (int)(scale * spriteMap.Width), (int)(scale * spriteMap.Height));
            this.width = width;
            this.height = height;
//            Index = 0; not needed anymore (unless we stop using a DTA object)
            for (int y = yStart; y < yStart + rows * height; y = y + height)
            {
                for (int x = xStart; x < xStart + cols * width; x = x + width)
                { 
                    Rectangle rect = new Rectangle(x, y, width, height);
                    Image frame = orig.Clone(rect, spriteMap.PixelFormat);
                    frames.Add(frame);
                }
            }
        }
    }
}
