using System.Reflection;

[assembly: AssemblyProduct("GenericQueryable")]
[assembly: AssemblyCompany("IvAt")]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("changes at build")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif