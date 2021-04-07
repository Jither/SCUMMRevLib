using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Annotations
{
    public sealed class Annotator
    {
        public static Annotator Current { get { return instance; } }
        private static readonly Annotator instance = new Annotator();

        private readonly Dictionary<SRFile, AnnotationMap> annotations;

        public string AnnotationsPath
        {
            get; private set;
        }

        private Annotator()
        {
            //AnnotationsPath = Path.Combine(RunStatus.ExeDirectory, "Annotations");
            ScanAnnotationFiles();
            annotations = new Dictionary<SRFile, AnnotationMap>();
        }

        private void ScanAnnotationFiles()
        {
            
        }

        public string GetAnnotation(Chunk chunk)
        {
            AnnotationMap map = annotations[chunk.File];
            return map.Get(chunk.ChunkTypeId, chunk.Offset);
        }

        public void SetAnnotation(Chunk chunk, string annotation)
        {
            AnnotationMap map = annotations[chunk.File];
            map.Set(chunk.ChunkTypeId, chunk.Offset, annotation);
        }

        public void LoadAnnotationsForFile(SRFile file)
        {
            AnnotationMap map = new AnnotationMap();
            string checksum = file.GetRecognitionChecksum();
            annotations.Add(file, map);
        }

        static Annotator()
        {
            
        }
    }
}
