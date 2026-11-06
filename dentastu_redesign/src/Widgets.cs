// [AI生成] UI再設計版のオーナードロー部品（セグメント切替・スライダー・トグル）
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace dentastu.Redesign
{
    static class Theme
    {
        public static readonly Color Bg = Color.FromArgb(244, 246, 250);
        public static readonly Color CardColor = Color.White;
        public static readonly Color Border = Color.FromArgb(226, 230, 236);
        public static readonly Color Text = Color.FromArgb(28, 34, 46);
        public static readonly Color Muted = Color.FromArgb(108, 116, 130);
        public static readonly Color Accent = Color.FromArgb(37, 99, 235);
        public static readonly Color AccentSoft = Color.FromArgb(225, 234, 253);
        public static readonly Color Warm = Color.FromArgb(234, 88, 12);
        public static readonly Color Grid = Color.FromArgb(236, 239, 243);
        public static readonly Color Track = Color.FromArgb(226, 231, 238);

        public const string UiFont = "Yu Gothic UI";
        public const string NumFont = "Segoe UI";

        public static GraphicsPath Round(RectangleF r, float rad)
        {
            var p = new GraphicsPath();
            float d = rad * 2;
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        public static void DrawCard(Graphics g, RectangleF r, float rad = 12)
        {
            using (var sh = Round(new RectangleF(r.X, r.Y + 2, r.Width, r.Height), rad))
            using (var b = new SolidBrush(Color.FromArgb(14, 20, 30, 60))) g.FillPath(b, sh);
            using (var p = Round(r, rad))
            {
                using (var b = new SolidBrush(Theme.CardColor)) g.FillPath(b, p);
                using (var pen = new Pen(Theme.Border)) g.DrawPath(pen, p);
            }
        }
    }

    public abstract class Widget
    {
        public Rectangle Bounds;
        public bool Visible = true;
        public event Action Changed;
        protected void RaiseChanged() { if (Changed != null) Changed(); }
        public abstract void Paint(Graphics g);
        public virtual bool HitTest(Point p) { return Visible && Bounds.Contains(p); }
        public virtual void MouseDown(Point p) { }
        public virtual void MouseMove(Point p) { }
        public virtual void MouseUp(Point p) { }
    }

    public class Segmented : Widget
    {
        public string[] Items;
        public int Selected;
        public Segmented(params string[] items) { Items = items; }

        public Rectangle ItemRect(int i)
        {
            int w = (Bounds.Width - 8) / Items.Length;
            return new Rectangle(Bounds.X + 4 + i * w, Bounds.Y + 4, w, Bounds.Height - 8);
        }

        public override void Paint(Graphics g)
        {
            using (var p = Theme.Round(Bounds, 10))
            using (var b = new SolidBrush(Color.FromArgb(236, 239, 244))) g.FillPath(b, p);
            using (var f = new Font(Theme.UiFont, 10.5f, FontStyle.Bold, GraphicsUnit.Point))
            {
                for (int i = 0; i < Items.Length; i++)
                {
                    var r = ItemRect(i);
                    if (i == Selected)
                    {
                        using (var sh = Theme.Round(new RectangleF(r.X, r.Y + 1, r.Width, r.Height), 8))
                        using (var sb = new SolidBrush(Color.FromArgb(22, 0, 0, 0))) g.FillPath(sb, sh);
                        using (var pp = Theme.Round(r, 8))
                        using (var wb = new SolidBrush(Color.White)) g.FillPath(wb, pp);
                    }
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    using (var tb = new SolidBrush(i == Selected ? Theme.Accent : Theme.Muted))
                        g.DrawString(Items[i], f, tb, r, sf);
                }
            }
        }

        public override void MouseDown(Point p)
        {
            int w = (Bounds.Width - 8) / Items.Length;
            int i = Math.Max(0, Math.Min(Items.Length - 1, (p.X - Bounds.X - 4) / w));
            if (i != Selected) { Selected = i; RaiseChanged(); }
        }
    }

    public class Slider : Widget
    {
        public string Label, Symbol;
        public double Min, Max, Step, Value;
        public string Format = "0.0#";
        bool dragging;

        public Slider(string label, string symbol, double min, double max, double step, double value)
        {
            Label = label; Symbol = symbol; Min = min; Max = max; Step = step; Value = value;
        }

        Rectangle TrackRect { get { return new Rectangle(Bounds.X + 2, Bounds.Y + 34, Bounds.Width - 4, 6); } }

        public Point ThumbCenter
        {
            get
            {
                var tr = TrackRect;
                double f = (Value - Min) / (Max - Min);
                return new Point(tr.X + (int)Math.Round(f * tr.Width), tr.Y + tr.Height / 2);
            }
        }

        public Point PointFor(double v)
        {
            var tr = TrackRect;
            double f = (v - Min) / (Max - Min);
            return new Point(tr.X + (int)Math.Round(f * tr.Width), tr.Y + tr.Height / 2);
        }

        public override void Paint(Graphics g)
        {
            using (var lf = new Font(Theme.UiFont, 10f, FontStyle.Regular, GraphicsUnit.Point))
            using (var sf = new Font(Theme.NumFont, 10f, FontStyle.Italic, GraphicsUnit.Point))
            using (var vf = new Font(Theme.NumFont, 11f, FontStyle.Bold, GraphicsUnit.Point))
            using (var mb = new SolidBrush(Theme.Muted))
            using (var tb = new SolidBrush(Theme.Text))
            {
                g.DrawString(Label, lf, mb, Bounds.X, Bounds.Y + 2);
                var lw = g.MeasureString(Label, lf).Width;
                g.DrawString(Symbol, sf, tb, Bounds.X + lw, Bounds.Y + 2);

                string v = Value.ToString(Format);
                var vs = g.MeasureString(v, vf);
                var pill = new RectangleF(Bounds.Right - vs.Width - 14, Bounds.Y, vs.Width + 14, 24);
                using (var pp = Theme.Round(pill, 6))
                using (var pb = new SolidBrush(Theme.AccentSoft)) g.FillPath(pb, pp);
                using (var ab = new SolidBrush(Theme.Accent)) g.DrawString(v, vf, ab, pill.X + 7, pill.Y + 2);
            }

            var tr = TrackRect;
            using (var p = Theme.Round(tr, 3))
            using (var b = new SolidBrush(Theme.Track)) g.FillPath(b, p);
            var c = ThumbCenter;
            var fill = new Rectangle(tr.X, tr.Y, Math.Max(6, c.X - tr.X), tr.Height);
            using (var p = Theme.Round(fill, 3))
            using (var b = new SolidBrush(Theme.Accent)) g.FillPath(b, p);

            float rr = dragging ? 10 : 9;
            using (var sb = new SolidBrush(Color.FromArgb(40, 0, 0, 0))) g.FillEllipse(sb, c.X - rr, c.Y - rr + 1.5f, rr * 2, rr * 2);
            g.FillEllipse(Brushes.White, c.X - rr, c.Y - rr, rr * 2, rr * 2);
            using (var pen = new Pen(Theme.Accent, 3)) g.DrawEllipse(pen, c.X - rr + 1.5f, c.Y - rr + 1.5f, rr * 2 - 3, rr * 2 - 3);
        }

        public override void MouseDown(Point p) { dragging = true; SetFromX(p.X); }
        public override void MouseMove(Point p) { if (dragging) SetFromX(p.X); }
        public override void MouseUp(Point p) { dragging = false; }

        void SetFromX(int x)
        {
            var tr = TrackRect;
            double f = Math.Max(0, Math.Min(1, (x - tr.X) / (double)tr.Width));
            double v = Min + f * (Max - Min);
            v = Math.Round(v / Step) * Step;
            v = Math.Max(Min, Math.Min(Max, v));
            if (Math.Abs(v - Value) > 1e-12) { Value = v; RaiseChanged(); }
        }
    }

    public class Toggle : Widget
    {
        public string Label;
        public bool On;
        public Toggle(string label, bool on) { Label = label; On = on; }

        public Rectangle SwitchRect { get { return new Rectangle(Bounds.Right - 44, Bounds.Y + (Bounds.Height - 24) / 2, 44, 24); } }

        public override void Paint(Graphics g)
        {
            using (var f = new Font(Theme.UiFont, 10f, FontStyle.Regular, GraphicsUnit.Point))
            using (var b = new SolidBrush(Theme.Text))
            {
                var sf = new StringFormat { LineAlignment = StringAlignment.Center };
                g.DrawString(Label, f, b, new RectangleF(Bounds.X, Bounds.Y, Bounds.Width - 50, Bounds.Height), sf);
            }
            var r = SwitchRect;
            using (var p = Theme.Round(r, 12))
            using (var bb = new SolidBrush(On ? Theme.Accent : Color.FromArgb(203, 209, 218))) g.FillPath(bb, p);
            int kx = On ? r.Right - 22 : r.X + 2;
            using (var sb = new SolidBrush(Color.FromArgb(40, 0, 0, 0))) g.FillEllipse(sb, kx, r.Y + 3, 20, 20);
            g.FillEllipse(Brushes.White, kx, r.Y + 2, 20, 20);
        }

        public override void MouseDown(Point p) { On = !On; RaiseChanged(); }
    }
}
