using System.Threading.Tasks;
using Xunit;
using Verifier =
	Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
		RoslynAnalyzerSample.SampleSyntaxAnalyzer>;

namespace RoslynAnalyzerSample.Tests;

public class SampleSyntaxAnalyzerTests
{
	[Fact]
	public async Task ClassWithMyCompanyTitle_AlertDiagnostic()
	{
		const string text = @"
public class Sample
{
}
";

		var expected = Verifier.Diagnostic()
			.WithLocation(2, 14)
			.WithArguments("Sample");
		await Verifier.VerifyAnalyzerAsync(text, expected).ConfigureAwait(false);
	}
}