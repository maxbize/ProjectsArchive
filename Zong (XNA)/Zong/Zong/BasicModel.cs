using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zong
{
    class BasicModel
    {
        public Model model { get; protected set; }
        protected Matrix world = Matrix.Identity;

        protected float alpha;
        bool enableDefaultLighting;
        Vector3 color;
        Vector3 size;

        public BasicModel(Model m, Vector3 colour, float alphaTransparency = 1, bool enableDefaultLightng = true)
        {
            color = colour;
            model = m;
            alpha = alphaTransparency;
            enableDefaultLighting = enableDefaultLightng;
            BoundingBox box = Functions.getBoundingBox(m, world);
            size = new Vector3(Math.Abs(box.Max.X - box.Min.X) / 2,
                Math.Abs(box.Max.Z - box.Min.Z) / 2, Math.Abs(box.Max.Y - box.Min.Y) / 2);
        }

        public virtual void Update(GameTime gameTime)
        {
            // Not implemented in parent class
        }

        public void Draw(Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.DiffuseColor = color;
                    be.Alpha = alpha;
                    if (enableDefaultLighting)
                    {
                        be.EnableDefaultLighting();
                    }
                    be.Projection = camera.projection;
                    be.View = camera.view;
                    be.World = GetWorld() * mesh.ParentBone.Transform;
                }

                mesh.Draw();
            }
        }

        public virtual Matrix GetWorld()
        {
            return world;
        }

        public virtual Vector3 GetSize()
        {
            return size;
        }
    }
}
