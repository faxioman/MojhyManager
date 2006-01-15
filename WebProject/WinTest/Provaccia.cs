using System;

using System.Drawing;

using System.Drawing.Drawing2D;

using System.Collections;

using System.ComponentModel;

using System.Windows.Forms;

using System.Data;



namespace RetainedMode
{

    /// <summary>

    /// Summary description for Form1.

    /// </summary>

    public class Form1 : System.Windows.Forms.Form
    {

        private System.Windows.Forms.ToolBar toolBar1;

        private System.Windows.Forms.ToolBarButton toolBarButton1;

        private System.Windows.Forms.ToolBarButton toolBarButton2;

        private System.ComponentModel.IContainer components;



        PrimitiveCollection _primitives = new PrimitiveCollection();



        float _zoom = 1.0f;

        float _rotation = 0f;

        int _panX = 0;

        int _panY = 0;



        bool _dragging;

        Primitive _topPrimitive;



        Point _lastPos = new Point(0, 0);

        private System.Windows.Forms.ToolBarButton toolBarButton3;

        private System.Windows.Forms.ToolBarButton toolBarButton4;

        Point _curPos = new Point(0, 0);



        public Form1()
        {

            //

            // Required for Windows Form Designer support

            //

            InitializeComponent();



            this.SetStyle(ControlStyles.AllPaintingInWmPaint |

              ControlStyles.UserPaint |

              ControlStyles.DoubleBuffer, true);



        }



        /// <summary>

        /// Clean up any resources being used.

        /// </summary>

        protected override void Dispose(bool disposing)
        {

            if (disposing)
            {

                if (components != null)
                {

                    components.Dispose();

                }

            }

            base.Dispose(disposing);

        }



        #region Windows Form Designer generated code

        /// <summary>

        /// Required method for Designer support - do not modify

        /// the contents of this method with the code editor.

        /// </summary>

        private void InitializeComponent()
        {

            this.toolBar1 = new System.Windows.Forms.ToolBar();

            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();

            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();

            this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();

            this.toolBarButton4 = new System.Windows.Forms.ToolBarButton();

            this.SuspendLayout();

            // 

            // toolBar1

            // 

            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {

                                            this.toolBarButton1,

                                            this.toolBarButton2,

                                            this.toolBarButton3,

                                            this.toolBarButton4});

            this.toolBar1.DropDownArrows = true;

            this.toolBar1.Location = new System.Drawing.Point(0, 0);

            this.toolBar1.Name = "toolBar1";

            this.toolBar1.ShowToolTips = true;

            this.toolBar1.Size = new System.Drawing.Size(432, 42);

            this.toolBar1.TabIndex = 0;

            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);

            // 

            // toolBarButton1

            // 

            this.toolBarButton1.Tag = "Ellipse";

            this.toolBarButton1.Text = "Ellipse";

            this.toolBarButton1.ToolTipText = "Drop an ellipse";

            // 

            // toolBarButton2

            // 

            this.toolBarButton2.Tag = "Square";

            this.toolBarButton2.Text = "Square";

            this.toolBarButton2.ToolTipText = "Drop a square";

            // 

            // toolBarButton3

            // 

            this.toolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;

            // 

            // toolBarButton4

            // 

            this.toolBarButton4.Tag = "Transform";

            this.toolBarButton4.Text = "Transform";

            // 

            // Form1

