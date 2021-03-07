using System;
using NSubstitute;
using NUnit.Framework;
using NaughtyBikerGames.SDK.LevelManagement;
using NaughtyBikerGames.SDK.LevelManagement.Installers;
using NaughtyBikerGames.SDK.LevelManagement.Interfaces;
using NaughtyBikerGames.SDK.Adapters.Interfaces;

namespace NaughtyBikerGames.SDK.Editor.Tests.LevelManagement {
	public class LevelManagerTests : ZenjectTests {
		ISceneManager sceneManagerMock;
		ISceneUtility sceneUtilityMock;

		[SetUp]
		public void SetUp() {
			LevelManagerBaseInstaller.Install(Container);

			sceneManagerMock = Substitute.For<ISceneManager>();
			sceneUtilityMock = Substitute.For<ISceneUtility>();

			Container.Bind<ISceneManager>()
                .FromInstance(sceneManagerMock)
                .AsSingle()
                .WhenInjectedInto<LevelManager>()
                .NonLazy();
            Container.Bind<ISceneUtility>()
                .FromInstance(sceneUtilityMock)
                .AsSingle()
                .WhenInjectedInto<LevelManager>()
                .NonLazy();
		}

		[Test]
		public void Initialize_TwoScenesOneWithPascalCaseAndOneWithUnderscoresInName_SetLabelsCorrectly() {
			string nameWithPascalCase = "FakeAndNonExistentLevel0Foo";
			string nameWithUnderscores = "fake_and_non_existent_level_1Foo";
			sceneManagerMock.sceneCountInBuildSettings.Returns(2);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(1);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(nameWithPascalCase);
			sceneUtilityMock.GetScenePathByBuildIndex(1).Returns(nameWithUnderscores);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;

			levelManager.Initialize();

			Assert.AreEqual("Fake And Non Existent Level 0 Foo", levelManager.GetLevelLabel(nameWithPascalCase));
			Assert.AreEqual("fake and non existent level 1 Foo", levelManager.GetLevelLabel(nameWithUnderscores));
		}

		[Test]
		public void Initialize_TwoScenesWithNames_MapLevelNameToCorrectBuildIndex() {
			string name1 = "Level0";
			string name2 = "Level1";
			sceneManagerMock.sceneCountInBuildSettings.Returns(2);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(1);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			sceneUtilityMock.GetScenePathByBuildIndex(1).Returns(name2);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;

			levelManager.Initialize();

			Assert.AreEqual(0, levelManager.GetLevel(name1));
			Assert.AreEqual(1, levelManager.GetLevel(name2));
		}

		[Test]
		public void Initialize_TwoScenesWithNames_SetFirstLevelToLevel0() {
			string name1 = "Level0";
			string name2 = "Level1";
			sceneManagerMock.sceneCountInBuildSettings.Returns(2);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(1);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			sceneUtilityMock.GetScenePathByBuildIndex(1).Returns(name2);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;

			levelManager.Initialize();

			Assert.AreEqual("Level0", levelManager.FirstLevel);
		}

		[Test]
		public void Initialize_ActiveSceneIsCurrentlyIndex1_SetActiveLevelTo1() {
			string name1 = "Level0";
			string name2 = "Level1";
			sceneManagerMock.sceneCountInBuildSettings.Returns(2);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(1);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			sceneUtilityMock.GetScenePathByBuildIndex(1).Returns(name2);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;

			levelManager.Initialize();

			Assert.AreEqual(1, levelManager.ActiveLevel);
		}

		[Test]
		public void GetLevel_GivenExistingLevelName_ReturnCorrectIndex() {
			string name1 = "Level0";
			string name2 = "Level1";
			string name3 = "Level2";
			sceneManagerMock.sceneCountInBuildSettings.Returns(3);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			sceneUtilityMock.GetScenePathByBuildIndex(1).Returns(name2);
			sceneUtilityMock.GetScenePathByBuildIndex(2).Returns(name3);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			int actual1 = levelManager.GetLevel("Level0");
			int actual2 = levelManager.GetLevel("Level2");

			Assert.AreEqual(0, actual1);
			Assert.AreEqual(2, actual2);
		}

