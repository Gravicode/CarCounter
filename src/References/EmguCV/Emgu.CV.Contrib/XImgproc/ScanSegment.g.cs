//----------------------------------------------------------------------------
//  This file is automatically generated, do not modify.      
//----------------------------------------------------------------------------



using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.XImgproc
{
   public static partial class XImgprocInvoke
   {

     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern int cveScanSegmentGetNumberOfSuperpixels(IntPtr obj);
     
   }

   public partial class ScanSegment
   {

     /// <summary>
    /// Returns the actual superpixel segmentation from the last image processed using iterate. Returns zero if no image has been processed.
     /// </summary>
     public int NumberOfSuperpixels
     {
        get { return XImgprocInvoke.cveScanSegmentGetNumberOfSuperpixels(_ptr); } 
     }
     
   }
}
