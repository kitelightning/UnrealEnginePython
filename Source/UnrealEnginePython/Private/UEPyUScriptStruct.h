#pragma once

#include "UnrealEnginePython.h"

class UScriptStruct;

typedef struct
{
	PyObject_HEAD
		/* Type-specific fields go here. */
		UScriptStruct *u_struct;
	uint8 *u_struct_ptr;
	// if set, the struct is responsible for freeing memory
	int u_struct_owned;
} ue_PyUScriptStruct;

PyObject *py_ue_new_uscriptstruct(UScriptStruct *, uint8 *);
PyObject *py_ue_new_owned_uscriptstruct(UScriptStruct *, uint8 *);
PyObject *py_ue_new_owned_uscriptstruct_zero_copy(UScriptStruct *, uint8 *);
ue_PyUScriptStruct *py_ue_is_uscriptstruct(PyObject *);

inline static uint8* py_ue_uscriptstruct_get_data(ue_PyUScriptStruct *py_u_struct)
{
    return (uint8*)(py_u_struct->is_ptr ? py_u_struct->original_data : py_u_struct->data);
}

UProperty *ue_struct_get_field_from_name(UScriptStruct *, char *);

void ue_python_init_uscriptstruct(PyObject *);