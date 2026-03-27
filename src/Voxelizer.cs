using System;
using System.Reflection.Metadata;

namespace obj_voxelizer
{
    public class Voxelizer
    {
        public Mesh mesh;
        public Octree octree;
        public int maxDepth;
        private List<Octree> potentialNodes = new List<Octree>();
        public const int MAX_INT = 1000000007; 
        public const int DEFAULT_DEPTH = 8;
        public const int MAX_DEPTH = 20;
        private static int[] nodeExCount = new int[MAX_INT]; 
        private static int[] nodeUnCount = new int[MAX_INT];
        private static int[] leafCount = new int[MAX_INT];
        private static List<Octree> leafNodes = new List<Octree>();

        public Voxelizer(Mesh mesh, int maxDepth = DEFAULT_DEPTH)
        {
            this.mesh = mesh;
            this.octree = new Octree(mesh.getBounds());
            this.maxDepth = maxDepth;
        }

        public Voxelizer(Mesh mesh, Octree octree, int maxDepth = DEFAULT_DEPTH)
        {
            this.mesh = mesh;
            this.octree = octree;
            this.maxDepth = maxDepth;
        }

        public void Voxelize()
        {
            int depth = this.octree.depth;
            // GetChildren()?
            switch (depth)
            {
                case int d when d == maxDepth: // mentok
                    nodeExCount[depth]++;
                    leafCount[depth]++;
                    leafNodes.Add(this.octree);
                    break;
                case int d when d > maxDepth || d < 0: // invalid
                    break;
                default:
                    nodeExCount[depth]++; // explored
                    octree.GetChildren();
                    foreach (var child in octree.child)
                    {
                        // Check if child intersect or contains mesh
                        if (this.Validate(child))
                        {
                            Voxelizer childVoxelizer = new Voxelizer(mesh, child, maxDepth);
                            childVoxelizer.Voxelize(); // Asumsi pasti ada minimal 1 child yang valid 
                            // potentialNodes.Add(child);
                        }
                        else 
                            nodeUnCount[child.depth]++; // unexplored
                    }
                    break;
            }
            // Check potential nodes, turn this into leaf or check children
            // switch (potentialNodes.Count)
            // {
            //     case int cnt when cnt == 8: // why bother?
            //         this.octree.isLeaf = true;
            //         leafNodes.Add(this.octree);
            //         break;
            //     default: // lemme check this for a second
            //         foreach (var node in potentialNodes)
            //         {
            //             Voxelizer childVoxelizer = new Voxelizer(mesh, node, maxDepth);
            //             childVoxelizer.Voxelize();
            //         }
            //         break;
            // }
                
            
        }

        public bool Validate(Octree node)
        {
            foreach (var faces in mesh.faces)
            {
                AABB triangle = new AABB(mesh.vertices[faces.Item1], 
                                         mesh.vertices[faces.Item2], 
                                         mesh.vertices[faces.Item3]);
                if (
                    Collision.IntersectAABB(triangle, node.bounds) || 
                    Collision.ContainsAABB(node.bounds, triangle))
                    return true; // AABB intersect or contain triangle
            }
            return false;
        }

        public void GetStats()
        {
            Console.WriteLine("--Stats");

            Console.WriteLine($"Banyaknya voxel yang terbentuk: {leafNodes.Count}");
            Console.WriteLine($"Banyaknya vertex yang terbentuk: {leafNodes.Count * 8}");
            Console.WriteLine($"Banyaknya faces yang terbentuk: {leafNodes.Count * 12}");

            Console.WriteLine("Banyaknya node yang terbentuk per depth:");
            for (int i = 0; i <= maxDepth; i++)
                Console.WriteLine($"{i}: {nodeExCount[i]}");

            Console.WriteLine("Banyaknya node yang tidak dieksplorasi per depth:");
            for (int i = 0; i <= maxDepth; i++)
                Console.WriteLine($"{i}: {nodeUnCount[i]}");
        }

        public static List<Octree> GetLeaf()
        {
            return leafNodes;
        }
    }
}