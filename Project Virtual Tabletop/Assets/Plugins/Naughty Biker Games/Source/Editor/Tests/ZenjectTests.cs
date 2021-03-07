using NUnit.Framework;
using Zenject;

namespace NaughtyBikerGames.SDK.Editor.Tests {
	public abstract class ZenjectTests : ZenjectUnitTestFixture {
        [TearDown]
        public virtual void TearDown() {
            Container.UnbindAll();
        }
	}
}