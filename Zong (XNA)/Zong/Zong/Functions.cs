using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zong
{
    class Functions
    {
        static Random random = new Random();

        public static Color randomColor()
        {
            Color color = new Color(
                random.Next(0, 255),
                random.Next(0, 255),
                random.Next(0, 255));
            return color;
        }

        public static int nextRandom()
        {
            return random.Next(101);
        }

        public static int nextRandom(int min, int max)
        {
            return random.Next(min, max);
        }

        public static bool cellInList(List<Int3> cellList, Int3 cell)
        {
            foreach (Int3 listCell in cellList)
            {
                if (listCell.X == cell.X &&
                    listCell.Y == cell.Y &&
                    listCell.Z == cell.Z)
                {
                    return true;
                }
            }
            return false;
        }

        public static BoundingBox getBoundingBox(Model model, Matrix worldTransform)
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), worldTransform);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            // Create and return bounding box
            return new BoundingBox(min, max);
        }
    }
}
