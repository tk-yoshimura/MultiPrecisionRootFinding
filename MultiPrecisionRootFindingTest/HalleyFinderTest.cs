using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiPrecision;
using MultiPrecisionRootFinding;

namespace MultiPrecisionRootFindingTest {
    [TestClass]
    public class HalleyFinderTest {
        [TestMethod]
        public void CbrtTest() {
            static (MultiPrecision<Pow2.N8> v, MultiPrecision<Pow2.N8> d1, MultiPrecision<Pow2.N8> d2) f(MultiPrecision<Pow2.N8> x) {
                return (x * x * x - 2, 3 * x * x, 6 * x);
            }

            MultiPrecision<Pow2.N8> y = HalleyFinder<Pow2.N8>.RootFind(f, x0: 2);

            Assert.IsTrue(MultiPrecision<Pow2.N8>.NearlyEqualBits(y, MultiPrecision<Pow2.N8>.Cbrt(2), 1));
        }
    }
}