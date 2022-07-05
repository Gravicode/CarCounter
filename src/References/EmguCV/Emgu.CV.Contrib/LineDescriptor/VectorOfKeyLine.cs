﻿//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//  Vector of KeyLine
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

namespace Emgu.CV.LineDescriptor
{
   /// <summary>
   /// Wrapped class of the C++ standard vector of KeyLine.
   /// </summary>
   [Serializable]
   [DebuggerTypeProxy(typeof(VectorOfKeyLine.DebuggerProxy))]
   public partial class VectorOfKeyLine : Emgu.CV.Util.UnmanagedVector, ISerializable
#if false
      , IInputOutputArray
#endif
   {
      private readonly bool _needDispose;
   
      static VectorOfKeyLine()
      {
         CvInvoke.Init();
         Debug.Assert(Emgu.Util.Toolbox.SizeOf<MKeyLine>() == SizeOfItemInBytes, "Size do not match");
      }

      /// <summary>
      /// Constructor used to deserialize runtime serialized object
      /// </summary>
      /// <param name="info">The serialization info</param>
      /// <param name="context">The streaming context</param>
      public VectorOfKeyLine(SerializationInfo info, StreamingContext context)
         : this()
      {
         Push((MKeyLine[])info.GetValue("KeyLineArray", typeof(MKeyLine[])));
      }
	  
      /// <summary>
      /// A function used for runtime serialization of the object
      /// </summary>
      /// <param name="info">Serialization info</param>
      /// <param name="context">Streaming context</param>
      public void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.AddValue("KeyLineArray", ToArray());
      }

      /// <summary>
      /// Create an empty standard vector of KeyLine
      /// </summary>
      public VectorOfKeyLine()
         : this(VectorOfKeyLineCreate(), true)
      {
      }
	  
      internal VectorOfKeyLine(IntPtr ptr, bool needDispose)
      {
         _ptr = ptr;
         _needDispose = needDispose;
      }

      /// <summary>
      /// Create an standard vector of KeyLine of the specific size
      /// </summary>
      /// <param name="size">The size of the vector</param>
      public VectorOfKeyLine(int size)
         : this( VectorOfKeyLineCreateSize(size), true)
      {
      }
	  
      /// <summary>
      /// Create an standard vector of KeyLine with the initial values
      /// </summary>
      /// <param name="values">The initial values</param>
      public VectorOfKeyLine(MKeyLine[] values)
         :this()
      {
         Push(values);
      }
	  
      /// <summary>
      /// Push an array of value into the standard vector
      /// </summary>
      /// <param name="value">The value to be pushed to the vector</param>
      public void Push(MKeyLine[] value)
      {
         if (value.Length > 0)
         {
            GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            VectorOfKeyLinePushMulti(_ptr, handle.AddrOfPinnedObject(), value.Length);
            handle.Free();
         }
      }
      
      /// <summary>
      /// Push multiple values from the other vector into this vector
      /// </summary>
      /// <param name="other">The other vector, from which the values will be pushed to the current vector</param>
      public void Push(VectorOfKeyLine other)
      {
         VectorOfKeyLinePushVector(_ptr, other);
      }
	  
      /// <summary>
      /// Convert the standard vector to an array of KeyLine
      /// </summary>
      /// <returns>An array of KeyLine</returns>
      public MKeyLine[] ToArray()
      {
         MKeyLine[] res = new MKeyLine[Size];
         if (res.Length > 0)
         {
            GCHandle handle = GCHandle.Alloc(res, GCHandleType.Pinned);
            VectorOfKeyLineCopyData(_ptr, handle.AddrOfPinnedObject());
            handle.Free();
         }
         return res;
      }

      /// <summary>
      /// Get the size of the vector
      /// </summary>
      public override int Size
      {
         get
         {
            return VectorOfKeyLineGetSize(_ptr);
         }
      }

      /// <summary>
      /// Clear the vector
      /// </summary>
      public void Clear()
      {
         VectorOfKeyLineClear(_ptr);
      }

      /// <summary>
      /// The pointer to the first element on the vector. In case of an empty vector, IntPtr.Zero will be returned.
      /// </summary>
      public override IntPtr StartAddress
      {
         get
         {
            return VectorOfKeyLineGetStartAddress(_ptr);
         }
      }
	  
	  /// <summary>
      /// The pointer to memory address at the end of the vector. In case of an empty vector, IntPtr.Zero will be returned.
      /// </summary>
      public override IntPtr EndAddress
      {
         get
         {
            return VectorOfKeyLineGetEndAddress(_ptr);
         }
      }
	  
      /// <summary>
      /// Get the item in the specific index
      /// </summary>
      /// <param name="index">The index</param>
      /// <returns>The item in the specific index</returns>
      public MKeyLine this[int index]
      {
         get
         {
            MKeyLine result = new MKeyLine();
            VectorOfKeyLineGetItem(_ptr, index, ref result);
            return result;
         }
      }

      /// <summary>
      /// Release the standard vector
      /// </summary>
      protected override void DisposeObject()
      {
         if (_needDispose && _ptr != IntPtr.Zero)
            VectorOfKeyLineRelease(ref _ptr);
      }

#if false
      /// <summary>
      /// Get the data as InputArray
      /// </summary>
      /// <returns>The input array </returns>
      public InputArray GetInputArray()
      {
         return new InputArray( cveInputArrayFromVectorOfKeyLine(_ptr), this );
      }
	  
      /// <summary>
      /// Get the data as OutputArray
      /// </summary>
      /// <returns>The output array </returns>
      public OutputArray GetOutputArray()
      {
         return new OutputArray( cveOutputArrayFromVectorOfKeyLine(_ptr), this );
      }

      /// <summary>
      /// Get the data as InputOutputArray
      /// </summary>
      /// <returns>The input output array </returns>
      public InputOutputArray GetInputOutputArray()
      {
         return new InputOutputArray( cveInputOutputArrayFromVectorOfKeyLine(_ptr), this );
      }
#endif
      
      /// <summary>
      /// The size of the item in this Vector, counted as size in bytes.
      /// </summary>
      public static int SizeOfItemInBytes
      {
         get { return VectorOfKeyLineSizeOfItemInBytes(); }
      }
	  
      internal class DebuggerProxy
      {
         private VectorOfKeyLine _v;

         public DebuggerProxy(VectorOfKeyLine v)
         {
            _v = v;
         }

         public MKeyLine[] Values
         {
            get { return _v.ToArray(); }
         }
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfKeyLineCreate();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfKeyLineCreateSize(int size);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfKeyLineRelease(ref IntPtr v);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfKeyLineGetSize(IntPtr v);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfKeyLineCopyData(IntPtr v, IntPtr data);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfKeyLineGetStartAddress(IntPtr v);
	  
	  [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfKeyLineGetEndAddress(IntPtr v);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfKeyLinePushMulti(IntPtr v, IntPtr values, int count);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfKeyLinePushVector(IntPtr ptr, IntPtr otherPtr);
      
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfKeyLineClear(IntPtr v);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfKeyLineGetItem(IntPtr vec, int index, ref MKeyLine element);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfKeyLineSizeOfItemInBytes();

#if false      
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveInputArrayFromVectorOfKeyLine(IntPtr vec);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveOutputArrayFromVectorOfKeyLine(IntPtr vec);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveInputOutputArrayFromVectorOfKeyLine(IntPtr vec);
#endif
   }
}


