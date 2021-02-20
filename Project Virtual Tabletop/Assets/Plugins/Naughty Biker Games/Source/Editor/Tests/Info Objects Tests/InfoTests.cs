using System;
using System.Collections.Generic;
using NaughtyBiker.InfoObjects;
using NUnit.Framework;

namespace NaughtyBiker.Editor.Tests.InfoObjects {
	public class InfoTests {
		[Test]
		public void Get_ValidAndExistingKey_ReturnsCorrectStringValue() {
			IDictionary<string, object> fakeData = new Dictionary<string, object>();
            fakeData.Add("foo", "bar");
            Info info = new Info("FAKE", fakeData);

            string actual = (string)info["foo"];

            Assert.AreEqual("bar", actual);
		}

        [Test]
		public void Get_ValidAndExistingKey_ReturnsCorrectIntegerValue() {
			IDictionary<string, object> fakeData = new Dictionary<string, object>();
            fakeData.Add("foo", 5);
            Info info = new Info("FAKE", fakeData);

            int actual = (int)info["foo"];

            Assert.AreEqual(5, actual);
		}

        [Test]
		public void Get_NonExistingKey_ThrowsKeyNotFoundException() {
            Info info = new Info("FAKE");

            Assert.Throws<KeyNotFoundException>(() => info.Get("foo"));
		}

        [Test]
		public void Get_NullKey_ThrowsArgumentNullException() {
            Info info = new Info("FAKE");

            Assert.Throws<ArgumentNullException>(() => info.Get(null));
		}

        [Test]
		public void Get_EmptyKey_ThrowsArgumentException() {
            Info info = new Info("FAKE");

            Assert.Throws<ArgumentException>(() => info.Get(""));
		}

        [Test]
        public void Set_ValidAndNonExistingKey_AddsNewKeyAndValue() {
            Info info = new Info("FAKE");

            info["foo"] = "bar";

            Assert.True(info.Data.ContainsKey("foo"));
            Assert.AreEqual("bar", info.Data["foo"]);
        }

        [Test]
        public void Set_ValidAndExistingKey_UpdatesValue() {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data.Add("foo", "bar");
            Info info = new Info("FAKE", data);

            info["foo"] = "baz";

            Assert.True(data.ContainsKey("foo"));
            Assert.AreEqual("baz", data["foo"]);
        }

        [Test]
        public void Set_IdAndValidKeyAndValue_CallsStateChangedWithCorrectInfo() {
            string actualId = "";
            string actualKey = "";
            string actualValue = "";
            Info info = new Info("foo", (id, key, value) => {
                actualId = id;
                actualKey = key;
                actualValue = (string)value;
            });

            info["bar"] = "baz";

            Assert.AreEqual("foo", actualId);
            Assert.AreEqual("bar", actualKey);
            Assert.AreEqual("baz", actualValue);
        }

        [Test]
        public void Set_NullKey_ThrowsArgumentNullException() {
            Info info = new Info("FAKE");

            Assert.Throws<ArgumentNullException>(() => info.Set(null, ""));
        }

        [Test]
        public void Set_EmptyKey_ThrowsArgumentException() {
            Info info = new Info("FAKE");

            Assert.Throws<ArgumentException>(() => info.Set("", ""));
        }

        [Test]
        public void Exists_ValidAndNonExistingKey_ReturnsFalse() {
            Info info = new Info("FAKE");

            bool actual = info.Exists("foo");

            Assert.False(actual);
        }

        [Test]
        public void Exists_ValidAndExistingKey_ReturnsTrue() {
            IDictionary<string, object> fakeData = new Dictionary<string, object>();
            fakeData.Add("foo", "bar");
            Info info = new Info("FAKE", fakeData);

            bool actual = info.Exists("foo");

            Assert.True(actual);
        }

        [Test]
        public void Exists_NullKey_ThrowsArgumentNullException() {
            Info info = new Info("FAKE");

            Assert.Throws<ArgumentNullException>(() => info.Exists(null));
        }

        [Test]
        public void Exists_EmptyKey_ThrowsArgumentException() {
            Info info = new Info("FAKE");

            Assert.Throws<ArgumentException>(() => info.Exists(""));
        }
	}
}
