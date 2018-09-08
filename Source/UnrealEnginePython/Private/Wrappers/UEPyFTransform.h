#pragma once



#include "UEPyUScriptStruct.h"

extern PyTypeObject ue_PyUScriptStructType;

typedef struct {
    ue_PyUScriptStruct py_base;
} ue_PyFTransform;

PyObject *py_ue_new_ftransform(const FTransform&);
PyObject *py_ue_new_ftransform_ptr(FTransform*);
ue_PyFTransform *py_ue_is_ftransform(PyObject *);

inline static FTransform& py_ue_ftransform_get(ue_PyFTransform *self)
{
    return *((FTransform*)((ue_PyUScriptStruct *)self)->u_struct_ptr);
}

void ue_python_init_ftransform(PyObject *);
bool py_ue_transform_arg(PyObject *, FTransform &);