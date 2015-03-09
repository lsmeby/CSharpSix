using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Formatting;

namespace IfElseBracketDiagnostic
{
    [ExportCodeFixProvider("IfElseBracketDiagnosticCodeFixProvider", LanguageNames.CSharp), Shared]
    public class IfElseBracketDiagnosticCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> GetFixableDiagnosticIds()
        {
            return ImmutableArray.Create(IfElseBracketDiagnosticAnalyzer.DiagnosticId);
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task ComputeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var errorToken = root.FindToken(diagnosticSpan.Start).Parent;

            // Register a code action that will invoke the fix.
            context.RegisterFix(
                CodeAction.Create("Add brackets", c => AddBracketsAsync(context.Document, errorToken, c)),
                diagnostic);
        }

        private async Task<Document> AddBracketsAsync(Document document, SyntaxNode errorStatement, CancellationToken cancellationToken)
        {
            SyntaxNode toReplace = null;

            if (errorStatement is IfStatementSyntax)
            {
                var newIfStatement = errorStatement as IfStatementSyntax;
                newIfStatement = newIfStatement.WithStatement(SyntaxFactory.Block(newIfStatement.Statement)).WithAdditionalAnnotations(Formatter.Annotation);
                toReplace = newIfStatement;
            }
            else if (errorStatement is ElseClauseSyntax)
            {
                var newElseStatement = errorStatement as ElseClauseSyntax;
                newElseStatement = newElseStatement.WithStatement(SyntaxFactory.Block(newElseStatement.Statement)).WithAdditionalAnnotations(Formatter.Annotation);
                toReplace = newElseStatement;
            }

            if (toReplace != null)
            {
                var root = await document.GetSyntaxRootAsync(cancellationToken);
                var newRoot = root.ReplaceNode(errorStatement, toReplace);
                var newDocument = document.WithSyntaxRoot(newRoot);
                return newDocument;
            }
            else
            {
                return null;
            }
        }
    }
}