﻿//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//  Vector of VectorOfPoint3D32F
//
//  This file is automatically generated, do not modify.
//----------------------------------------------------------------------------



using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Structure;

namespace Emgu.CV.Util
{
   /// <summary>
   /// Wrapped class of the C++ standard vector of VectorOfPoint3D32F.
   /// </summary>
   [Serializable]
   [DebuggerTypeProxy(typeof(VectorOfVectorOfPoint3D32F.DebuggerProxy))]
   public partial class VectorOfVectorOfPoint3D32F : Emgu.CV.Util.UnmanagedVector
#if true
      , IInputOutputArray
#endif
   {
      private readonly bool _needDispose;
   
      static VectorOfVectorOfPoint3D32F()
      {
         CvInvoke.Init();
      }

      /// <summary>
      /// Create an empty standard vector of VectorOfPoint3D32F
      /// </summary>
      public VectorOfVectorOfPoint3D32F()
         : this(VectorOfVectorOfPoint3D32FCreate(), true)
      {
      }

      internal VectorOfVectorOfPoint3D32F(IntPtr ptr, bool needDispose)
      {
         _ptr = ptr;
         _needDispose = needDispose;
      }

      /// <summary>
      /// Create an standard vector of VectorOfPoint3D32F of the specific size
      /// </summary>
      /// <param name="size">The size of the vector</param>
      public VectorOfVectorOfPoint3D32F(int size)
         : this( VectorOfVectorOfPoint3D32FCreateSize(size), true)
      {
      }

      /// <summary>
      /// Create an standard vector of VectorOfPoint3D32F with the initial values
      /// </summary>
      /// <param name="values">The initial values</param>
      public VectorOfVectorOfPoint3D32F(params VectorOfPoint3D32F[] values)
        : this()
      {
        Push(values);
      }

      /// <summary>
      /// Get the size of the vector
      /// </summary>
      public override int Size
      {
         get
         {
            return VectorOfVectorOfPoint3D32FGetSize(_ptr);
         }
      }
	  
	  /// <summary>
      /// The pointer to the first element on the vector. In case of an empty vector, IntPtr.Zero will be returned.
      /// </summary>
      public override IntPtr StartAddress
      {
         get
         {
            return VectorOfVectorOfPoint3D32FGetStartAddress(_ptr);
         }
      }
	  
	  /// <summary>
      /// The pointer to memory address at the end of the vector. In case of an empty vector, IntPtr.Zero will be returned.
      /// </summary>
      public override IntPtr EndAddress
      {
         get
         {
            return VectorOfVectorOfPoint3D32FGetEndAddress(_ptr);
         }
      }

      /// <summary>
      /// Clear the vector
      /// </summary>
      public void Clear()
      {
         VectorOfVectorOfPoint3D32FClear(_ptr);
      }

      /// <summary>
      /// Push a value into the standard vector
      /// </summary>
      /// <param name="value">The value to be pushed to the vector</param>
      public void Push(VectorOfPoint3D32F value)
      {
         VectorOfVectorOfPoint3D32FPush(_ptr, value.Ptr);
      }

      /// <summary>
      /// Push multiple values into the standard vector
      /// </summary>
      /// <param name="values">The values to be pushed to the vector</param>
      public void Push(VectorOfPoint3D32F[] values)
      {
         foreach (VectorOfPoint3D32F value in values)
            Push(value);
      }

      /// <summary>
      /// Push multiple values from the other vector into this vector
      /// </summary>
      /// <param name="other">The other vector, from which the values will be pushed to the current vector</param>
      public void Push(VectorOfVectorOfPoint3D32F other)
      {
         VectorOfVectorOfPoint3D32FPushVector(_ptr, other);
      }
      
      /// <summary>
      /// Get the item in the specific index
      /// </summary>
      /// <param name="index">The index</param>
      /// <returns>The item in the specific index</returns>
      public VectorOfPoint3D32F this[int index]
      {
         get
         {
            IntPtr itemPtr = IntPtr.Zero;
            VectorOfVectorOfPoint3D32FGetItemPtr(_ptr, index, ref itemPtr);
            return new VectorOfPoint3D32F(itemPtr, false);
         }
      }

      /// <summary>
      /// Release the standard vector
      /// </summary>
      protected override void DisposeObject()
      {
         if (_needDispose && _ptr != IntPtr.Zero)
            VectorOfVectorOfPoint3D32FRelease(ref _ptr);
      }

#if true
      /// <summary>
      /// Get the pointer to cv::_InputArray
      /// </summary>
      /// <returns>The input array</returns>
      public InputArray GetInputArray()
      {
        return new InputArray( cveInputArrayFromVectorOfVectorOfPoint3D32F(_ptr), this );
      }

      /// <summary>
      /// Get the pointer to cv::_OutputArray
      /// </summary>
      /// <returns>The output array</returns>
      public OutputArray GetOutputArray()
      {
         return new OutputArray( cveOutputArrayFromVectorOfVectorOfPoint3D32F(_ptr), this );
      }

      /// <summary>
      /// Get the pointer to cv::_InputOutputArray
      /// </summary>
      /// <returns>The input output array</returns>
      public InputOutputArray GetInputOutputArray()
      {
         return new InputOutputArray( cveInputOutputArrayFromVectorOfVectorOfPoint3D32F(_ptr), this );
      }
#endif
      
      /// <summary>
      /// The size of the item in this Vector, counted as size in bytes.
      /// </summary>
      public static int SizeOfItemInBytes
      {
         get { return VectorOfVectorOfPoint3D32FSizeOfItemInBytes(); }
      }

#if true
      /// <summary>
      /// Create the standard vector of VectorOfPoint3D32F 
      /// </summary>
      /// <param name="values">The values to be pushed to the vector</param>
      public VectorOfVectorOfPoint3D32F(MCvPoint3D32f[][] values)
         : this()
      {
         using (VectorOfPoint3D32F v = new VectorOfPoint3D32F())
         {
            for (int i = 0; i < values.Length; i++)
            {
               v.Push(values[i]);
               Push(v);
               v.Clear();
            }
         }
      }

      /// <summary>
      /// Convert the standard vector to arrays of arrays of MCvPoint3D32f
      /// </summary>
      /// <returns>Arrays of arrays of the MCvPoint3D32f</returns>
      public MCvPoint3D32f[][] ToArrayOfArray()
      {
         int size = Size;
         MCvPoint3D32f[][] res = new MCvPoint3D32f[size][];
         for (int i = 0; i < size; i++)
         {
            using (VectorOfPoint3D32F v = this[i])
            {
               res[i] = v.ToArray();
            }
         }
         return res;
      }
#endif

      internal class DebuggerProxy
      {
         private VectorOfVectorOfPoint3D32F _v;

         public DebuggerProxy(VectorOfVectorOfPoint3D32F v)
         {
            _v = v;
         }

#if true
         public MCvPoint3D32f[][] Values
         {
            get { return _v.ToArrayOfArray(); }
         }
#else
         public VectorOfPoint3D32F[] Values
         {
            get
            {
               VectorOfPoint3D32F[] result = new VectorOfPoint3D32F[_v.Size];
               for (int i = 0; i < result.Length; i++)
               {
                  result[i] = _v[i];
               }
               return result;
            }
         }
#endif
      }


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfVectorOfPoint3D32FCreate();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfVectorOfPoint3D32FCreateSize(int size);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfVectorOfPoint3D32FRelease(ref IntPtr v);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfVectorOfPoint3D32FGetSize(IntPtr v);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfVectorOfPoint3D32FGetStartAddress(IntPtr v);
	  
	  [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfVectorOfPoint3D32FGetEndAddress(IntPtr v);
	  
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfVectorOfPoint3D32FPush(IntPtr v, IntPtr value);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfVectorOfPoint3D32FPushVector(IntPtr ptr, IntPtr otherPtr);
      
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfVectorOfPoint3D32FClear(IntPtr v);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfVectorOfPoint3D32FGetItemPtr(IntPtr vec, int index, ref IntPtr element);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfVectorOfPoint3D32FSizeOfItemInBytes();

#if true
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveInputArrayFromVectorOfVectorOfPoint3D32F(IntPtr vec);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveOutputArrayFromVectorOfVectorOfPoint3D32F(IntPtr vec);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveInputOutputArrayFromVectorOfVectorOfPoint3D32F(IntPtr vec);
#endif
   }
}


