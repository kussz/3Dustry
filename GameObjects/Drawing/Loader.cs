using SharpDX;
//using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D11;
using SharpDX.WIC;

namespace GameObjects.Drawing
{
    public class Loader : IDisposable
    {
        private DirectX3DGraphics _directX3DGraphics;

        public Loader(DirectX3DGraphics directX3DGraphics)
        {
            _directX3DGraphics = directX3DGraphics;
        }
        public MeshObject MakeMenuTile(float aspect,float size,Vector2 position)
        {
            float aspectX;
            float aspectY;
            //size /= 2;
            if(aspect>=1)
            {
                aspectX = size;
                aspectY = size / aspect;
            }
            else
            {
                aspectX = aspect*size;
                aspectY = size;
            }
            Renderer.VertexDataStruct[] vertices =
            {
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(-aspectX,-aspectY,1,1.0f),
                    texCoord= new Vector2(0,1),
                },
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(+aspectX,-aspectY,1,1.0f),
                    texCoord= new Vector2(1,1),
                },
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(-aspectX,+aspectY,1,1.0f),
                    texCoord= new Vector2(0,0),
                },
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(+aspectX,+aspectY,1,1.0f),
                    texCoord= new Vector2(1,0),
                },
            };
            uint[] indices = new uint[]
            {
                0, 1, 2,    1,3,2,
            };



            return new MeshObject(_directX3DGraphics,new Vector4(position.X,position.Y,0,1),
                0, 0, 0, vertices, indices);
        }
        public MeshObject MakeTileSquare(Vector4 position)
        {
            Renderer.VertexDataStruct[] vertices = new Renderer.VertexDataStruct[4]
            {
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(position.X,position.Y,position.Z,1.0f),
                    texCoord= new Vector2(0,0),
                        normal = new Vector3(0,1,0)

                },
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(position.X+1,position.Y,position.Z,1.0f),
                    texCoord= new Vector2(1,0),
                        normal = new Vector3(0,1,0)

                },
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(position.X,position.Y,position.Z+1,1.0f),
                    texCoord= new Vector2(0,1),
                        normal = new Vector3(0,1,0)

                },
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(position.X+1,position.Y,position.Z+1,1.0f),
                    texCoord= new Vector2(1,1),
                        normal = new Vector3(0,1,0)

                },
            };
            uint[] indices = new uint[]
            {
                0, 1, 2,    1,3,2,
            };



            return new MeshObject(_directX3DGraphics, position,
                0, 0, 0, vertices, indices);
        }
        public MeshObject MakeCube(Vector4 position,Vector2 size, float yaw, float pitch, float roll)
        {
            float deltaWidth = size[0];
            float height = size[1];
            Renderer.VertexDataStruct[] vertices =
               new Renderer.VertexDataStruct[]
               {

                    new Renderer.VertexDataStruct // top 0
                    {
                        position = new Vector4(-deltaWidth/2, height, deltaWidth/2, 1.0f),
                        texCoord = new Vector2(3/4f,3/4f),
                        normal = new Vector3(0,1,0)
                    },
                    new Renderer.VertexDataStruct // top 1
                    {
                        position = new Vector4(-deltaWidth/2, height, -deltaWidth/2, 1.0f),
                        texCoord = new Vector2(3/4f, 1/4f),
                        normal = new Vector3(0,1,0)
                    },
                    new Renderer.VertexDataStruct // top 2
                    {
                        position = new Vector4(deltaWidth/2, height, -deltaWidth/2, 1.0f),
                        texCoord = new Vector2(1/4f, 1/4f),
                        normal = new Vector3(0,1,0)

                    },
                    new Renderer.VertexDataStruct // top 3
                    {
                        position = new Vector4(deltaWidth/2, height, deltaWidth/2, 1.0f),
                        texCoord = new Vector2(1/4f,3/4f),
                        normal = new Vector3(0,1,0)

                    },

                    new Renderer.VertexDataStruct // bottom 4
                    {
                        position = new Vector4(-deltaWidth/2, 0, -deltaWidth/2, 1.0f),
                        texCoord = new Vector2(1f, 1/4f),
                        normal = new Vector3(-1,0,0)
                    },
                    new Renderer.VertexDataStruct // bottom 45 (5)
                    {
                        position = new Vector4(-deltaWidth/2, 0, -deltaWidth/2, 1.0f),
                        texCoord = new Vector2(3/4f, 0.0f),
                        normal = new Vector3(0,0,-1)
                    },
                    new Renderer.VertexDataStruct // bottom 5 (6)
                    {
                        position = new Vector4(-deltaWidth/2, 0,deltaWidth/2, 1.0f),
                        texCoord = new Vector2(1.0f, 3/4f),
                        normal = new Vector3(-1,0,0)
                    },
                    new Renderer.VertexDataStruct // bottom 56 (7)
                    {
                        position = new Vector4(-deltaWidth/2, 0,deltaWidth/2, 1.0f),
                        texCoord = new Vector2(3 / 4f, 1f),
                        normal = new Vector3(0,0,1)
                    },
                    new Renderer.VertexDataStruct // bottom 6 (8)
                    {
                        position = new Vector4(deltaWidth/2, 0, deltaWidth/2, 1.0f),
                        texCoord = new Vector2(1 / 4f, 1.0f),
                        normal = new Vector3(0,0,1)
                    },
                    new Renderer.VertexDataStruct // bottom 67 (9)
                    {
                        position = new Vector4(deltaWidth/2, 0, deltaWidth/2, 1.0f),
                        texCoord = new Vector2(0f, 3 / 4f),
                        normal = new Vector3(1,0,0)
                    },
                    new Renderer.VertexDataStruct // bottom 7 (10)
                    {
                        position = new Vector4(deltaWidth/2, 0, -deltaWidth/2, 1.0f),
                        texCoord = new Vector2(0f, 1/ 4f),
                        normal = new Vector3(1,0,0)
                    },
                    new Renderer.VertexDataStruct // bottom 78 (11)
                    {
                        position = new Vector4(deltaWidth/2, 0, -deltaWidth/2, 1.0f),
                        texCoord = new Vector2(1 / 4f, 0f),
                        normal = new Vector3(0,0,-1)
                    }
               };
            uint[] indices = new uint[]
            {
                0, 1, 2,    0,2,3,
                //4, 6,8,      4,8,10,

                3,10,9,
                2,10,3,

                3,8,7,
                0,3,7,

                1,0,6,
                1,6,4,

                11,2,1,
                11,1,5
            };
            // Создание вершинного буфера с данными
            return new MeshObject(_directX3DGraphics, position,
                yaw, pitch, roll, vertices, indices);
        }
        
        public void Dispose()
        {

        }
    }
}
