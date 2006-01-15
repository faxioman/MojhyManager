using System;

using System.Drawing;

using System.Drawing.Drawing2D;

using System.Collections;



namespace RetainedMode
{

    /// <summary> 

    /// Describes a simple primitive retained mode object 

    /// Each primitive object has a location, size and colour. 

    /// Specialized draw and hit test routines in derived classes 

    /// customize the behaviour of the actual shapes 

    /// </summary> 

    public class Primitive
    {

        Size _size;

        Color _color;

        Point _location;

        bool _highlight;



        public bool Highlight
        {

            get { return _highlight; }

            set { _highlight = value; }

        }



        public Size Size
        {

            get { return _size; }

            set { _size = value; }

        }



        public Color Color
        {

            get { return _color; }

            set { _color = value; }

        }



        public Point Location
        {

            get { return _location; }

            set { _location = value; }

        }



        public Primitive()
        {

            //create a random color 

            Random r = new Random((int)DateTime.Now.Millisecond);

            _color = Color.FromArgb(r.Next(255), r.Next(255), r.Next(255));

        }



        public virtual void Draw(Graphics g)
        {

            // overridden in the derived class 

        }



        public virtual bool HitTest(Point p)
        {

            //default behaviour 

            return new Rectangle(_location, _size).Contains(p);

        }

    }



    /// <summary> 

    /// A specialized collection for storing primitive objects 

    /// </summary> 

    public class PrimitiveCollection : CollectionBase
    {

        public PrimitiveCollection()
            : base()
        {

        }



        /// <summary> 

        /// Add a primitive object to the collection. 

        /// </summary> 

        /// <param name="o"></param> 

        public void Add(Primitive o)
        {

            this.List.Add(o);

        }



        /// <summary> 

        /// Get or set a primitive object by index 

        /// </summary> 

        public Primitive this[int index]
        {

            get
            {

                return (Primitive)List[index];

            }

            set
            {

                List[index] = value;

            }

        }



        /// <summary> 

        /// Remove a primitive object from the collection. 

        /// </summary> 

        /// <param name="o"></param> 

        public void Remove(Primitive o)
        {

            List.Remove(o);

        }

    }



    /// <summary> 

    /// A square object derived from the Primitive 

    /// </summary> 

    public class Square : Primitive
    {

        public Square()
            : base()
        {

        }



        /// <summary> 

        /// Overidden to draw the square object. 

        /// </summary> 

        /// <param name="g"></param> 

        public override void Draw(Graphics g)
        {

            SolidBrush b = new SolidBrush(this.Color);

            g.FillRectangle(b, new Rectangle(this.Location, this.Size));

            b.Dispose();

            if (Highlight)
            {

                Pen p = new Pen(Color.Red, 3);

                p.DashStyle = DashStyle.DashDot;

                g.DrawRectangle(p, new Rectangle(this.Location, this.Size));

                p.Dispose();

            }

        }

    }



    /// <summary> 

    /// A circular object derived from the Primitive. 

    /// </summary> 

    public class Ellipse : Primitive
    {

        public Ellipse()
            : base()
        {

        }



        /// <summary> 

        /// Overridden to draw the elliptical object 

        /// </summary> 

        /// <param name="g"></param> 

        public override void Draw(Graphics g)
        {

            SolidBrush b = new SolidBrush(this.Color);

            g.FillEllipse(b, new Rectangle(this.Location, this.Size));

            b.Dispose();

            if (Highlight)
            {

                Pen p = new Pen(Color.Red, 3);

                p.DashStyle = DashStyle.DashDot;

                g.DrawEllipse(p, new Rectangle(this.Location, this.Size));

                p.Dispose();

            }

        }



        /// <summary> 

        /// overridden from the base class to provide exact hit testing of the ellipse, excluding the 

        /// extra corners added by the rectangular nature of the location and size definitions 

        /// </summary> 

        /// <param name="p">The point to hit test</param> 

        /// <returns>True if the point is in the ellipse</returns> 

        public override bool HitTest(Point p)
        {

            GraphicsPath pth = new GraphicsPath();

            pth.AddEllipse(new Rectangle(Location, Size));

            bool retval = pth.IsVisible(p);

            pth.Dispose();

            return retval;

        }

    }

}
