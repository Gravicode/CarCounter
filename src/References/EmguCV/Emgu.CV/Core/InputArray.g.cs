//----------------------------------------------------------------------------
//  This file is automatically generated, do not modify.      
//----------------------------------------------------------------------------



using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{
   public static partial class CvInvoke
   {

     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     [return: MarshalAs(CvInvoke.BoolMarshalType)]
     internal static extern bool cveInputArrayIsMat(IntPtr obj);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     [return: MarshalAs(CvInvoke.BoolMarshalType)]
     internal static extern bool cveInputArrayIsUMat(IntPtr obj);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     [return: MarshalAs(CvInvoke.BoolMarshalType)]
     internal static extern bool cveInputArrayIsMatVector(IntPtr obj);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     [return: MarshalAs(CvInvoke.BoolMarshalType)]
     internal static extern bool cveInputArrayIsUMatVector(IntPtr obj);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     [return: MarshalAs(CvInvoke.BoolMarshalType)]
     internal static extern bool cveInputArrayIsMatx(IntPtr obj);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern InputArray.Type cveInputArrayKind(IntPtr obj);
     
   }

   public partial class InputArray
   {

     /// <summary>
     /// True if the input array is a Mat
     /// </summary>
     public bool IsMat
     {
        get { return CvInvoke.cveInputArrayIsMat(_ptr); } 
     }
     
     /// <summary>
     /// True if the input array is an UMat
     /// </summary>
     public bool IsUMat
     {
        get { return CvInvoke.cveInputArrayIsUMat(_ptr); } 
     }
     
     /// <summary>
     /// True if the input array is a vector of Mat
     /// </summary>
     public bool IsMatVector
     {
        get { return CvInvoke.cveInputArrayIsMatVector(_ptr); } 
     }
     
     /// <summary>
     /// True if the input array is a vector of UMat
     /// </summary>
     public bool IsUMatVector
     {
        get { return CvInvoke.cveInputArrayIsUMatVector(_ptr); } 
     }
     
     /// <summary>
     /// True if the input array is a Matx
     /// </summary>
     public bool IsMatx
     {
        get { return CvInvoke.cveInputArrayIsMatx(_ptr); } 
     }
     
     /// <summary>
     /// The type of the input array
     /// </summary>
     public InputArray.Type Kind
     {
        get { return CvInvoke.cveInputArrayKind(_ptr); } 
     }
     
   }
}
