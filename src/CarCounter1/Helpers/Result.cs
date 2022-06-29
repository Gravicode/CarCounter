using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCounter1.Helpers
{
    public class Result
    {
        /// <summary>
        /// x1, y1, x2, y2 in page coordinates.
        /// </summary>
        public float[] BoundingBox { get; }

        /// <summary>
        /// The Bounding box category.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Confidence level.
        /// </summary>
        public float Confidence { get; }

        public Result(float[] boundingBox, string label, float confidence)
        {
            BoundingBox = boundingBox;
            Label = label;
            Confidence = confidence;
        }
    }

}
