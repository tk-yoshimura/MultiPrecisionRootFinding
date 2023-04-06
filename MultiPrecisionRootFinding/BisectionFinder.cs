using MultiPrecision;

namespace MultiPrecisionRootFinding {
    public static class BisectionFinder<N> where N : struct, IConstant {
        public static MultiPrecision<N> RootFind(
            Func<MultiPrecision<N>, MultiPrecision<N>> f,
            MultiPrecision<N> x1, MultiPrecision<N> x2,
            int iters = -1, int accurate_bits = -1) {

            if (accurate_bits > MultiPrecision<N>.Bits - 2) {
                throw new ArgumentOutOfRangeException(nameof(accurate_bits), $"{nameof(accurate_bits)} <= {MultiPrecision<N>.Bits - 2}");
            }

            accurate_bits = accurate_bits > 0 ? accurate_bits : MultiPrecision<N>.Bits - 2;
            bool convergenced = false;

            MultiPrecision<N> x = MultiPrecision<N>.Ldexp(x1 + x2, -1), y1 = f(x1), y2 = f(x2);

            while (iters != 0) {
                (x1, y1, x2, y2, bool success) = Iteration(f, x1, y1, x2, y2);

                x = MultiPrecision<N>.Ldexp(x1 + x2, -1);
                MultiPrecision<N> dx = x1 - x2;

                if (dx.IsZero || x.Exponent - dx.Exponent > accurate_bits) {
                    if (convergenced) {
                        break;
                    }

                    convergenced = true;
                }
                else {
                    convergenced = false;
                }

                if (!success) {
                    break;
                }

                iters = Math.Max(-1, iters - 1);
            }

            return x;
        }

        public static (MultiPrecision<N> x1, MultiPrecision<N> y1, MultiPrecision<N> x2, MultiPrecision<N> y2, bool success) Iteration(Func<MultiPrecision<N>, MultiPrecision<N>> f, MultiPrecision<N> x1, MultiPrecision<N> y1, MultiPrecision<N> x2, MultiPrecision<N> y2) {
            if (y1.Sign == y2.Sign) {
                return (x1, y1, x2, y2, success: false);
            }

            MultiPrecision<N> xc = MultiPrecision<N>.Ldexp(x1 + x2, -1);
            MultiPrecision<N> yc = f(xc);

            if (xc == x1 || xc == x2) {
                return (xc, yc, xc, yc, success: false);
            }

            if (y1.Sign == yc.Sign) {
                return (xc, yc, x2, y2, success: true);
            }
            else {
                return (x1, y1, xc, yc, success: true);
            }
        }
    }
}
