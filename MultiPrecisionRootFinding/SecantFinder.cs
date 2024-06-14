using MultiPrecision;

namespace MultiPrecisionRootFinding {
    public static class SecantFinder<N> where N : struct, IConstant {
        public static MultiPrecision<N> RootFind(
            Func<MultiPrecision<N>, MultiPrecision<N>> f,
            MultiPrecision<N> x0,
            int iters = -1, int accurate_bits = -1, bool overshoot_decay = true, bool divergence_decay = true) {

            if (!MultiPrecision<N>.IsFinite(x0)) {
                return MultiPrecision<N>.NaN;
            }

            if (accurate_bits > MultiPrecision<N>.Bits - 4) {
                throw new ArgumentOutOfRangeException(nameof(accurate_bits), $"{nameof(accurate_bits)} <= {MultiPrecision<N>.Bits - 4}");
            }

            accurate_bits = accurate_bits > 0 ? accurate_bits : MultiPrecision<N>.Bits - 2;
            bool convergenced = false;

            MultiPrecision<N> x = x0;
            MultiPrecision<N> h = MultiPrecision<N>.Max(
                MultiPrecision<N>.Ldexp(1, -MultiPrecision<N>.Bits),
                MultiPrecision<N>.Ldexp(MultiPrecision<N>.Abs(x), -4)
            );

            MultiPrecision<N> x_prev = x + h, y_prev = f(x_prev);

            MultiPrecision<N>? dx_prev = null;

            while (iters != 0) {
                MultiPrecision<N> y = f(x);
                MultiPrecision<N> dx = Delta(x, y, x_prev, y_prev);

                if (MultiPrecision<N>.IsNaN(dx)) {
                    break;
                }

                if (dx_prev is not null) {
                    if (divergence_decay && MultiPrecision<N>.Abs(dx) > MultiPrecision<N>.Abs(dx_prev)) {
                        dx = (dx.Sign == Sign.Plus ? +1 : -1) * MultiPrecision<N>.Abs(dx_prev) / 2;
                    }
                    if (overshoot_decay && dx.Sign != dx_prev.Sign) {
                        dx = MultiPrecision<N>.Ldexp(dx, -1);
                    }
                }

                (x_prev, y_prev) = (x, y);

                x += dx;
                dx_prev = dx;

                if (MultiPrecision<N>.IsZero(dx) || x.Exponent - dx.Exponent > accurate_bits) {
                    if (convergenced) {
                        break;
                    }

                    convergenced = true;
                }
                else {
                    convergenced = false;
                }

                if (!MultiPrecision<N>.IsFinite(x)) {
                    break;
                }

                iters = Math.Max(-1, iters - 1);
            }

            return x;
        }

        public static MultiPrecision<N> Delta(MultiPrecision<N> x, MultiPrecision<N> y, MultiPrecision<N> x_prev, MultiPrecision<N> y_prev) {
            MultiPrecision<N> dx = y * (x_prev - x) / (y - y_prev);

            return dx;
        }
    }
}