		[Test]
		public void GetLevel_GivenNullLevelName_ThrowArgumentNullException() {
			string name1 = "Level0";
			sceneManagerMock.sceneCountInBuildSettings.Returns(1);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			Assert.Throws<ArgumentNullException>(() => {
				levelManager.GetLevel(null);
			});
		}

		[Test]
		public void GetLevel_GivenEmptyLevelName_ThrowArgumentNullException() {
			string name1 = "Level0";
			sceneManagerMock.sceneCountInBuildSettings.Returns(1);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			Assert.Throws<ArgumentNullException>(() => {
				levelManager.GetLevel("");
			});
		}

		[Test]
		public void GetLevel_GivenNonExistentLevelName_ThrowArgumentException() {
			string name1 = "Level0";
			sceneManagerMock.sceneCountInBuildSettings.Returns(1);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			Assert.Throws<ArgumentException>(() => {
				levelManager.GetLevel("NonExistentLevelName");
			});
		}

		[Test]
		public void GetLevelLabel_ByIndex_GivenExistingIndex_ReturnCorrectLabel() {
			string name1 = "Level0";
			string name2 = "Level1";
			string name3 = "Level2";
			sceneManagerMock.sceneCountInBuildSettings.Returns(3);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			sceneUtilityMock.GetScenePathByBuildIndex(1).Returns(name2);
			sceneUtilityMock.GetScenePathByBuildIndex(2).Returns(name3);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			string actual1 = levelManager.GetLevelLabel(1);
			string actual2 = levelManager.GetLevelLabel(2);

			Assert.AreEqual("Level 1", actual1);
			Assert.AreEqual("Level 2", actual2);
		}

		[Test]
		public void GetLevelLabel_ByIndex_GivenNegativeIndex_ThrowArgumentOutOfRangeException() {
			string name1 = "Level0";
			sceneManagerMock.sceneCountInBuildSettings.Returns(1);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			Exception e = Assert.Throws<ArgumentOutOfRangeException>(() => {
				levelManager.GetLevelLabel(-1);
			});
			Assert.AreEqual("Level index cannot be a negative!\nParameter name: index\nActual value was -1.", e.Message);
		}

		[Test]
		public void GetLevelLabel_ByIndex_GivenNonExistentIndex_ThrowArgumentOutOfRangeException() {
			string name1 = "Level0";
			sceneManagerMock.sceneCountInBuildSettings.Returns(1);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			Exception e = Assert.Throws<ArgumentOutOfRangeException>(() => {
				levelManager.GetLevelLabel(999);
			});
			Assert.AreEqual("The index doesn't exist!\nParameter name: index\nActual value was 999.", e.Message);
		}

		[Test]
		public void GetLevelLabel_ByName_GivenExistingLevelName_ReturnCorrectLabel() {
			string name1 = "Level0";
			string name2 = "Level1";
			string name3 = "Level2";
			sceneManagerMock.sceneCountInBuildSettings.Returns(3);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			sceneUtilityMock.GetScenePathByBuildIndex(1).Returns(name2);
			sceneUtilityMock.GetScenePathByBuildIndex(2).Returns(name3);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			string actual1 = levelManager.GetLevelLabel("Level1");
			string actual2 = levelManager.GetLevelLabel("Level2");

			Assert.AreEqual("Level 1", actual1);
			Assert.AreEqual("Level 2", actual2);
		}

		[Test]
		public void GetLevelLabel_ByName_GivenNullLevelName_ThrowArgumentNullException() {
			string name1 = "Level0";
			sceneManagerMock.sceneCountInBuildSettings.Returns(1);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			Assert.Throws<ArgumentNullException>(() => {
				levelManager.GetLevelLabel(null);
			});
		}

		[Test]
		public void GetLevelLabel_ByName_GivenEmptyLevelName_ThrowArgumentNullException() {
			string name1 = "Level0";
			sceneManagerMock.sceneCountInBuildSettings.Returns(1);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			Assert.Throws<ArgumentNullException>(() => {
				levelManager.GetLevelLabel("");
			});
		}

