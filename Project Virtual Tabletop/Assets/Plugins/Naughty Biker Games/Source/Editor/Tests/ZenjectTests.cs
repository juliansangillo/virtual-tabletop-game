using NUnit.Framework;
using Zenject;

namespace NaughtyBiker.Editor.Tests {
	public class ZenjectTests : ZenjectUnitTestFixture {
        [TearDown]
        public void TearDown() {
            Container.UnbindAll();
        }
	}
}