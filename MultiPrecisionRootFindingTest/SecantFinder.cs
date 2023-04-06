using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiPrecision;
using MultiPrecisionRootFinding;

namespace MultiPrecisionRootFindingTest {
    [TestClass]
    public class SecantFinderTest {
        [TestMethod]
        public void CbrtTest() {
            static MultiPrecision<Pow2.N8> f(MultiPrecision<Pow2.N8> x) {
                return x * x * x - 2;
            }

            MultiPrecision<Pow2.N8> y = SecantFinder<Pow2.N8>.RootFind(f, x0: 1);

            Assert.IsTrue(MultiPrecision<Pow2.N8>.NearlyEqualBits(y, MultiPrecision<Pow2.N8>.Cbrt(2), 1));
        }
    }
}