		[Test]
		public void GetLevelLabel_ByName_GivenNonExistentLevelName_ThrowArgumentException() {
			string name1 = "Level0";
			sceneManagerMock.sceneCountInBuildSettings.Returns(1);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			Assert.Throws<ArgumentException>(() => {
				levelManager.GetLevelLabel("NonExistentLevelName");
			});
		}

		[Test]
		public void LoadLevel_GivenExistingLevelName_CallLoadSceneAndSetActiveLevelWithCorrectIndex() {
			string name1 = "Level0";
			string name2 = "Level1";
			string name3 = "Level2";
			sceneManagerMock.sceneCountInBuildSettings.Returns(3);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			sceneUtilityMock.GetScenePathByBuildIndex(1).Returns(name2);
			sceneUtilityMock.GetScenePathByBuildIndex(2).Returns(name3);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			levelManager.LoadLevel("Level2");

			sceneManagerMock.Received(1).LoadScene(Arg.Is<int>(2));
			Assert.AreEqual(2, levelManager.ActiveLevel);
		}

		[Test]
		public void LoadLevel_GivenNullLevelName_ThrowArgumentNullException() {
			string name1 = "Level0";
			sceneManagerMock.sceneCountInBuildSettings.Returns(1);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			Assert.Throws<ArgumentNullException>(() => {
				levelManager.LoadLevel(null);
			});
		}

		[Test]
		public void LoadLevel_GivenEmptyLevelName_ThrowArgumentNullException() {
			string name1 = "Level0";
			sceneManagerMock.sceneCountInBuildSettings.Returns(1);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			Assert.Throws<ArgumentNullException>(() => {
				levelManager.LoadLevel("");
			});
		}

		[Test]
		public void LoadLevel_GivenNonExistentLevelName_ThrowArgumentException() {
			string name1 = "Level0";
			sceneManagerMock.sceneCountInBuildSettings.Returns(1);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			Assert.Throws<ArgumentException>(() => {
				levelManager.LoadLevel("NonExistentLevelName");
			});
		}

		[Test]
		public void LoadNextLevel_NextLevelDoesExist_IncrementActiveLevelAndCallLoadSceneOnThatIndex() {
			string name1 = "Level0";
			string name2 = "Level1";
			string name3 = "Level2";
			sceneManagerMock.sceneCountInBuildSettings.Returns(3);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			sceneUtilityMock.GetScenePathByBuildIndex(1).Returns(name2);
			sceneUtilityMock.GetScenePathByBuildIndex(2).Returns(name3);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			levelManager.LoadNextLevel();

			sceneManagerMock.Received(1).LoadScene(Arg.Is<int>(1));
			Assert.AreEqual(1, levelManager.ActiveLevel);
		}

		[Test]
		public void LoadNextLevel_NextLevelDoesNotExist_SetActiveLevelToIndex0AndCallLoadSceneOnIndex0() {
			string name1 = "Level0";
			string name2 = "Level1";
			string name3 = "Level2";
			sceneManagerMock.sceneCountInBuildSettings.Returns(3);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(2);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			sceneUtilityMock.GetScenePathByBuildIndex(1).Returns(name2);
			sceneUtilityMock.GetScenePathByBuildIndex(2).Returns(name3);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			levelManager.LoadNextLevel();

			sceneManagerMock.Received(1).LoadScene(Arg.Is<int>(0));
			Assert.AreEqual(0, levelManager.ActiveLevel);
		}

		[Test]
		public void LoadPreviousLevel_PreviousLevelDoesExist_DecrementActiveLevelAndCallLoadSceneOnThatIndex() {
			string name1 = "Level0";
			string name2 = "Level1";
			string name3 = "Level2";
			sceneManagerMock.sceneCountInBuildSettings.Returns(3);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(2);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			sceneUtilityMock.GetScenePathByBuildIndex(1).Returns(name2);
			sceneUtilityMock.GetScenePathByBuildIndex(2).Returns(name3);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			levelManager.LoadPreviousLevel();

			sceneManagerMock.Received(1).LoadScene(Arg.Is<int>(1));
			Assert.AreEqual(1, levelManager.ActiveLevel);
		}

