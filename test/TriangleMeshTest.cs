﻿using BulletSharp;
using NUnit.Framework;

namespace BulletSharpTest
{
    [TestFixture]
    class TriangleMeshTest
    {
        DefaultCollisionConfiguration conf;
        CollisionDispatcher dispatcher;
        AxisSweep3 broadphase;
        DiscreteDynamicsWorld world;

        TriangleIndexVertexArray indexVertexArray;
        GImpactMeshShape gImpactMeshShape;
        BvhTriangleMeshShape triangleMeshShape;
        RigidBody gImpactMesh, triangleMesh;

        [Test]
        public void TriangleMeshTest1()
        {
            foreach (var indexedMesh in indexVertexArray.IndexedMeshArray)
            {
                Assert.NotNull(indexedMesh);
            }
            var initialMesh = indexVertexArray.IndexedMeshArray[0];
            Assert.AreEqual(initialMesh.IndexType, PhyScalarType.Int32);
            Assert.AreEqual(initialMesh.VertexType, PhyScalarType.Single);
            Assert.AreEqual(initialMesh.NumVertices, TorusMesh.Vertices.Length / 3);
            Assert.AreEqual(initialMesh.NumTriangles, TorusMesh.Indices.Length / 3);
            Assert.AreEqual(initialMesh.VertexStride, sizeof(float) * 3);
            Assert.AreEqual(initialMesh.TriangleIndexStride, sizeof(int) * 3);

            var triangleIndices = initialMesh.TriangleIndices;
            Assert.AreEqual(triangleIndices.Count, TorusMesh.Indices.Length);
            for (int i = 0; i < triangleIndices.Count; i++)
            {
                Assert.AreEqual(triangleIndices[i], TorusMesh.Indices[i]);
            }

            Vector3 aabbMin, aabbMax;
            gImpactMeshShape.GetAabb(Matrix.Identity, out aabbMin, out aabbMax);
            triangleMeshShape.GetAabb(Matrix.Identity, out aabbMin, out aabbMax);

            for (int i = 0; i < 600; i++)
            {
                world.StepSimulation(1.0f / 60.0f);
            }
        }

        [TestFixtureSetUp]
        public void SetUp()
        {
            conf = new DefaultCollisionConfiguration();
            dispatcher = new CollisionDispatcher(conf);
            broadphase = new AxisSweep3(new Vector3(-1000, -1000, -1000), new Vector3(1000, 1000, 1000));
            world = new DiscreteDynamicsWorld(dispatcher, broadphase, null, conf);

            indexVertexArray = new TriangleIndexVertexArray(TorusMesh.Indices, TorusMesh.Vertices);

            gImpactMeshShape = new GImpactMeshShape(indexVertexArray);
            gImpactMeshShape.CalculateLocalInertia(1.0f);

            triangleMeshShape = new BvhTriangleMeshShape(indexVertexArray, true);
            // CalculateLocalInertia must fail for static shapes (shapes based on TriangleMeshShape)
            //triangleMeshShape.CalculateLocalInertia(1.0f);

            gImpactMesh = CreateBody(1.0f, gImpactMeshShape, Vector3.Zero);
            triangleMesh = CreateBody(0.0f, triangleMeshShape, Vector3.Zero);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            world.RemoveRigidBody(gImpactMesh);
            world.RemoveRigidBody(gImpactMesh);
            gImpactMesh.MotionState.Dispose();
            triangleMesh.MotionState.Dispose();
            gImpactMesh.Dispose();
            triangleMesh.Dispose();
            gImpactMeshShape.Dispose();
            triangleMeshShape.Dispose();
            indexVertexArray.Dispose();

            world.Dispose();
            dispatcher.Dispose();
            broadphase.Dispose();
            conf.Dispose();
        }

        RigidBody CreateBody(float mass, CollisionShape shape, Vector3 offset)
        {
            using (var info = new RigidBodyConstructionInfo(mass, new DefaultMotionState(), shape, Vector3.Zero))
            {
                if (mass != 0.0f)
                {
                    info.LocalInertia = info.CollisionShape.CalculateLocalInertia(mass);
                }
                var collisionObject = new RigidBody(info);
                collisionObject.Translate(offset);
                world.AddRigidBody(collisionObject);
                return collisionObject;
            }
        }
    }
}