            // 

            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);

            this.BackColor = System.Drawing.Color.White;

            this.ClientSize = new System.Drawing.Size(432, 325);

            this.Controls.Add(this.toolBar1);

            this.Name = "Form1";

            this.Text = "Retained Mode Graphics demo";

            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);

            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);

            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);

            this.ResumeLayout(false);



        }

        #endregion



        /// <summary>

        /// The main entry point for the application.

        /// </summary>

        [STAThread]

        static void Main()
        {

            Application.Run(new Form1());

        }



        private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {

            Primitive p = null;





            switch ((string)e.Button.Tag)
            {

                case "Ellipse":

                    p = new Ellipse();

                    break;

                case "Square":

                    p = new Square();

                    break;

                case "Transform":

                    TransForm t1 = new TransForm();

                    t1.Zoom = _zoom;

                    t1.Rotation = _rotation;

                    t1.PanX = _panX;

                    t1.PanY = _panY;

                    t1.ShowDialog();

                    _zoom = t1.Zoom;

                    _rotation = t1.Rotation;

                    _panX = t1.PanX;

                    _panY = t1.PanY;

                    Invalidate();

                    return;

            }



            Random r = new Random((int)DateTime.Now.Millisecond);



            p.Location = new Point(r.Next(400), r.Next(400));

            p.Size = new Size(5 + r.Next(100), 5 + r.Next(100));



            this._primitives.Add(p);



            Invalidate();



        }



        protected override void OnPaint(PaintEventArgs e)
        {

            Matrix mx = new Matrix();

            mx.Translate(-this.ClientSize.Width / 2, -this.ClientSize.Height / 2, MatrixOrder.Append);

            mx.Rotate(_rotation, MatrixOrder.Append);

            mx.Translate(this.ClientSize.Width / 2 + _panX, this.ClientSize.Height / 2 + _panY, MatrixOrder.Append);

            mx.Scale(_zoom, _zoom, MatrixOrder.Append);

            e.Graphics.Transform = mx;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (Primitive p in _primitives)

                p.Draw(e.Graphics);

        }



        private void Form1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            _lastPos = _curPos;

            _curPos = new Point(e.X, e.Y);



            //backtrack the mouse..

            Point[] pts = new Point[] { _curPos };

            Matrix mx = new Matrix();

            mx.Translate(-this.ClientSize.Width / 2, -this.ClientSize.Height / 2, MatrixOrder.Append);

            mx.Rotate(_rotation, MatrixOrder.Append);

            mx.Translate(this.ClientSize.Width / 2 + _panX, this.ClientSize.Height / 2 + _panY, MatrixOrder.Append);

            mx.Scale(_zoom, _zoom, MatrixOrder.Append);

            mx.Invert();

            mx.TransformPoints(pts);

            _curPos = pts[0];



            if (!_dragging)
            {

                _topPrimitive = null;

                bool needsInvalidate = false;

                foreach (Primitive p in _primitives)
                {

                    if (p.Highlight == true)
                    {

                        needsInvalidate = true;

                        p.Highlight = false;

                    }

                    if (p.HitTest(_curPos)) //changed this to use the adjusted cursor position.
                    {

                        _topPrimitive = p;

                    }

                }



                if (_topPrimitive != null)
                {

                    needsInvalidate = true;

                    _topPrimitive.Highlight = true;

                }



                if (needsInvalidate)

                    Invalidate();

            }

            else
            {

                //Move the primitive by the difference between the last mouse position and this mouse position

                _topPrimitive.Location = new Point(_topPrimitive.Location.X + (_curPos.X - _lastPos.X), _topPrimitive.Location.Y + (_curPos.Y - _lastPos.Y));

                Invalidate();

            }



        }



        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            //If the mouse is in a primitive, we drag it.

            if (_topPrimitive != null)

                _dragging = true;

        }



        private void Form1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            _dragging = false;

        }



    }

    /// <summary>

    /// Summary description for TransForm.

    /// </summary>

    public class TransForm : System.Windows.Forms.Form
    {

        private System.Windows.Forms.TextBox textBox1;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.TextBox textBox2;

        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.TextBox textBox3;

        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.TextBox textBox4;

        private System.Windows.Forms.Button button1;

        /// <summary>

        /// Required designer variable.

        /// </summary>

        private System.ComponentModel.Container components = null;



        public TransForm()
        {

            //

            // Required for Windows Form Designer support

            //

            InitializeComponent();



            //

            // TODO: Add any constructor code after InitializeComponent call

            //

        }



        /// <summary>

        /// Clean up any resources being used.

        /// </summary>

        protected override void Dispose(bool disposing)
        {

            if (disposing)
            {

                if (components != null)
                {

                    components.Dispose();

                }

            }

            base.Dispose(disposing);

        }



        #region Windows Form Designer generated code

        /// <summary>

        /// Required method for Designer support - do not modify

        /// the contents of this method with the code editor.

        /// </summary>

        private void InitializeComponent()
        {

            this.textBox1 = new System.Windows.Forms.TextBox();

            this.label1 = new System.Windows.Forms.Label();

            this.label2 = new System.Windows.Forms.Label();

            this.textBox2 = new System.Windows.Forms.TextBox();

            this.label3 = new System.Windows.Forms.Label();

            this.textBox3 = new System.Windows.Forms.TextBox();

            this.label4 = new System.Windows.Forms.Label();

            this.textBox4 = new System.Windows.Forms.TextBox();

            this.button1 = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // 

            // textBox1

            // 

            this.textBox1.Location = new System.Drawing.Point(160, 32);

            this.textBox1.Name = "textBox1";

            this.textBox1.TabIndex = 0;

            this.textBox1.Text = "1.0";

            // 

            // label1

            // 

            this.label1.Location = new System.Drawing.Point(32, 32);

            this.label1.Name = "label1";

            this.label1.TabIndex = 1;

            this.label1.Text = "Zoom";

            // 

            // label2

            // 

            this.label2.Location = new System.Drawing.Point(32, 64);

            this.label2.Name = "label2";

            this.label2.TabIndex = 1;

            this.label2.Text = "Rotate";

            // 

            // textBox2

            // 

            this.textBox2.Location = new System.Drawing.Point(160, 64);

            this.textBox2.Name = "textBox2";

            this.textBox2.TabIndex = 0;

            this.textBox2.Text = "1.0";

            // 

            // label3

            // 

            this.label3.Location = new System.Drawing.Point(24, 96);

            this.label3.Name = "label3";

            this.label3.TabIndex = 1;

            this.label3.Text = "Pan-X";

            // 

            // textBox3

            // 

            this.textBox3.Location = new System.Drawing.Point(160, 96);

            this.textBox3.Name = "textBox3";

            this.textBox3.TabIndex = 0;

            this.textBox3.Text = "0";

            // 

            // label4

            // 

            this.label4.Location = new System.Drawing.Point(24, 128);

            this.label4.Name = "label4";

            this.label4.TabIndex = 1;

            this.label4.Text = "Pan-Y";

            // 

            // textBox4

            // 

            this.textBox4.Location = new System.Drawing.Point(160, 128);

            this.textBox4.Name = "textBox4";

            this.textBox4.TabIndex = 0;

            this.textBox4.Text = "0";

            // 

            // button1

            // 

            this.button1.Location = new System.Drawing.Point(104, 184);

            this.button1.Name = "button1";

            this.button1.TabIndex = 2;

            this.button1.Text = "Done";

            this.button1.Click += new System.EventHandler(this.button1_Click);

            // 

            // TransForm

            // 

            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);

            this.ClientSize = new System.Drawing.Size(292, 221);

            this.Controls.Add(this.button1);

            this.Controls.Add(this.label1);

            this.Controls.Add(this.textBox1);

            this.Controls.Add(this.label2);

            this.Controls.Add(this.textBox2);

            this.Controls.Add(this.label3);

            this.Controls.Add(this.textBox3);

            this.Controls.Add(this.label4);

            this.Controls.Add(this.textBox4);

            this.Name = "TransForm";

            this.Text = "TransForm";

            this.ResumeLayout(false);



        }

        #endregion



        float _zoom;

        public float Zoom
        {

            get { return _zoom; }

            set
            {

                _zoom = value;

                this.textBox1.Text = value.ToString();

            }

        }



        float _rotation;

        public float Rotation
        {

            get { return _rotation; }

            set
            {

                _rotation = value;

                this.textBox2.Text = value.ToString();

            }

        }





        int _panX;

        public int PanX
        {

            get { return _panX; }

            set
            {

                _panX = value;

                this.textBox3.Text = value.ToString();

            }

        }



        int _panY;

        public int PanY
        {

            get { return _panY; }

            set
            {

                _panY = value;

                this.textBox4.Text = value.ToString();

            }

        }





        private void button1_Click(object sender, System.EventArgs e)
        {

            try
            {

                _zoom = float.Parse(this.textBox1.Text);

                _rotation = float.Parse(this.textBox2.Text);

                _panX = int.Parse(this.textBox3.Text);

                _panY = int.Parse(this.textBox4.Text);

                this.DialogResult = DialogResult.OK;

            }

            catch
            {

                MessageBox.Show("Enter all numeric values");

            }

        }



    }

}