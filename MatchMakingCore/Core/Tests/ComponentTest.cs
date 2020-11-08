using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Diagnostics;
using System;
using UnityEngine;
using UnityEngine.TestTools;
using MatchMakingCore;
using Debug = UnityEngine.Debug;

namespace MatchMaking.Tests
{
    public class ComponentTest
    {
        [Test]
        public void AddComponentTest()
        {
            Container container = new Container();
            int entityCount = 100;

            for(int i = 0; i < entityCount; ++i)
            {
                int entityId = container.CreateEntity();
                Assert.AreEqual(i, entityId);
                container.AddPlayerInfoComponent(entityId);
            }
        }

        [Test]
        public void AccessComponentTest()
        {
            Container container = new Container();
            int entityCount = 100;

            for (int i = 0; i < entityCount; ++i)
            {
                int entityId = container.CreateEntity();
                container.AddPlayerInfoComponent(entityId);
            }

            for(int i = 0; i < entityCount; ++i)
            {
                Assert.IsTrue(container.TryGetPlayerInfoDatabaseKeyFromEntityId(i, out int _));
            }

            int[] falseIds = new int[] { -1, 100, 300};
            foreach(int falseId in falseIds)
            {
                Assert.IsFalse(container.TryGetPlayerInfoDatabaseKeyFromEntityId(falseId, out int _));
            }
        }

        [Test]
        public void ChangeComponentTest()
        {
            Container container = new Container();
            int entityCount = 100;

            for (int i = 0; i < entityCount; ++i)
            {
                int entityId = container.CreateEntity();
                container.AddPlayerInfoComponent(entityId);
            }

            for(int i = 0; i < entityCount; ++i)
            {
                Assert.IsTrue(container.TrySetPlayerInfoDatabaseKeyFromEntityId(i, i + entityCount));
            }

            for (int i = 0; i < entityCount; ++i)
            {
                int databaseKey;
                Assert.IsTrue(container.TryGetPlayerInfoDatabaseKeyFromEntityId(i, out databaseKey));
                Assert.AreEqual(i + entityCount, databaseKey);
            }
        }

        [Test]
        public void RemoveComponentTest()
        {
            Container container = new Container();
            int entityCount = 100;

            for (int i = 0; i < entityCount; ++i)
            {
                int entityId = container.CreateEntity();
                container.AddPlayerInfoComponent(entityId);
            }

            for (int i = 0; i < entityCount; ++i)
            {
                Assert.IsTrue(container.TrySetPlayerInfoDatabaseKeyFromEntityId(i, i + entityCount));
            }

            int[] removeIndices = new int[] { 0, 30, 22, 99};

            foreach(int index in removeIndices)
            {
                container.RemovePlayerInfoComponent(index);
                Assert.IsFalse(container.HasPlayerInfoComponent(index));
            }

            foreach(int index in removeIndices)
            {
                Assert.IsFalse(container.HasPlayerInfoComponent(index));
            }

            int newId = container.CreateEntity();
            container.AddPlayerInfoComponent(newId);
            int newDatabaseId;
            Assert.IsTrue(container.TryGetPlayerInfoDatabaseKeyFromEntityId(newId, out newDatabaseId));

            Assert.AreEqual(Container.EMPTY_ID, newDatabaseId);
        }
    }
}