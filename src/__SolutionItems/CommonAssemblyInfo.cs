using System.Reflection;

[assembly: AssemblyProduct("GenericQueryable")]
[assembly: AssemblyCompany("IvAt")]

[assembly: AssemblyVersion("1.3.0.4")]
[assembly: AssemblyInformationalVersion("changes at build")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif