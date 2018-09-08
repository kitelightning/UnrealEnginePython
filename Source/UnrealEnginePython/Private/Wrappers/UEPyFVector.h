#pragma once



#include "UEPyUScriptStruct.h"

extern PyTypeObject ue_PyUScriptStructType;

typedef struct {
    ue_PyUScriptStruct py_base;
} ue_PyFVector;

PyObject *py_ue_new_fvector(const FVector&);
PyObject *py_ue_new_fvector_ptr(FVector*);
ue_PyFVector *py_ue_is_fvector(PyObject *);

inline static FVector& py_ue_fvector_get(ue_PyFVector *self)
{
    return *((FVector*)((ue_PyUScriptStruct *)self)->u_struct_ptr);
}

void ue_python_init_fvector(PyObject *);

bool py_ue_vector_arg(PyObject *, FVector &);