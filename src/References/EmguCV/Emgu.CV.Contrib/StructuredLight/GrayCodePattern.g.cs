//----------------------------------------------------------------------------
//  This file is automatically generated, do not modify.      
//----------------------------------------------------------------------------



using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.StructuredLight
{
   public static partial class StructuredLightInvoke
   {

     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern int cveGrayCodePatternGetNumberOfPatternImages(IntPtr obj);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveGrayCodePatternSetWhiteThreshold(
        IntPtr obj,  
        int val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveGrayCodePatternSetBlackThreshold(
        IntPtr obj,  
        int val);
     
   }

   public partial class GrayCodePattern
   {

     /// <summary>
    /// Get the number of pattern images needed for the graycode pattern
     /// </summary>
     public int NumberOfPatternImages
     {
        get { return StructuredLightInvoke.cveGrayCodePatternGetNumberOfPatternImages(_ptr); } 
     }
     
     /// <summary>
     /// White threshold is a number between 0-255 that represents the minimum brightness difference required for valid pixels, between the graycode pattern and its inverse images, used in getProjPixel method
     /// </summary>
	 /// <param name="value">The value</param>
     public void SetWhiteThreshold(int value)
     {
        StructuredLightInvoke.cveGrayCodePatternSetWhiteThreshold(_ptr, value); 
     }
     
     /// <summary>
     /// Black threshold is a number between 0-255 that represents the minimum brightness difference required for valid pixels, between the fully illuminated (white) and the not illuminated images (black), used in computeShadowMasks method
     /// </summary>
	 /// <param name="value">The value</param>
     public void SetBlackThreshold(int value)
     {
        StructuredLightInvoke.cveGrayCodePatternSetBlackThreshold(_ptr, value); 
     }
     
   }
}
