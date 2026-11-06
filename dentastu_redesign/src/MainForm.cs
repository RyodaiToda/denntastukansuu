// [AI生成] UI再設計版のメインウィンドウ。
// 1 枚のフォームにサイドバー（入力）・ブロック線図・応答グラフ・指標カードをまとめ、
// パラメータを動かすとその場でグラフが更新される。
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace dentastu.Redesign
{
    public class MainForm : Form
    {
        public const int W = 1200, H = 760, SideW = 340;

        public readonly ResponseModel Model = new ResponseModel();
        public readonly Segmented InputSeg = new Segmented("単位インパルス", "単位ステップ") { Selected = 1 };
        public readonly Segmented OrderSeg = new Segmented("1次遅れ", "2次遅れ");
        public readonly Slider SK = new Slider("ゲイン ", "K", 0.1, 10, 0.1, 1.0);
        public readonly Slider ST = new Slider("時定数 ", "T", 0.1, 5, 0.1, 1.0);
        public readonly Slider SA = new Slider("s の係数 ", "a", 0.05, 4, 0.05, 0.4);
        public readonly Slider SB = new Slider("s² の係数 ", "b", 0.1, 4, 0.05, 1.0);
        public readonly Toggle Fb = new Toggle("単位負帰還（フィードバック）", true);
        public readonly Slider STmax = new Slider("表示時間 ", "t_max", 1, 40, 0.5, 10) { Format = "0.0" };

        readonly List<Widget> widgets = new List<Widget>();
        Widget active;
        double[] ghostT, ghostY;

        public MainForm()
        {
            Text = "伝達関数 応答シミュレータ";
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(W, H);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true;
            BackColor = Theme.Bg;

            widgets.AddRange(new Widget[] { InputSeg, OrderSeg, SK, ST, SA, SB, Fb, STmax });
            foreach (var w in widgets) w.Changed += OnChanged;
            LayoutWidgets();
            Recompute();
        }

        void LayoutWidgets()
        {
            int x = 24, w = SideW - 48;
            InputSeg.Bounds = new Rectangle(x, 128, w, 40);
            OrderSeg.Bounds = new Rectangle(x, 212, w, 40);
            int y = 296;
            SK.Bounds = new Rectangle(x, y, w, 46); y += 62;
            bool first = OrderSeg.Selected == 0;
            ST.Visible = first; SA.Visible = !first; SB.Visible = !first;
            if (first) { ST.Bounds = new Rectangle(x, y, w, 46); y += 62; }
            else
            {
                SA.Bounds = new Rectangle(x, y, w, 46); y += 62;
                SB.Bounds = new Rectangle(x, y, w, 46); y += 62;
            }
            Fb.Bounds = new Rectangle(x, y, w, 36); y += 56;
            STmax.Bounds = new Rectangle(x, y, w, 46);
        }

        void OnChanged()
        {
            LayoutWidgets();
            Recompute();
            Invalidate();
        }

        public void Recompute()
        {
            Model.Input = InputSeg.Selected == 0 ? InputKind.Impulse : InputKind.Step;
            Model.Order = OrderSeg.Selected + 1;
            Model.K = SK.Value; Model.T = ST.Value; Model.A = SA.Value; Model.B = SB.Value;
            Model.Feedback = Fb.On; Model.TMax = STmax.Value;
            Model.Compute();
        }

        // ---------- マウス処理（キャプチャ用ハーネスからも呼べるよう public） ----------
        public void HandleMouseDown(Point p)
        {
            active = widgets.FirstOrDefault(w => w.HitTest(p));
            if (active != null)
            {
                ghostT = (double[])Model.Ts.Clone();   // 変更前の応答を残しておく
                ghostY = (double[])Model.Ys.Clone();
                active.MouseDown(p);
                Invalidate();
            }
        }
        public void HandleMouseMove(Point p) { if (active != null) { active.MouseMove(p); Invalidate(); } }
        public void HandleMouseUp(Point p) { if (active != null) { active.MouseUp(p); active = null; Invalidate(); } }

        protected override void OnMouseDown(MouseEventArgs e) { HandleMouseDown(e.Location); }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            HandleMouseMove(e.Location);
            Cursor = widgets.Any(w => w.HitTest(e.Location)) ? Cursors.Hand : Cursors.Default;
        }
        protected override void OnMouseUp(MouseEventArgs e) { HandleMouseUp(e.Location); }

        protected override void OnPaint(PaintEventArgs e) { Render(e.Graphics); }

        // ---------- 描画 ----------
        public void Render(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.Clear(Theme.Bg);
            DrawSidebar(g);
            DrawDiagram(g, new RectangleF(SideW + 24, 24, W - SideW - 48, 176));
            DrawChart(g, new RectangleF(SideW + 24, 216, W - SideW - 48, 392));
            DrawMetrics(g, new RectangleF(SideW + 24, 624, W - SideW - 48, 112));
        }

        static Font F(float size, FontStyle st = FontStyle.Regular, string fam = Theme.UiFont)
        {
            return new Font(fam, size, st, GraphicsUnit.Point);
        }

        void DrawSidebar(Graphics g)
        {
            g.FillRectangle(Brushes.White, 0, 0, SideW, H);
            using (var pen = new Pen(Theme.Border)) g.DrawLine(pen, SideW - 1, 0, SideW - 1, H);

            using (var tf = F(15, FontStyle.Bold))
            using (var sf = F(9))
            using (var hf = F(9, FontStyle.Bold))
            using (var tb = new SolidBrush(Theme.Text))
            using (var mb = new SolidBrush(Theme.Muted))
            {
                // ロゴ的なマーク
                using (var p = Theme.Round(new RectangleF(24, 26, 34, 34), 9))
                using (var b = new LinearGradientBrush(new PointF(24, 26), new PointF(58, 60), Theme.Accent, Color.FromArgb(124, 58, 237)))
                    g.FillPath(b, p);
                using (var pen = new Pen(Color.White, 2.2f))
                    g.DrawBezier(pen, 30, 52, 36, 32, 42, 32, 52, 34);
                g.DrawString("伝達関数 応答シミュレータ", tf, tb, 66, 22);
                g.DrawString("Durand–Kerner 法 × 留数定理で逆ラプラス変換", sf, mb, 67, 48);

                g.DrawString("入力信号  r(t)", hf, mb, 24, 102);
                g.DrawString("制御対象  G(s)", hf, mb, 24, 186);
                g.DrawString("パラメータ", hf, mb, 24, 270);

                using (var pen = new Pen(Theme.Border)) g.DrawLine(pen, 24, H - 64, SideW - 24, H - 64);
                g.DrawString("計算コア: オリジナルの DK.cs / Polynomial.cs", sf, mb, 24, H - 54);
                g.DrawString("UI: AI（Claude）による再設計版", sf, mb, 24, H - 34);
            }
            foreach (var w in widgets) if (w.Visible) w.Paint(g);
        }

        static string N(double v) { return v.ToString("0.0#"); }

        string DenominatorText(bool closed)
        {
            var m = Model;
            double c0 = closed && m.Feedback ? 1 + m.K : 1;
            return m.Order == 1
                ? string.Format("{0} + {1}s", N(c0), N(m.T))
                : string.Format("{0} + {1}s + {2}s²", N(c0), N(m.A), N(m.B));
        }

        static void DrawFraction(Graphics g, float cx, float cy, string num, string den, Font f, Brush b, out float width)
        {
            var ns = g.MeasureString(num, f); var ds = g.MeasureString(den, f);
            width = Math.Max(ns.Width, ds.Width) + 16;
            g.DrawString(num, f, b, cx - ns.Width / 2, cy - ns.Height - 2);
            g.DrawString(den, f, b, cx - ds.Width / 2, cy + 3);
            using (var pen = new Pen(b, 1.6f)) g.DrawLine(pen, cx - width / 2, cy, cx + width / 2, cy);
        }

        void DrawDiagram(Graphics g, RectangleF r)
        {
            Theme.DrawCard(g, r);
            using (var hf = F(9, FontStyle.Bold))
            using (var mb = new SolidBrush(Theme.Muted))
                g.DrawString("ブロック線図", hf, mb, r.X + 20, r.Y + 14);

            float cy = r.Y + 84;
            float x0 = r.X + 36, sumX = r.X + 120, boxL = r.X + 170, boxR = r.X + 400, outX = r.X + 500;
            var line = new Pen(Theme.Text, 2f) { EndCap = LineCap.Round, StartCap = LineCap.Round };
            var arrow = new Pen(Theme.Text, 2f) { CustomEndCap = new AdjustableArrowCap(4.5f, 5.5f) };
            using (var lf = F(12, FontStyle.Italic, Theme.NumFont))
            using (var ff = F(12, FontStyle.Regular, Theme.NumFont))
            using (var tb = new SolidBrush(Theme.Text))
            using (var ab = new SolidBrush(Theme.Accent))
            using (var mb = new SolidBrush(Theme.Muted))
            {
                g.DrawString("R(s)", lf, tb, x0 - 22, cy - 30);
                g.DrawLine(arrow, x0, cy, sumX - 12, cy);

                // 加え合わせ点
                if (Model.Feedback)
                {
                    g.FillEllipse(Brushes.White, sumX - 11, cy - 11, 22, 22);
                    g.DrawEllipse(line, sumX - 11, cy - 11, 22, 22);
                    using (var sf = F(9, FontStyle.Bold, Theme.NumFont))
                    {
                        g.DrawString("+", sf, tb, sumX - 26, cy - 24);
                        g.DrawString("−", sf, tb, sumX + 2, cy + 11);
                    }
                    g.DrawLine(arrow, sumX + 11, cy, boxL, cy);
                }
                else g.DrawLine(arrow, sumX - 12, cy, boxL, cy);

                // G(s) のブロック
                var box = new RectangleF(boxL, cy - 44, boxR - boxL, 88);
                using (var p = Theme.Round(box, 10))
                {
                    using (var bb = new SolidBrush(Color.FromArgb(246, 249, 255))) g.FillPath(bb, p);
                    using (var bp = new Pen(Theme.Accent, 2f)) g.DrawPath(bp, p);
                }
                float fw;
                DrawFraction(g, (boxL + boxR) / 2, cy, N(Model.K), DenominatorText(false), ff, ab, out fw);

                g.DrawLine(line, boxR, cy, outX - 40, cy);
                g.DrawLine(arrow, outX - 40, cy, outX, cy);
                g.DrawString("Y(s)", lf, tb, outX - 18, cy - 30);

                if (Model.Feedback)
                {
                    float fy = cy + 64;
                    g.FillEllipse(tb, outX - 44, cy - 4, 8, 8);
                    g.DrawLine(line, outX - 40, cy, outX - 40, fy);
                    g.DrawLine(line, outX - 40, fy, sumX, fy);
                    g.DrawLine(arrow, sumX, fy, sumX, cy + 12);
                }

                // 閉ループ伝達関数
                float tx = r.X + 560;
                using (var pen = new Pen(Theme.Border)) g.DrawLine(pen, tx - 24, r.Y + 20, tx - 24, r.Bottom - 20);
                using (var hf = F(9, FontStyle.Bold))
                    g.DrawString(Model.Feedback ? "閉ループ伝達関数" : "伝達関数（開ループ）", hf, mb, tx, r.Y + 14);
                using (var lf2 = F(12, FontStyle.Italic, Theme.NumFont))
                    g.DrawString("Y/R =", lf2, tb, tx, cy - 11);
                DrawFraction(g, tx + 64 + 70, cy, N(Model.K), DenominatorText(true), ff, tb, out fw);
            }
            line.Dispose(); arrow.Dispose();
        }

        static double NiceStep(double range, int target)
        {
            double raw = range / target;
            double mag = Math.Pow(10, Math.Floor(Math.Log10(raw)));
            double n = raw / mag;
            double s = n < 1.5 ? 1 : n < 3 ? 2 : n < 7 ? 5 : 10;
            return s * mag;
        }

        void DrawChart(Graphics g, RectangleF r)
        {
            Theme.DrawCard(g, r);
            var m = Model;
            using (var hf = F(11, FontStyle.Bold))
            using (var sf = F(9))
            using (var tb = new SolidBrush(Theme.Text))
            using (var mb = new SolidBrush(Theme.Muted))
            {
                g.DrawString(m.Input == InputKind.Step ? "単位ステップ応答  y(t)" : "単位インパルス応答  y(t)", hf, tb, r.X + 20, r.Y + 14);

                // 凡例
                float lx = r.Right - 230, ly = r.Y + 24;
                using (var p = new Pen(Theme.Accent, 2.5f)) g.DrawLine(p, lx, ly, lx + 22, ly);
                g.DrawString("現在", sf, mb, lx + 28, ly - 9);
                if (ghostY != null)
                {
                    using (var p = new Pen(Color.FromArgb(170, 176, 188), 2f) { DashStyle = DashStyle.Dash }) g.DrawLine(p, lx + 72, ly, lx + 94, ly);
                    g.DrawString("変更前", sf, mb, lx + 100, ly - 9);
                }
                if (m.Input == InputKind.Step)
                {
                    using (var p = new Pen(Theme.Warm, 1.5f) { DashStyle = DashStyle.Dot }) g.DrawLine(p, lx + 156, ly, lx + 178, ly);
                    g.DrawString("定常値", sf, mb, lx + 184, ly - 9);
                }
            }

            var pa = new RectangleF(r.X + 64, r.Y + 56, r.Width - 92, r.Height - 100);

            double ymin = Math.Min(0, m.Ys.Min()), ymax = Math.Max(m.Ys.Max(), m.FinalValue);
            if (ghostY != null) { ymin = Math.Min(ymin, ghostY.Min()); ymax = Math.Max(ymax, ghostY.Max()); }
            if (ymax - ymin < 1e-9) ymax = ymin + 1;
            double ystep = NiceStep((ymax - ymin) * 1.1, 5);
            ymin = Math.Floor(ymin / ystep) * ystep;
            ymax = Math.Ceiling(ymax * 1.06 / ystep) * ystep;
            double xstep = NiceStep(m.TMax, 8);

            Func<double, float> X = t => (float)(pa.X + t / m.TMax * pa.Width);
            Func<double, float> Y = v => (float)(pa.Bottom - (v - ymin) / (ymax - ymin) * pa.Height);

            using (var gp = new Pen(Theme.Grid, 1))
            using (var nf = F(9, FontStyle.Regular, Theme.NumFont))
            using (var mb = new SolidBrush(Theme.Muted))
            {
                var right = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };
                var center = new StringFormat { Alignment = StringAlignment.Center };
                for (double v = ymin; v <= ymax + 1e-9; v += ystep)
                {
                    g.DrawLine(gp, pa.X, Y(v), pa.Right, Y(v));
                    g.DrawString(v.ToString("0.###"), nf, mb, new RectangleF(pa.X - 60, Y(v) - 10, 52, 20), right);
                }
                for (double t = 0; t <= m.TMax + 1e-9; t += xstep)
                {
                    g.DrawLine(gp, X(t), pa.Y, X(t), pa.Bottom);
                    g.DrawString(t.ToString("0.##"), nf, mb, new RectangleF(X(t) - 30, pa.Bottom + 6, 60, 20), center);
                }
                g.DrawString("t [s]", nf, mb, pa.Right - 30, pa.Bottom + 22);
                using (var ap = new Pen(Color.FromArgb(150, 158, 170), 1.2f)) g.DrawLine(ap, pa.X, Y(0), pa.Right, Y(0));
            }

            var clip = g.Clip; g.SetClip(new RectangleF(pa.X - 2, pa.Y - 8, pa.Width + 4, pa.Height + 10));

            // 変更前の応答（グレーの破線）
            if (ghostY != null)
            {
                var gpts = new List<PointF>();
                for (int k = 0; k < ghostT.Length; k++) if (ghostT[k] <= m.TMax) gpts.Add(new PointF(X(ghostT[k]), Y(ghostY[k])));
                if (gpts.Count > 1)
                    using (var p = new Pen(Color.FromArgb(170, 176, 188), 2f) { DashStyle = DashStyle.Dash }) g.DrawLines(p, gpts.ToArray());
            }

            var pts = m.Ts.Select((t, k) => new PointF(X(t), Y(m.Ys[k]))).ToArray();
            // 塗りつぶし
            using (var path = new GraphicsPath())
            {
                path.AddLine(X(0), Y(0), pts[0].X, pts[0].Y);
                path.AddLines(pts);
                path.AddLine(pts[pts.Length - 1].X, pts[pts.Length - 1].Y, X(m.TMax), Y(0));
                path.CloseFigure();
                using (var b = new LinearGradientBrush(new PointF(0, pa.Y), new PointF(0, pa.Bottom), Color.FromArgb(60, Theme.Accent), Color.FromArgb(4, Theme.Accent)))
                    g.FillPath(b, path);
            }
            using (var p = new Pen(Theme.Accent, 2.6f) { LineJoin = LineJoin.Round }) g.DrawLines(p, pts);

            if (m.Input == InputKind.Step)
                using (var p = new Pen(Theme.Warm, 1.5f) { DashStyle = DashStyle.Dot }) g.DrawLine(p, pa.X, Y(m.FinalValue), pa.Right, Y(m.FinalValue));
            g.Clip = clip;

            // ピーク注記
            bool showPeak = m.Input == InputKind.Impulse || m.OvershootPercent > 0.5;
            if (showPeak && m.PeakTime < m.TMax * 0.98)
            {
                float px = X(m.PeakTime), py = Y(m.PeakValue);
                using (var b = new SolidBrush(Theme.Accent)) g.FillEllipse(b, px - 5, py - 5, 10, 10);
                g.FillEllipse(Brushes.White, px - 2.5f, py - 2.5f, 5, 5);
                string label = m.Input == InputKind.Step
                    ? string.Format("最大 {0:0.###}（+{1:0.0}%）", m.PeakValue, m.OvershootPercent)
                    : string.Format("ピーク {0:0.###}", m.PeakValue);
                using (var f = F(9, FontStyle.Bold))
                {
                    var s = g.MeasureString(label, f);
                    float bx = Math.Min(px + 10, pa.Right - s.Width - 8), by = Math.Max(pa.Y, py - s.Height - 10);
                    var br = new RectangleF(bx, by, s.Width + 8, s.Height + 2);
                    using (var p = Theme.Round(br, 5))
                    using (var bb = new SolidBrush(Color.FromArgb(235, 28, 34, 46))) g.FillPath(bb, p);
                    g.DrawString(label, f, Brushes.White, bx + 4, by + 1);
                }
            }
        }

        static string PoleText(Complex[] poles)
        {
            var parts = new List<string>();
            foreach (var p in poles.OrderByDescending(z => z.Imaginary))
            {
                if (Math.Abs(p.Imaginary) < 1e-6) parts.Add(p.Real.ToString("0.###").Replace("-", "−"));
                else if (p.Imaginary > 0) parts.Add(string.Format("{0} ± {1}j", p.Real.ToString("0.###").Replace("-", "−"), p.Imaginary.ToString("0.###")));
            }
            return string.Join(",  ", parts);
        }

        void DrawMetrics(Graphics g, RectangleF r)
        {
            var m = Model;
            var tiles = new List<string[]>
            {
                new[] { "極（DK法で計算）", PoleText(m.ClosedLoopPoles) },
                new[] { "定常値", m.FinalValue.ToString("0.###") },
                new[] { "最大行き過ぎ量", m.Input == InputKind.Step ? (m.OvershootPercent < 0.05 ? "0 %" : m.OvershootPercent.ToString("0.0") + " %") : "—" },
                new[] { "2% 整定時間", double.IsNaN(m.SettlingTime) ? "> t_max" : m.SettlingTime.ToString("0.00") + " s" },
                m.Order == 2
                    ? new[] { "減衰比 ζ ／ 固有角周波数 ωn", string.Format("{0:0.00} ／ {1:0.00}", m.Zeta, m.Wn) }
                    : new[] { "時定数（" + (m.Feedback ? "閉ループ" : "開ループ") + "）", m.TimeConstant.ToString("0.00") + " s" },
            };
            float gap = 12;
            float[] weights = { 1.45f, 0.8f, 0.9f, 0.9f, 1.25f };
            float total = weights.Sum(), avail = r.Width - gap * (tiles.Count - 1), x = r.X;
            using (var lf = F(9))
            using (var vf = F(15, FontStyle.Bold, Theme.NumFont))
            using (var vfs = F(12, FontStyle.Bold, Theme.NumFont))
            using (var mb = new SolidBrush(Theme.Muted))
            using (var tb = new SolidBrush(Theme.Text))
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    float w = avail * weights[i] / total;
                    var tr = new RectangleF(x, r.Y, w, r.Height);
                    Theme.DrawCard(g, tr, 10);
                    g.DrawString(tiles[i][0], lf, mb, tr.X + 16, tr.Y + 16);
                    var font = g.MeasureString(tiles[i][1], vf).Width > w - 28 ? vfs : vf;
                    g.DrawString(tiles[i][1], font, tb, new RectangleF(tr.X + 14, tr.Y + 44, w - 24, tr.Height - 50));
                    x += w + gap;
                }
            }
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
