using MultiPrecision;

namespace MultiPrecisionRootFinding {
    public static class BrentFinder<N> where N : struct, IConstant {
        public static MultiPrecision<N> RootFind(
            Func<MultiPrecision<N>, MultiPrecision<N>> f,
            MultiPrecision<N> x1, MultiPrecision<N> x2,
            int iters = -1, int accurate_bits = -1) {

            if (accurate_bits > MultiPrecision<N>.Bits - 2) {
                throw new ArgumentOutOfRangeException(nameof(accurate_bits), $"{nameof(accurate_bits)} <= 1e-30");
            }

            if (!MultiPrecision<N>.IsFinite(x1) || !MultiPrecision<N>.IsFinite(x2)) {
                return MultiPrecision<N>.NaN;
            }

            accurate_bits = accurate_bits > 0 ? accurate_bits : MultiPrecision<N>.Bits - 2;

            MultiPrecision<N> y1 = f(x1), y2 = f(x2), xc = 0d, yc = 0d;

            if (MultiPrecision<N>.IsZero(y1)) {
                return x1;
            }
            if (MultiPrecision<N>.IsZero(y2)) {
                return x2;
            }

            if (y1.Sign == y2.Sign) {
                throw new ArithmeticException($"invalid interval: sgn(f(x1)) == sgn(f(x2))");
            }

            MultiPrecision<N> dx_prev = 0, dx = 0;

            while (iters != 0) {
                if (!MultiPrecision<N>.IsZero(y1) && !MultiPrecision<N>.IsZero(y2) && (y1.Sign != y2.Sign)) {
                    (xc, yc) = (x1, y1);
                    dx_prev = dx = x2 - x1;
                }

                if (MultiPrecision<N>.Abs(yc) < MultiPrecision<N>.Abs(y2)) {
                    (x2, x1, xc) = (xc, x2, x2);
                    (y2, y1, yc) = (yc, y2, y2);
                }

                MultiPrecision<N> delta = MultiPrecision<N>.Max(MultiPrecision<N>.Epsilon, MultiPrecision<N>.Ldexp(MultiPrecision<N>.Abs(x2), -accurate_bits));
                MultiPrecision<N> dx_bisect = MultiPrecision<N>.Ldexp(xc - x2, -1), dx_interp;

                if (MultiPrecision<N>.IsZero(y2) || !(MultiPrecision<N>.Abs(dx_bisect) >= delta)) {
                    return x2;
                }

                if (MultiPrecision<N>.Abs(dx_prev) > delta && MultiPrecision<N>.Abs(y2) < MultiPrecision<N>.Abs(y1)) {
                    if (x1 != xc) {
                        MultiPrecision<N> rab = (y1 - y2) / (x1 - x2);
                        MultiPrecision<N> rcb = (yc - y2) / (xc - x2);
                        dx_interp = -y2 * (yc * rcb - y1 * rab) / (rcb * rab * (yc - y1));
                    }
                    else {
                        dx_interp = -y2 * (x2 - x1) / (y2 - y1);
                    }

                    if (MultiPrecision<N>.Ldexp(MultiPrecision<N>.Abs(dx_interp), 1) < MultiPrecision<N>.Min(MultiPrecision<N>.Abs(dx_prev), 3 * MultiPrecision<N>.Abs(dx_bisect) - delta)) {
                        dx_prev = dx;
                        dx = dx_interp;
                    }
                    else {
                        dx_prev = dx = dx_bisect;
                    }
                }
                else {
                    dx_prev = dx = dx_bisect;
                }

                (x1, y1) = (x2, y2);

                if (MultiPrecision<N>.Abs(dx) > delta) {
                    x2 += dx;
                }
                else {
                    if (!MultiPrecision<N>.IsFinite(dx_bisect)) {
                        return x2;
                    }

                    x2 += dx_bisect.Sign == Sign.Plus ? delta : -delta;
                }

                y2 = f(x2);

                iters = Math.Max(-1, iters - 1);
            }

            return x2;
        }
    }
}
