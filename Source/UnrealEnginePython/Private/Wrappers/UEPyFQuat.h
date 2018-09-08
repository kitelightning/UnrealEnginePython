#pragma once

#include "UEPyUScriptStruct.h"
#include "Runtime/Core/Public/Math/Quat.h"

extern PyTypeObject ue_PyUScriptStructType;

typedef struct {
    ue_PyUScriptStruct py_base;
} ue_PyFQuat;

PyObject *py_ue_new_fquat(const FQuat&);
PyObject *py_ue_new_fquat_ptr(FQuat*);
ue_PyFQuat *py_ue_is_fquat(PyObject *);

inline static FQuat& py_ue_fquat_get(ue_PyFQuat *self)
{
    return *((FQuat*)((ue_PyUScriptStruct*)self)->u_struct_ptr);
}

void ue_python_init_fquat(PyObject *);

bool py_ue_quat_arg(PyObject *, FQuat &);