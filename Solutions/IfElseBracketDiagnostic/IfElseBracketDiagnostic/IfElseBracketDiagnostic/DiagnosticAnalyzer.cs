using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IfElseBracketDiagnostic
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class IfElseBracketDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "IfElseBracketDiagnostic";
        internal const string Title = "If and else statements must use braces";
        internal const string MessageFormat = "'{0}' statements must have braces";
        internal const string Category = "Formatting";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeIfElseStatement, SyntaxKind.IfStatement, SyntaxKind.ElseClause);
        }

        private static void AnalyzeIfElseStatement(SyntaxNodeAnalysisContext context)
        {
            var ifStatement = context.Node as IfStatementSyntax;

            if (ifStatement != null 
                && ifStatement.Statement != null 
                && ifStatement.Statement.IsKind(SyntaxKind.Block) == false)
            {
                var diagnostic = Diagnostic.Create(Rule, ifStatement.GetLocation(), "if");
                context.ReportDiagnostic(diagnostic);
            }

            var elseSyntax = context.Node as ElseClauseSyntax;

            if (elseSyntax != null
                && elseSyntax.Statement != null
                && elseSyntax.Statement.IsKind(SyntaxKind.IfStatement) == false
                && elseSyntax.Statement.IsKind(SyntaxKind.Block) == false)
            {
                var diagnostic = Diagnostic.Create(Rule, elseSyntax.GetLocation(), "else");
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
