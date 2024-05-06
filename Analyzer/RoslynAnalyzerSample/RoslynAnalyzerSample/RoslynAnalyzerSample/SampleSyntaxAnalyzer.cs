using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace RoslynAnalyzerSample;

//class名がSampleならエラーを吐く
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class SampleSyntaxAnalyzer : DiagnosticAnalyzer
{
	private static readonly DiagnosticDescriptor Rule = new("SampleError01", "NamingError", "命名にSampleを使用した", "Naming",
		DiagnosticSeverity.Error, isEnabledByDefault: true, description: "サンプルエラー、名前にSampleを使わないでください");

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
		ImmutableArray.Create(Rule);

	public override void Initialize(AnalysisContext context)
	{
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
		context.EnableConcurrentExecution();

		context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.ClassDeclaration);
	}

	private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
	{
		if (context.Node is not ClassDeclarationSyntax classDeclarationNode)
			return;

		var classDeclarationIdentifier = classDeclarationNode.Identifier;
		if (classDeclarationIdentifier.Text == "Sample")
		{
			var diagnostic = Diagnostic.Create(Rule,
				classDeclarationIdentifier.GetLocation(),
				classDeclarationIdentifier.Text);

			context.ReportDiagnostic(diagnostic);
		}
	}
}