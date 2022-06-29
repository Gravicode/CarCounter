using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CarCounter1.Helpers
{
    public class ImagePrediction
    {
       

        // Read more on YOLO configuration here:
        // https://github.com/hunglc007/tensorflow-yolov4-tflite/blob/9f16748aa3f45ff240608da4bd9b1216a29127f5/core/config.py#L18
        // https://github.com/hunglc007/tensorflow-yolov4-tflite/blob/9f16748aa3f45ff240608da4bd9b1216a29127f5/core/config.py#L20
        /*
        //car
        private readonly float[] STRIDES = new float[] { 8, 16, 32 };
        private readonly float[] XYSCALE = new float[] { 1.2f, 1.1f, 1.05f };
        private readonly int[] SHAPES = new int[] { 56, 28, 14 };


        private const int _anchorsCount = 3;
        private const float _scoreThreshold = 0.25f;
        private const float _iouThreshold = 0.45f;
        */
        private readonly float[][][] ANCHORS = new float[][][]
        {
            new float[][] { new float[] { 12, 16 }, new float[] { 19, 36 }, new float[] { 40, 28 } },
            new float[][] { new float[] { 36, 75 }, new float[] { 76, 55 }, new float[] { 72, 146 } },
            new float[][] { new float[] { 142, 110 }, new float[] { 192, 243 }, new float[] { 459, 401 } }
        };

        // Read more on YOLO configuration here:
        // https://github.com/hunglc007/tensorflow-yolov4-tflite/blob/9f16748aa3f45ff240608da4bd9b1216a29127f5/core/config.py#L18
        // https://github.com/hunglc007/tensorflow-yolov4-tflite/blob/9f16748aa3f45ff240608da4bd9b1216a29127f5/core/config.py#L20

        private readonly float[] STRIDES = new float[] { 8, 16, 32 };
        private readonly float[] XYSCALE = new float[] { 1.2f, 1.1f, 1.05f };
        private readonly int[] SHAPES = new int[] { 52, 26, 13 };


        private const int _anchorsCount = 3;
        private const float _scoreThreshold = 0.5f;
        private const float _iouThreshold = 0.5f;

       
        [VectorType(1, 52, 52, 3, 85)]
        [ColumnName("Identity:0")]
        public float[] Identity { get; set; }

        /// <summary>
        /// Output - Identity 1:0
        /// </summary>
        [VectorType(1, 26, 26, 3, 85)]
        [ColumnName("Identity_1:0")]
        public float[] Identity1 { get; set; }

        /// <summary>
        /// Output - Identity 2:0
        /// </summary>
        [VectorType(1, 13, 13, 3, 85)]
        [ColumnName("Identity_2:0")]
        public float[] Identity2 { get; set; }
        
         

        /*
        /// <summary>
        /// Output - Identity
        /// </summary>
        //[VectorType(1, 17640, 10)]
        //[ColumnName("modelOutput")]
        //public float[] Identity { get; set; }

        /// <summary>
        /// Output - Identity 1:0
        /// </summary>
        [VectorType(1, 3, 56, 80, 10)]
        [ColumnName("onnx::Sigmoid_527")]
        public float[] Identity1 { get; set; }

        /// <summary>
        /// Output - Identity 2:0
        /// </summary>
        [VectorType(1, 3, 28, 40, 10)]
        [ColumnName("onnx::Sigmoid_799")]
        public float[] Identity2 { get; set; }
        
        [VectorType(1, 3, 14, 20, 10)]
        [ColumnName("onnx::Sigmoid_1071")]
        public float[] Identity3 { get; set; }
        */
        [ColumnName("width")]
        public float ImageWidth { get; set; }

        [ColumnName("height")]
        public float ImageHeight { get; set; }

        public IReadOnlyList<Result> GetResults(string[] categories, string[] filter)
        {
            var postProcesssedBoundingBoxes = PostProcessBoundingBoxes(new[] { Identity,  Identity1, Identity2 }, categories.Length);
            return NMS(postProcesssedBoundingBoxes, categories,filter);
        }

        /// <summary>
        /// This method is postprocess_bbbox()
        /// Ported from https://github.com/onnx/models/tree/master/vision/object_detection_segmentation/yolov4#postprocessing-steps
        /// Thans to the help of: https://github.com/BobLd/YOLOv4MLNet/blob/master/YOLOv4MLNet/DataStructures/YoloV4Prediction.cs
        /// </summary>
        /// <returns></returns>
        private List<float[]> PostProcessBoundingBoxes(float[][] results, int classesCount)
        {
            List<float[]> postProcesssedResults = new List<float[]>();

            for (int i = 0; i < results.Length; i++)
            {
                var pred = results[i];
                var outputSize = SHAPES[i];

                for (int boxY = 0; boxY < outputSize; boxY++)
                {
                    for (int boxX = 0; boxX < outputSize; boxX++)
                    {
                        for (int a = 0; a < _anchorsCount; a++)
                        {
                            var offset = (boxY * outputSize * (classesCount + 5) * _anchorsCount) + (boxX * (classesCount + 5) * _anchorsCount) + a * (classesCount + 5);
                            var predBbox = pred.Skip(offset).Take(classesCount + 5).ToArray();

                            var predXywh = predBbox.Take(4).ToArray();
                            var predConf = predBbox[4];
                            var predProb = predBbox.Skip(5).ToArray();

                            var rawDx = predXywh[0];
                            var rawDy = predXywh[1];
                            var rawDw = predXywh[2];
                            var rawDh = predXywh[3];

                            float predX = ((Sigmoid(rawDx) * XYSCALE[i]) - 0.5f * (XYSCALE[i] - 1) + boxX) * STRIDES[i];
                            float predY = ((Sigmoid(rawDy) * XYSCALE[i]) - 0.5f * (XYSCALE[i] - 1) + boxY) * STRIDES[i];
                            float predW = (float)Math.Exp(rawDw) * ANCHORS[i][a][0];
                            float predH = (float)Math.Exp(rawDh) * ANCHORS[i][a][1];

                            // (x, y, w, h) --> (xmin, ymin, xmax, ymax)
                            float predX1 = predX - predW * 0.5f;
                            float predY1 = predY - predH * 0.5f;
                            float predX2 = predX + predW * 0.5f;
                            float predY2 = predY + predH * 0.5f;

                            // (xmin, ymin, xmax, ymax) -> (xmin_org, ymin_org, xmax_org, ymax_org)
                            float org_h = ImageHeight;
                            float org_w = ImageWidth;

                            float inputSize = 416f;
                            float resizeRatio = Math.Min(inputSize / org_w, inputSize / org_h);
                            float dw = (inputSize - resizeRatio * org_w) / 2f;
                            float dh = (inputSize - resizeRatio * org_h) / 2f;

                            var orgX1 = 1f * (predX1 - dw) / resizeRatio;
                            var orgX2 = 1f * (predX2 - dw) / resizeRatio;
                            var orgY1 = 1f * (predY1 - dh) / resizeRatio;
                            var orgY2 = 1f * (predY2 - dh) / resizeRatio;

                            // Clip boxes that are out of range
                            orgX1 = Math.Max(orgX1, 0);
                            orgY1 = Math.Max(orgY1, 0);
                            orgX2 = Math.Min(orgX2, org_w - 1);
                            orgY2 = Math.Min(orgY2, org_h - 1);

                            if (orgX1 > orgX2 || orgY1 > orgY2)
                            {
                                continue;
                            }

                            // Discard boxes with low scores
                            var scores = predProb.Select(p => p * predConf).ToList();

                            float scoreMaxCat = scores.Max();
                            if (scoreMaxCat > _scoreThreshold)
                            {
                                postProcesssedResults.Add(new float[] { orgX1, orgY1, orgX2, orgY2, scoreMaxCat, scores.IndexOf(scoreMaxCat) });
                            }
                        }
                    }
                }
            }

            return postProcesssedResults;
        }

        /// <summary>
        /// Performs Non-Maximum Suppression.
        /// </summary>
        /// <returns>List of Results</returns>
        private List<Result> NMS(List<float[]> postProcesssedBoundingBoxes, string[] categories, string[] filter)
        {
            postProcesssedBoundingBoxes = postProcesssedBoundingBoxes.OrderByDescending(x => x[4]).ToList();
            var resultsNms = new List<Result>();

            int counter = 0;
            while (counter < postProcesssedBoundingBoxes.Count)
            {
                var result = postProcesssedBoundingBoxes[counter];
                if (result == null)
                {
                    counter++;
                    continue;
                }

                var confidence = result[4];
                string label = categories[(int)result[5]];
                if (filter.Contains(label))
                {
                    resultsNms.Add(new Result(result.Take(4).ToArray(), label, confidence));

                    postProcesssedBoundingBoxes[counter] = null;

                    var iou = postProcesssedBoundingBoxes.Select(bbox => bbox == null ? float.NaN : BoxIoU(result, bbox)).ToList();

                    for (int i = 0; i < iou.Count; i++)
                    {
                        if (float.IsNaN(iou[i]))
                        {
                            continue;
                        }

                        if (iou[i] > _iouThreshold)
                        {
                            postProcesssedBoundingBoxes[i] = null;
                        }
                    }
                }
                counter++;
            }

            return resultsNms;
        }

        /// <summary>
        /// Intersection-over-union (Jaccard index) of boxes.
        /// </summary>
        private float BoxIoU(float[] boxes1, float[] boxes2)
        {
            var area1 = GetBoxArea(boxes1);
            var area2 = GetBoxArea(boxes2);

            var dx = Math.Max(0, Math.Min(boxes1[2], boxes2[2]) - Math.Max(boxes1[0], boxes2[0]));
            var dy = Math.Max(0, Math.Min(boxes1[3], boxes2[3]) - Math.Max(boxes1[1], boxes2[1]));

            return (dx * dy) / (area1 + area2 - (dx * dy));
        }

        private float GetBoxArea(float[] box)
        {
            return (box[2] - box[0]) * (box[3] - box[1]);
        }

        private float Sigmoid(float x)
        {
            return 1f / (1f + (float)Math.Exp(-x));
        }
    }
}
