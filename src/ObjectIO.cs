using System;
using System.Collections.Generic;
using System.IO;

namespace obj_voxelizer
{
    public class ObjectIO
    {
        public static List<Vectorz> vertices = new List<Vectorz>();
        public static List<(int, int, int)> faces = new List<(int, int, int)>();
        public static Mesh Import(string filePath)
        {
            Console.WriteLine($"Importing OBJ file dari: {filePath}");

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                switch (line)
                {
                    case var l when l.StartsWith("v "):
                        // Parse vertex line
                        var parts = l.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 4)
                        {
                            float x = float.Parse(parts[1]);
                            float y = float.Parse(parts[2]);
                            float z = float.Parse(parts[3]);
                            vertices.Add(new Vectorz(x, y, z));
                        }
                        break;
                    case var l when l.StartsWith("f "):
                        // Parse face line
                        parts = l.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 4)
                        {
                            int v1 = int.Parse(parts[1].Split(' ')[0]) - 1;
                            int v2 = int.Parse(parts[2].Split(' ')[0]) - 1;
                            int v3 = int.Parse(parts[3].Split(' ')[0]) - 1;
                            faces.Add((v1, v2, v3));
                        }
                        break;
                    default:
                        // Handle other line types
                        // Skip empty or invalid lines
                        break;
                }
            }
            Mesh ret = new Mesh(vertices, faces);
            return ret;
        }

        public static void Export(List<Octree> leafNodes, string outputPath)
        {
            Console.WriteLine($"Export data ke: {outputPath}");
            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                int cnt = 0;
                foreach (var node in leafNodes)
                {
                    // redundant voxel algs, tapi gatau mau gimana lagi hehe
                    // dia selalu buat node dengan ukuran maxDepth, jadi lama
                    // harusnya bisa dioptimalisasi pake skema 'merge' dari merge-sort, tapi gitu deh

                    writer.WriteLine($"v {node.bounds.min.X} {node.bounds.min.Y} {node.bounds.min.Z}"); // 0 0 0
                    writer.WriteLine($"v {node.bounds.min.X} {node.bounds.min.Y} {node.bounds.max.Z}"); // 0 0 1
                    writer.WriteLine($"v {node.bounds.min.X} {node.bounds.max.Y} {node.bounds.min.Z}"); // 0 1 0
                    writer.WriteLine($"v {node.bounds.min.X} {node.bounds.max.Y} {node.bounds.max.Z}"); // 0 1 1
                    writer.WriteLine($"v {node.bounds.max.X} {node.bounds.min.Y} {node.bounds.min.Z}"); // 1 0 0
                    writer.WriteLine($"v {node.bounds.max.X} {node.bounds.min.Y} {node.bounds.max.Z}"); // 1 0 1
                    writer.WriteLine($"v {node.bounds.max.X} {node.bounds.max.Y} {node.bounds.min.Z}"); // 1 1 0
                    writer.WriteLine($"v {node.bounds.max.X} {node.bounds.max.Y} {node.bounds.max.Z}"); // 1 1 1

                    writer.WriteLine($"f {cnt+1} {cnt+2} {cnt+3}");
                    writer.WriteLine($"f {cnt+2} {cnt+3} {cnt+4}");
                    writer.WriteLine($"f {cnt+5} {cnt+6} {cnt+7}");
                    writer.WriteLine($"f {cnt+6} {cnt+7} {cnt+8}");

                    writer.WriteLine($"f {cnt+1} {cnt+2} {cnt+5}");
                    writer.WriteLine($"f {cnt+2} {cnt+5} {cnt+6}");
                    writer.WriteLine($"f {cnt+3} {cnt+4} {cnt+7}");
                    writer.WriteLine($"f {cnt+4} {cnt+7} {cnt+8}");

                    writer.WriteLine($"f {cnt+1} {cnt+3} {cnt+5}");
                    writer.WriteLine($"f {cnt+3} {cnt+5} {cnt+7}");
                    writer.WriteLine($"f {cnt+2} {cnt+4} {cnt+6}");
                    writer.WriteLine($"f {cnt+4} {cnt+6} {cnt+8}");
                    
                    cnt += 8;
                }
            }
        }
    }
}