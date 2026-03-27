// Main
using System;

namespace obj_voxelizer
{
    class Program
    {
        static void Main()
        {
            try
            {
                Console.WriteLine("Masukkan path menuju file OBJ (full path):");
                string? ObjPath = Console.ReadLine();
                ObjPath = ObjPath?.Trim();

                if (string.IsNullOrEmpty(ObjPath))
                    throw new Exception("File path kosong.");

                Mesh mesh = ObjectIO.Import(ObjPath);
                if (mesh.vertices.Count == 0 || mesh.faces.Count == 0)
                    throw new Exception("Import gagal. File kosong atau invalid.");

                Console.WriteLine($"Berhasil memuat data.");
                Console.WriteLine($"Mesh: {mesh.vertices.Count} vertices, {mesh.faces.Count} faces.");

                Console.WriteLine("Masukkan kedalaman maksimum (default 8):"); // jujur udah keburu integrate ini, maaf kalo harusnya gaada wkwkwk

                string? inputDepth = Console.ReadLine();
                int maxDepth = Voxelizer.DEFAULT_DEPTH; // redundant si tapi yaudah
                if (!string.IsNullOrEmpty(inputDepth) && int.TryParse(inputDepth, out int parsedDepth) && parsedDepth >= 0)
                    maxDepth = parsedDepth;
                
                maxDepth = Math.Min(maxDepth, Voxelizer.MAX_DEPTH); // mmf biar ga kelamaan
                Console.WriteLine($"kedalaman maksimal: {maxDepth}");
                
                Console.WriteLine("Memulai voxelization...");
                Voxelizer voxelizer = new Voxelizer(mesh, maxDepth);
                DateTime startTime = DateTime.Now;
                voxelizer.Voxelize();
                DateTime endTime = DateTime.Now;
                TimeSpan duration = endTime - startTime;

                Console.WriteLine($"Program selesai dalam {duration.TotalMilliseconds} ms.");

                voxelizer.GetStats();
                
                Console.WriteLine($"Masukkan output path untuk voxelized OBJ file (atau Enter untuk default path):");
                string? outputPath = Console.ReadLine();
                outputPath = outputPath?.Trim();
                if (string.IsNullOrEmpty(outputPath))
                    outputPath = Path.ChangeExtension(ObjPath, "_voxels.obj");

                ObjectIO.Export(Voxelizer.GetLeaf(), outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}