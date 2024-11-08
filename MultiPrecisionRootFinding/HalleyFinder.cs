using MultiPrecision;

namespace MultiPrecisionRootFinding {
    public static class HalleyFinder<N> where N : struct, IConstant {

        public static MultiPrecision<N> RootFind(
            Func<MultiPrecision<N>, (MultiPrecision<N> v, MultiPrecision<N> d1, MultiPrecision<N> d2)> f,
            MultiPrecision<N> x0,
            int iters = -1, int accurate_bits = -1, bool overshoot_decay = true) {

            if (accurate_bits > MultiPrecision<N>.Bits - 4) {
                throw new ArgumentOutOfRangeException(nameof(accurate_bits), $"{nameof(accurate_bits)} <= {MultiPrecision<N>.Bits - 4}");
            }

            if (!MultiPrecision<N>.IsFinite(x0)) {
                return MultiPrecision<N>.NaN;
            }

            MultiPrecision<N> x = x0, dx;
            MultiPrecision<N>? dx_prev = null;

            accurate_bits = accurate_bits > 0 ? accurate_bits : MultiPrecision<N>.Bits - 4;
            bool convergenced = false;

            while (iters != 0) {
                dx = Delta(f, x);

                if (overshoot_decay && dx_prev is not null) {
                    if (dx.Sign != dx_prev.Sign) {
                        dx = MultiPrecision<N>.Ldexp(dx, -2);
                    }
                }

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

                iters = int.Max(-1, iters - 1);
            }

            return x;
        }

        public static MultiPrecision<N> RootFind(
            Func<MultiPrecision<N>, (MultiPrecision<N> v, MultiPrecision<N> d1, MultiPrecision<N> d2)> f,
            MultiPrecision<N> x0, (MultiPrecision<N> min, MultiPrecision<N> max) xrange,
            int iters = -1, int accurate_bits = -1, bool overshoot_decay = true) {

            if (accurate_bits > MultiPrecision<N>.Bits - 4) {
                throw new ArgumentOutOfRangeException(nameof(accurate_bits), $"{nameof(accurate_bits)} <= {MultiPrecision<N>.Bits - 4}");
            }
            if (!(xrange.min < xrange.max)) {
                throw new ArgumentException("Invalid range.", nameof(xrange));
            }

            MultiPrecision<N> x = x0, dx;
            MultiPrecision<N>? dx_prev = null;

            accurate_bits = accurate_bits > 0 ? accurate_bits : MultiPrecision<N>.Bits - 4;
            bool convergenced = false;

            while (iters != 0) {
                dx = Delta(f, x);

                if (overshoot_decay && dx_prev is not null) {
                    if (dx.Sign != dx_prev.Sign) {
                        dx = MultiPrecision<N>.Ldexp(dx, -1);
                    }
                }

                x += dx;
                dx_prev = dx;

                if ((x == xrange.min && dx.Sign == Sign.Minus) || (x == xrange.max && dx.Sign == Sign.Plus)) {
                    return MultiPrecision<N>.NaN;
                }

                if (x < xrange.min) {
                    x = xrange.min;
                }
                else if (x > xrange.max) {
                    x = xrange.max;
                }

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

                iters = int.Max(-1, iters - 1);
            }

            return x;
        }

        public static MultiPrecision<N> Delta(
            Func<MultiPrecision<N>, (MultiPrecision<N> v, MultiPrecision<N> d1, MultiPrecision<N> d2)> f,
            MultiPrecision<N> x) {

            (MultiPrecision<N> v, MultiPrecision<N> d1, MultiPrecision<N> d2) = f(x);

            MultiPrecision<N> dx = MultiPrecision<N>.Ldexp(v * d1, 1) / (v * d2 - MultiPrecision<N>.Ldexp(d1 * d1, 1));

            return dx;
        }
    }
}