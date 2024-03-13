using System.Reflection;

namespace WebApi.Services;

public static class AssemblyService {
	public static string GetAssemblyName() {
		return Assembly.GetExecutingAssembly().GetName().Name
			?? throw new NullReferenceException("AssemblyName");
	}
}
