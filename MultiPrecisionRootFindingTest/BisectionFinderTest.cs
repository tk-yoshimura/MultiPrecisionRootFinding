using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiPrecision;
using MultiPrecisionRootFinding;

namespace MultiPrecisionRootFindingTest {
    [TestClass]
    public class BisectionFinderTest {
        [TestMethod]
        public void CbrtTest() {
            static MultiPrecision<Pow2.N8> f(MultiPrecision<Pow2.N8> x) {
                return x * x * x - 2;
            }

            MultiPrecision<Pow2.N8> y = BisectionFinder<Pow2.N8>.RootFind(f, 1, 1.5);

            Assert.IsTrue(MultiPrecision<Pow2.N8>.NearlyEqualBits(y, MultiPrecision<Pow2.N8>.Cbrt(2), 1));
        }
    }
}