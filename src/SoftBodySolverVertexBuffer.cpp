#include "StdAfx.h"

#ifndef DISABLE_SOFTBODY

#include "Collections.h"
#include "SoftBodySolverVertexBuffer.h"

SoftBody::VertexBufferDescriptor::VertexBufferDescriptor(btVertexBufferDescriptor* buffer)
{
	_buffer = buffer;
}

btVertexBufferDescriptor* SoftBody::VertexBufferDescriptor::UnmanagedPointer::get()
{
	return _buffer;
}
void SoftBody::VertexBufferDescriptor::UnmanagedPointer::set(btVertexBufferDescriptor* value)
{
	_buffer = value;
}


SoftBody::CpuVertexBufferDescriptor::CpuVertexBufferDescriptor(FloatArray^ array, int vertexOffset, int vertexStride, int normalOffset, int normalStride)
: VertexBufferDescriptor(new btCPUVertexBufferDescriptor((array != nullptr) ? array->UnmanagedPointer : 0, vertexOffset, vertexStride, normalOffset, normalStride))
{
}

btCPUVertexBufferDescriptor* SoftBody::CpuVertexBufferDescriptor::UnmanagedPointer::get()
{
	return (btCPUVertexBufferDescriptor*) VertexBufferDescriptor::UnmanagedPointer;
}

#endif
