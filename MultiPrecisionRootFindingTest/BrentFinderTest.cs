using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiPrecision;
using MultiPrecisionRootFinding;

namespace MultiPrecisionRootFindingTest {
    [TestClass]
    public class BrentFinderTest {
        [TestMethod]
        public void CubicTest() {
            static MultiPrecision<Pow2.N8> f(MultiPrecision<Pow2.N8> x) {
                return (x + 3) * MultiPrecision<Pow2.N8>.Square(x - 1);
            }

            MultiPrecision<Pow2.N8> y = BrentFinder<Pow2.N8>.RootFind(f, x1: -4, x2: 4d / 3);

            Assert.IsTrue(MultiPrecision<Pow2.N8>.NearlyEqualBits(y, -3, 1));
        }

        [TestMethod]
        public void CbrtTest() {
            static MultiPrecision<Pow2.N8> f(MultiPrecision<Pow2.N8> x) {
                return x * x * x - 2;
            }

            MultiPrecision<Pow2.N8> y = BrentFinder<Pow2.N8>.RootFind(f, x1: 1, x2: 2);

            Assert.IsTrue(MultiPrecision<Pow2.N8>.NearlyEqualBits(y, MultiPrecision<Pow2.N8>.Cbrt(2), 1));
        }

        [TestMethod]
        public void InvGammaTest() {
            static MultiPrecision<Pow2.N8> f(MultiPrecision<Pow2.N8> x) {
                return MultiPrecision<Pow2.N8>.Gamma(x) - 63;
            }

            MultiPrecision<Pow2.N8> y = BrentFinder<Pow2.N8>.RootFind(f, x1: 4, x2: 7);

            Assert.IsTrue(MultiPrecision<Pow2.N8>.NearlyEqualBits(MultiPrecision<Pow2.N8>.Gamma(y), 63, 3));
        }

        [TestMethod]
        public void InvErfTest() {
            static MultiPrecision<Pow2.N8> f(MultiPrecision<Pow2.N8> x) {
                return MultiPrecision<Pow2.N8>.Erf(x) - 0.75;
            }

            MultiPrecision<Pow2.N8> y = BrentFinder<Pow2.N8>.RootFind(f, x1: 0, x2: 4);

            Assert.IsTrue(MultiPrecision<Pow2.N8>.NearlyEqualBits(y, MultiPrecision<Pow2.N8>.InverseErf(0.75), 2));
        }
    }
}