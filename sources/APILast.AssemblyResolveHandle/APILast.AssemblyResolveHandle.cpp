#include "stdafx.h"
#include "APILast.AssemblyResolveHandle.h"

System::Reflection::Assembly ^ OnAssemblyResolve(System::Object ^sender, System::ResolveEventArgs ^args);

extern "C" __declspec(dllexport) void __stdcall RegisterHandler()
{
	AppDomain::CurrentDomain->AssemblyResolve += gcnew System::ResolveEventHandler(&OnAssemblyResolve);
}

System::Reflection::Assembly ^ OnAssemblyResolve(System::Object ^sender, System::ResolveEventArgs ^args)
{
	auto nameParts = args->Name->Split(',');
	auto name = nameParts[0];

	String ^fullPath = "D:\\Develop\\KleinZeug\\InterProcessCommunication\\ListProcess\\REST\\bin\\Debug\\" + name + ".dll";
	Console::WriteLine("Native Hello " + fullPath);
	Console::WriteLine("Location: " + args->RequestingAssembly->Location);
	auto assembly = System::Reflection::Assembly::LoadFile(fullPath);
	Console::WriteLine("Native Goodbye Assembly didnt throw");

	return assembly;
}
