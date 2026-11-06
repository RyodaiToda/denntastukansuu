// [AI生成] UI再設計版の計算部分。
// 根の計算にはオリジナルの DK.cs（Durand–Kerner 法）と Polynomial.cs をそのまま使い、
// 逆ラプラス変換（留数計算）だけをこのファイルで行う。
using System;
using System.Linq;
using System.Numerics;

namespace dentastu
{
    public enum InputKind { Impulse, Step }

    public class ResponseModel
    {
        public InputKind Input = InputKind.Step;
        public int Order = 1;          // 1: 1次遅れ, 2: 2次遅れ
        public double K = 1.0;         // ゲイン
        public double T = 1.0;         // 1次遅れ: 1 + T s
        public double A = 0.4;         // 2次遅れ: 1 + A s + B s^2
        public double B = 1.0;
        public bool Feedback = true;   // 単位負帰還
        public double TMax = 10.0;

        // ---- 計算結果 ----
        public Complex[] ClosedLoopPoles = new Complex[0];
        public double[] Ts = new double[0];
        public double[] Ys = new double[0];
        public double FinalValue;
        public double PeakValue, PeakTime;
        public double OvershootPercent;   // ステップ応答のみ
        public double SettlingTime;       // 2% 整定時間（t_max 内に整定しなければ NaN）
        public double Zeta = double.NaN, Wn = double.NaN, TimeConstant = double.NaN;

        Complex[] poles = new Complex[0];
        Complex[] residues = new Complex[0];

        /// <summary>開ループ/閉ループ伝達関数の分母係数（昇べき順）</summary>
        public double[] Denominator()
        {
            double[] den = Order == 1 ? new[] { 1.0, T } : new[] { 1.0, A, B };
            if (Feedback) den[0] += K;   // K / (D(s) + K)
            return den;
        }

        public void Compute(int samples = 600)
        {
            double[] den = Denominator();
            double[] num = { K };

            // Y(s) = K / (D(s) * U(s)^-1)   … インパルス: U=1, ステップ: U=1/s
            var denP = new Polynomial(den);
            var inP = new Polynomial(Input == InputKind.Step ? new[] { 0.0, 1.0 } : new[] { 1.0 });
            var yDen = Polynomial.Multiplication(denP, inP).GetCoef();

            poles = FindRoots(yDen);
            ClosedLoopPoles = FindRoots(den);

            // 留数: c_i = N(p_i) / (a_n * Π_{j≠i}(p_i - p_j))   ← 最高次係数 a_n で割るのを忘れない
            double an = yDen[yDen.Length - 1];
            residues = new Complex[poles.Length];
            for (int i = 0; i < poles.Length; i++)
            {
                Complex prod = an;
                for (int j = 0; j < poles.Length; j++)
                    if (j != i) prod *= poles[i] - poles[j];
                residues[i] = num[0] / prod;
            }

            Ts = new double[samples + 1];
            Ys = new double[samples + 1];
            for (int k = 0; k <= samples; k++)
            {
                Ts[k] = TMax * k / samples;
                Ys[k] = Eval(Ts[k]);
            }

            // 最終値の定理
            FinalValue = Input == InputKind.Step ? num[0] / den[0] : 0.0;

            int ip = 0;
            for (int k = 1; k < Ys.Length; k++) if (Math.Abs(Ys[k]) > Math.Abs(Ys[ip])) ip = k;
            PeakValue = Ys[ip]; PeakTime = Ts[ip];
            OvershootPercent = (Input == InputKind.Step && FinalValue > 0)
                ? Math.Max(0, (Ys.Max() - FinalValue) / FinalValue * 100.0) : double.NaN;

            double band = 0.02 * (Input == InputKind.Step ? Math.Abs(FinalValue) : Math.Abs(PeakValue));
            int last = -1;
            for (int k = 0; k < Ys.Length; k++) if (Math.Abs(Ys[k] - FinalValue) > band) last = k;
            SettlingTime = last >= Ys.Length - 1 ? double.NaN : Ts[Math.Min(last + 1, Ys.Length - 1)];

            if (Order == 2)
            {
                Wn = Math.Sqrt(den[0] / den[2]);
                Zeta = den[1] / (2.0 * den[2] * Wn);
                TimeConstant = double.NaN;
            }
            else
            {
                TimeConstant = den[1] / den[0];
                Zeta = Wn = double.NaN;
            }
        }

        public double Eval(double t)
        {
            Complex sum = Complex.Zero;
            for (int i = 0; i < poles.Length; i++) sum += residues[i] * Complex.Exp(poles[i] * t);
            return sum.Real;
        }

        /// <summary>オリジナルの DK クラスで多項式の根を求める</summary>
        static Complex[] FindRoots(double[] coef)
        {
            int n = coef.Length - 1;
            // 初期値の半径: Cauchy の上界 1 + max|a_i / a_n|（オリジナルは r = 5 固定）
            double r = 1.0;
            for (int i = 0; i < n; i++) r = Math.Max(r, 1.0 + Math.Abs(coef[i] / coef[n]));

            var dk = new DK(coef);
            dk.SetInitialValue(r);
            var stdout = Console.Out;
            Console.SetOut(System.IO.TextWriter.Null);   // DK.Calclate のコンソール出力を抑制
            try { dk.Calclate(200); } finally { Console.SetOut(stdout); }
            return (Complex[])dk.GetZ().Clone();
        }
    }
}