		[Test]
		public void LoadPreviousLevel_PreviousLevelDoesNotExist_SetActiveLevelToIndex0AndThrowInvalidOperationException() {
			string name1 = "Level0";
			string name2 = "Level1";
			string name3 = "Level2";
			sceneManagerMock.sceneCountInBuildSettings.Returns(3);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			sceneUtilityMock.GetScenePathByBuildIndex(1).Returns(name2);
			sceneUtilityMock.GetScenePathByBuildIndex(2).Returns(name3);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			Assert.Throws<InvalidOperationException>(() => {
				levelManager.LoadPreviousLevel();
			});
			Assert.AreEqual(0, levelManager.ActiveLevel);
		}

		[Test]
		public void LoadFirstLevel_FirstLevelWasNeverSet_SetActiveLevelToIndex0AndCallLoadSceneOnThatIndex() {
			string name1 = "Level0";
			string name2 = "Level1";
			string name3 = "Level2";
			sceneManagerMock.sceneCountInBuildSettings.Returns(3);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(2);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			sceneUtilityMock.GetScenePathByBuildIndex(1).Returns(name2);
			sceneUtilityMock.GetScenePathByBuildIndex(2).Returns(name3);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			levelManager.LoadFirstLevel();

			sceneManagerMock.Received(1).LoadScene(Arg.Is<int>(0));
			Assert.AreEqual(0, levelManager.ActiveLevel);
		}

		[Test]
		public void LoadFirstLevel_FirstLevelWasSet_SetActiveLevelToFirstLevelAndCallLoadSceneOnThatIndex() {
			string name1 = "Level0";
			string name2 = "Level1";
			string name3 = "Level2";
			sceneManagerMock.sceneCountInBuildSettings.Returns(3);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(2);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			sceneUtilityMock.GetScenePathByBuildIndex(1).Returns(name2);
			sceneUtilityMock.GetScenePathByBuildIndex(2).Returns(name3);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			levelManager.FirstLevel = "Level1";
			levelManager.LoadFirstLevel();

			sceneManagerMock.Received(1).LoadScene(Arg.Is<int>(1));
			Assert.AreEqual(1, levelManager.ActiveLevel);
		}

		[Test]
		public void FirstLevel_GivenNullLevelName_ThrowArgumentNullException() {
			string name1 = "Level0";
			sceneManagerMock.sceneCountInBuildSettings.Returns(1);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			Assert.Throws<ArgumentNullException>(() => {
				levelManager.FirstLevel = null;
			});
		}

		[Test]
		public void FirstLevel_GivenEmptyLevelName_ThrowArgumentNullException() {
			string name1 = "Level0";
			sceneManagerMock.sceneCountInBuildSettings.Returns(1);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			Assert.Throws<ArgumentNullException>(() => {
				levelManager.FirstLevel = "";
			});
		}

		[Test]
		public void FirstLevel_GivenNonExistentLevelName_ThrowArgumentException() {
			string name1 = "Level0";
			sceneManagerMock.sceneCountInBuildSettings.Returns(1);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(0);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			Assert.Throws<ArgumentException>(() => {
				levelManager.FirstLevel = "NonExistentLevelName";
			});
		}

		[Test]
		public void ReloadLevel_CallLoadSceneOnTheActiveLevel() {
			string name1 = "Level0";
			string name2 = "Level1";
			sceneManagerMock.sceneCountInBuildSettings.Returns(2);
			sceneManagerMock.GetActiveSceneBuildIndex().Returns(1);
			sceneUtilityMock.GetScenePathByBuildIndex(0).Returns(name1);
			sceneUtilityMock.GetScenePathByBuildIndex(1).Returns(name2);
			LevelManager levelManager = Container.Resolve<ILevelManager>() as LevelManager;
			levelManager.Initialize();

			levelManager.ReloadLevel();

			sceneManagerMock.Received(1).LoadScene(Arg.Is<int>(1));
		}
	}
}
